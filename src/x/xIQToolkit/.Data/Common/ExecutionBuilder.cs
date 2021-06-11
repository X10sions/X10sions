using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit.Data.Common {
  /// <summary>
  /// Builds an execution plan for a query expression.
  /// </summary>
  public class ExecutionBuilder : DbExpressionVisitor {
    private readonly QueryPolicy policy;
    private readonly QueryLinguist linguist;
    private readonly Expression executor;
    private Scope scope;
    private bool isTop = true;
    private MemberInfo receivingMember;
    private int nReaders = 0;
    private readonly List<ParameterExpression> variables = new List<ParameterExpression>();
    private readonly List<Expression> initializers = new List<Expression>();
    private Dictionary<string, Expression> variableMap = new Dictionary<string, Expression>();

    private ExecutionBuilder(QueryLinguist linguist, QueryPolicy policy, Expression executor) {
      this.linguist = linguist;
      this.policy = policy;
      this.executor = executor;
    }

    public static Expression Build(QueryLinguist linguist, QueryPolicy policy, Expression expression, Expression provider) {
      var executor = Expression.Parameter(typeof(QueryExecutor), "executor");
      var builder = new ExecutionBuilder(linguist, policy, executor);
      builder.variables.Add(executor);
      builder.initializers.Add(Expression.Call(Expression.Convert(provider, typeof(IQueryExecutorFactory)), "CreateExecutor", null, null));
      var result = builder.Build(expression);
      return result;
    }

    private Expression Build(Expression expression) {
      expression = Visit(expression);
      expression = AddVariables(expression);
      return expression;
    }

    private Expression AddVariables(Expression expression) {
      // add variable assignments up front
      if (variables.Count > 0) {
        var exprs = new List<Expression>();
        for (int i = 0, n = variables.Count; i < n; i++) {
          exprs.Add(MakeAssign(variables[i], initializers[i]));
        }
        exprs.Add(expression);
        var sequence = MakeSequence(exprs);  // yields last expression value

        // use invoke/lambda to create variables via parameters in scope
        Expression[] nulls = variables.Select(v => Expression.Constant(null, v.Type)).ToArray();
        expression = Expression.Invoke(Expression.Lambda(sequence, variables.ToArray()), nulls);
      }

      return expression;
    }

    private static Expression MakeSequence(IList<Expression> expressions) {
      var last = expressions[expressions.Count - 1];
      expressions = expressions.Select(e => e.Type.GetTypeInfo().IsValueType ? Expression.Convert(e, typeof(object)) : e).ToList();
      return Expression.Convert(Expression.Call(typeof(ExecutionBuilder), "Sequence", null, Expression.NewArrayInit(typeof(object), expressions)), last.Type);
    }

    public static object Sequence(params object[] values) => values[values.Length - 1];

    public static IEnumerable<R> Batch<T, R>(IEnumerable<T> items, Func<T, R> selector, bool stream) {
      var result = items.Select(selector);
      if (!stream) {
        return result.ToList();
      } else {
        return new EnumerateOnce<R>(result);
      }
    }

    private static Expression MakeAssign(ParameterExpression variable, Expression value) => Expression.Call(typeof(ExecutionBuilder), "Assign", new Type[] { variable.Type }, variable, value);

    public static T Assign<T>(ref T variable, T value) {
      variable = value;
      return value;
    }

    private Expression BuildInner(Expression expression) {
      var eb = new ExecutionBuilder(linguist, policy, executor) {
        scope = scope,
        receivingMember = receivingMember,
        nReaders = nReaders,
        nLookup = nLookup,
        variableMap = variableMap
      };
      return eb.Build(expression);
    }

    protected override MemberBinding VisitBinding(MemberBinding binding) {
      var save = receivingMember;
      receivingMember = binding.Member;
      var result = base.VisitBinding(binding);
      receivingMember = save;
      return result;
    }

    int nLookup = 0;

    private Expression MakeJoinKey(IList<Expression> key) {
      if (key.Count == 1) {
        return key[0];
      } else {
        var constructor = TypeHelper.FindConstructor(typeof(CompoundKey), new[] { typeof(object[]) });

        return Expression.New(
            constructor,
            Expression.NewArrayInit(typeof(object), key.Select(k => (Expression)Expression.Convert(k, typeof(object))))
            );
      }
    }

    protected override Expression VisitClientJoin(ClientJoinExpression join) {
      // convert client join into a up-front lookup table builder & replace client-join in tree with lookup accessor

      // 1) lookup = query.Select(e => new KVP(key: inner, value: e)).ToLookup(kvp => kvp.Key, kvp => kvp.Value)
      var innerKey = MakeJoinKey(join.InnerKey);
      var outerKey = MakeJoinKey(join.OuterKey);

      var kvpConstructor = TypeHelper.FindConstructor(typeof(KeyValuePair<,>).MakeGenericType(innerKey.Type, join.Projection.Projector.Type), new Type[] { innerKey.Type, join.Projection.Projector.Type });
      Expression constructKVPair = Expression.New(kvpConstructor, innerKey, join.Projection.Projector);
      var newProjection = new ProjectionExpression(join.Projection.Select, constructKVPair);

      var iLookup = ++nLookup;
      var execution = ExecuteProjection(newProjection, false);

      var kvp = Expression.Parameter(constructKVPair.Type, "kvp");

      // filter out nulls
      if (join.Projection.Projector.NodeType == (ExpressionType)DbExpressionType.OuterJoined) {
        var pred = Expression.Lambda(
            Expression.PropertyOrField(kvp, "Value").NotEqual(TypeHelper.GetNullConstant(join.Projection.Projector.Type)),
            kvp
            );
        execution = Expression.Call(typeof(Enumerable), "Where", new Type[] { kvp.Type }, execution, pred);
      }

      // make lookup
      var keySelector = Expression.Lambda(Expression.PropertyOrField(kvp, "Key"), kvp);
      var elementSelector = Expression.Lambda(Expression.PropertyOrField(kvp, "Value"), kvp);
      Expression toLookup = Expression.Call(typeof(Enumerable), "ToLookup", new Type[] { kvp.Type, outerKey.Type, join.Projection.Projector.Type }, execution, keySelector, elementSelector);

      // 2) agg(lookup[outer])
      var lookup = Expression.Parameter(toLookup.Type, "lookup" + iLookup);
      var property = lookup.Type.GetTypeInfo().GetDeclaredProperty("Item");
      Expression access = Expression.Call(lookup, property.GetMethod, Visit(outerKey));
      if (join.Projection.Aggregator != null) {
        // apply aggregator
        access = DbExpressionReplacer.Replace(join.Projection.Aggregator.Body, join.Projection.Aggregator.Parameters[0], access);
      }

      variables.Add(lookup);
      initializers.Add(toLookup);

      return access;
    }

    protected override Expression VisitProjection(ProjectionExpression projection) {
      if (isTop) {
        isTop = false;
        return ExecuteProjection(projection, scope != null);
      } else {
        return BuildInner(projection);
      }
    }

    protected virtual Expression Parameterize(Expression expression) {
      if (variableMap.Count > 0) {
        expression = VariableSubstitutor.Substitute(variableMap, expression);
      }
      return linguist.Parameterize(expression);
    }

    private Expression ExecuteProjection(ProjectionExpression projection, bool okayToDefer) {
      // parameterize query
      projection = (ProjectionExpression)Parameterize(projection);

      if (scope != null) {
        // also convert references to outer alias to named values!  these become SQL parameters too
        projection = (ProjectionExpression)OuterParameterizer.Parameterize(scope.Alias, projection);
      }

      var commandText = linguist.Format(projection.Select);
      var namedValues = NamedValueGatherer.Gather(projection.Select);
      var command = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
      Expression[] values = namedValues.Select(v => Expression.Convert(Visit(v.Value), typeof(object))).ToArray();

      return ExecuteProjection(projection, okayToDefer, command, values);
    }

    private Expression ExecuteProjection(ProjectionExpression projection, bool okayToDefer, QueryCommand command, Expression[] values) {
      okayToDefer &= (receivingMember != null && policy.IsDeferLoaded(receivingMember));

      var saveScope = scope;
      var reader = Expression.Parameter(typeof(FieldReader), "r" + nReaders++);
      scope = new Scope(scope, reader, projection.Select.Alias, projection.Select.Columns);
      var projector = Expression.Lambda(Visit(projection.Projector), reader);
      scope = saveScope;

      var entity = EntityFinder.Find(projection.Projector);

      var methExecute = okayToDefer
          ? "ExecuteDeferred"
          : "Execute";

      // call low-level execute directly on supplied DbQueryProvider
      Expression result = Expression.Call(executor, methExecute, new Type[] { projector.Body.Type },
          Expression.Constant(command),
          projector,
          Expression.Constant(entity, typeof(MappingEntity)),
          Expression.NewArrayInit(typeof(object), values)
          );

      if (projection.Aggregator != null) {
        // apply aggregator
        result = DbExpressionReplacer.Replace(projection.Aggregator.Body, projection.Aggregator.Parameters[0], result);
      }
      return result;
    }

    protected override Expression VisitBatch(BatchExpression batch) {
      if (linguist.Language.AllowsMultipleCommands || !IsMultipleCommands(batch.Operation.Body as CommandExpression)) {
        return BuildExecuteBatch(batch);
      } else {
        var source = Visit(batch.Input);
        var op = Visit(batch.Operation.Body);
        var fn = Expression.Lambda(op, batch.Operation.Parameters[1]);
        return Expression.Call(GetType(), "Batch", new Type[] { TypeHelper.GetElementType(source.Type), batch.Operation.Body.Type }, source, fn, batch.Stream);
      }
    }

    protected virtual Expression BuildExecuteBatch(BatchExpression batch) {
      // parameterize query
      var operation = Parameterize(batch.Operation.Body);

      var commandText = linguist.Format(operation);
      var namedValues = NamedValueGatherer.Gather(operation);
      var command = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
      Expression[] values = namedValues.Select(v => Expression.Convert(Visit(v.Value), typeof(object))).ToArray();

      Expression paramSets = Expression.Call(typeof(Enumerable), "Select", new Type[] { batch.Operation.Parameters[1].Type, typeof(object[]) },
          batch.Input,
          Expression.Lambda(Expression.NewArrayInit(typeof(object), values), new[] { batch.Operation.Parameters[1] })
          );

      Expression plan = null;

      var projection = ProjectionFinder.FindProjection(operation);
      if (projection != null) {
        var saveScope = scope;
        var reader = Expression.Parameter(typeof(FieldReader), "r" + nReaders++);
        scope = new Scope(scope, reader, projection.Select.Alias, projection.Select.Columns);
        var projector = Expression.Lambda(Visit(projection.Projector), reader);
        scope = saveScope;

        var entity = EntityFinder.Find(projection.Projector);
        command = new QueryCommand(command.CommandText, command.Parameters);

        plan = Expression.Call(executor, "ExecuteBatch", new Type[] { projector.Body.Type },
            Expression.Constant(command),
            paramSets,
            projector,
            Expression.Constant(entity, typeof(MappingEntity)),
            batch.BatchSize,
            batch.Stream
            );
      } else {
        plan = Expression.Call(executor, "ExecuteBatch", null,
            Expression.Constant(command),
            paramSets,
            batch.BatchSize,
            batch.Stream
            );
      }

      return plan;
    }

    protected override Expression VisitCommand(CommandExpression command) {
      if (linguist.Language.AllowsMultipleCommands || !IsMultipleCommands(command)) {
        return BuildExecuteCommand(command);
      } else {
        return base.VisitCommand(command);
      }
    }

    protected virtual bool IsMultipleCommands(CommandExpression command) {
      if (command == null)
        return false;
      switch ((DbExpressionType)command.NodeType) {
        case DbExpressionType.Insert:
        case DbExpressionType.Delete:
        case DbExpressionType.Update:
          return false;
        default:
          return true;
      }
    }

    protected override Expression VisitInsert(InsertCommand insert) => BuildExecuteCommand(insert);

    protected override Expression VisitUpdate(UpdateCommand update) => BuildExecuteCommand(update);

    protected override Expression VisitDelete(DeleteCommand delete) => BuildExecuteCommand(delete);

    protected override Expression VisitBlock(BlockCommand block) => MakeSequence(VisitExpressionList(block.Commands));

    protected override Expression VisitIf(IFCommand ifx) {
      var test =
          Expression.Condition(
              ifx.Check,
              ifx.IfTrue,
              ifx.IfFalse != null
                  ? ifx.IfFalse
                  : ifx.IfTrue.Type == typeof(int)
                      ? Expression.Property(executor, "RowsAffected")
                      : (Expression)Expression.Constant(TypeHelper.GetDefault(ifx.IfTrue.Type), ifx.IfTrue.Type)
                      );
      return Visit(test);
    }

    protected override Expression VisitFunction(FunctionExpression func) {
      if (linguist.Language.IsRowsAffectedExpressions(func)) {
        return Expression.Property(executor, "RowsAffected");
      }
      return base.VisitFunction(func);
    }

    protected override Expression VisitExists(ExistsExpression exists) {
      // how did we get here? Translate exists into count query
      var colType = linguist.Language.TypeSystem.GetColumnType(typeof(int));
      var newSelect = exists.Select.SetColumns(
          new[] { new ColumnDeclaration("value", new AggregateExpression(typeof(int), "Count", null, false), colType) }
          );

      var projection =
          new ProjectionExpression(
              newSelect,
              new ColumnExpression(typeof(int), colType, newSelect.Alias, "value"),
              Aggregator.GetAggregator(typeof(int), typeof(IEnumerable<int>))
              );

      var expression = projection.GreaterThan(Expression.Constant(0));

      return Visit(expression);
    }

    protected override Expression VisitDeclaration(DeclarationCommand decl) {
      if (decl.Source != null) {
        // make query that returns all these declared values as an object[]
        var projection = new ProjectionExpression(
            decl.Source,
            Expression.NewArrayInit(
                typeof(object),
                decl.Variables.Select(v => v.Expression.Type.GetTypeInfo().IsValueType
                    ? Expression.Convert(v.Expression, typeof(object))
                    : v.Expression).ToArray()
                ),
            Aggregator.GetAggregator(typeof(object[]), typeof(IEnumerable<object[]>))
            );

        // create execution variable to hold the array of declared variables
        var vars = Expression.Parameter(typeof(object[]), "vars");
        variables.Add(vars);
        initializers.Add(Expression.Constant(null, typeof(object[])));

        // create subsitution for each variable (so it will find the variable value in the new vars array)
        for (int i = 0, n = decl.Variables.Count; i < n; i++) {
          var v = decl.Variables[i];
          var nv = new NamedValueExpression(
              v.Name, v.QueryType,
              Expression.Convert(Expression.ArrayIndex(vars, Expression.Constant(i)), v.Expression.Type)
              );
          variableMap.Add(v.Name, nv);
        }

        // make sure the execution of the select stuffs the results into the new vars array
        return MakeAssign(vars, Visit(projection));
      }

      // probably bad if we get here since we must not allow mulitple commands
      throw new InvalidOperationException("Declaration query not allowed for this langauge");
    }

    protected virtual Expression BuildExecuteCommand(CommandExpression command) {
      // parameterize query
      var expression = Parameterize(command);

      var commandText = linguist.Format(expression);
      var namedValues = NamedValueGatherer.Gather(expression);
      var qc = new QueryCommand(commandText, namedValues.Select(v => new QueryParameter(v.Name, v.Type, v.QueryType)));
      Expression[] values = namedValues.Select(v => Expression.Convert(Visit(v.Value), typeof(object))).ToArray();

      var projection = ProjectionFinder.FindProjection(expression);
      if (projection != null) {
        return ExecuteProjection(projection, false, qc, values);
      }

      Expression plan = Expression.Call(executor, "ExecuteCommand", null,
          Expression.Constant(qc),
          Expression.NewArrayInit(typeof(object), values)
          );

      return plan;
    }

    protected override Expression VisitEntity(EntityExpression entity) => Visit(entity.Expression);

    protected override Expression VisitOuterJoined(OuterJoinedExpression outer) {
      var expr = Visit(outer.Expression);
      var column = (ColumnExpression)outer.Test;
      ParameterExpression reader;
      int iOrdinal;
      if (scope.TryGetValue(column, out reader, out iOrdinal)) {
        return Expression.Condition(
            Expression.Call(reader, "IsDbNull", null, Expression.Constant(iOrdinal)),
            Expression.Constant(TypeHelper.GetDefault(outer.Type), outer.Type),
            expr
            );
      }
      return expr;
    }

    protected override Expression VisitColumn(ColumnExpression column) {
      ParameterExpression fieldReader;
      int iOrdinal;
      if (scope != null && scope.TryGetValue(column, out fieldReader, out iOrdinal)) {
        var method = FieldReader.GetReaderMethod(column.Type);
        return Expression.Call(fieldReader, method, Expression.Constant(iOrdinal));
      } else {
        System.Diagnostics.Debug.Fail(string.Format("column not in scope: {0}", column));
      }
      return column;
    }

    class Scope {
      Scope outer;
      ParameterExpression fieldReader;
      internal TableAlias Alias { get; private set; }
      Dictionary<string, int> nameMap;

      internal Scope(Scope outer, ParameterExpression fieldReader, TableAlias alias, IEnumerable<ColumnDeclaration> columns) {
        this.outer = outer;
        this.fieldReader = fieldReader;
        Alias = alias;
        nameMap = columns.Select((c, i) => new { c, i }).ToDictionary(x => x.c.Name, x => x.i);
      }

      internal bool TryGetValue(ColumnExpression column, out ParameterExpression fieldReader, out int ordinal) {
        for (var s = this; s != null; s = s.outer) {
          if (column.Alias == s.Alias && nameMap.TryGetValue(column.Name, out ordinal)) {
            fieldReader = this.fieldReader;
            return true;
          }
        }
        fieldReader = null;
        ordinal = 0;
        return false;
      }
    }

    /// <summary>
    /// columns referencing the outer alias are turned into special named-value parameters
    /// </summary>
    class OuterParameterizer : DbExpressionVisitor {
      int iParam;
      TableAlias outerAlias;
      Dictionary<ColumnExpression, NamedValueExpression> map = new Dictionary<ColumnExpression, NamedValueExpression>();

      internal static Expression Parameterize(TableAlias outerAlias, Expression expr) {
        var op = new OuterParameterizer {
          outerAlias = outerAlias
        };
        return op.Visit(expr);
      }

      protected override Expression VisitProjection(ProjectionExpression proj) {
        var select = (SelectExpression)Visit(proj.Select);
        return UpdateProjection(proj, select, proj.Projector, proj.Aggregator);
      }

      protected override Expression VisitColumn(ColumnExpression column) {
        if (column.Alias == outerAlias) {
          NamedValueExpression nv;
          if (!map.TryGetValue(column, out nv)) {
            nv = new NamedValueExpression("n" + (iParam++), column.QueryType, column);
            map.Add(column, nv);
          }
          return nv;
        }
        return column;
      }
    }

    class ColumnGatherer : DbExpressionVisitor {
      Dictionary<string, ColumnExpression> columns = new Dictionary<string, ColumnExpression>();

      internal static IEnumerable<ColumnExpression> Gather(Expression expression) {
        var gatherer = new ColumnGatherer();
        gatherer.Visit(expression);
        return gatherer.columns.Values;
      }

      protected override Expression VisitColumn(ColumnExpression column) {
        if (!columns.ContainsKey(column.Name)) {
          columns.Add(column.Name, column);
        }
        return column;
      }
    }

    class ProjectionFinder : DbExpressionVisitor {
      ProjectionExpression found = null;

      internal static ProjectionExpression FindProjection(Expression expression) {
        var finder = new ProjectionFinder();
        finder.Visit(expression);
        return finder.found;
      }

      protected override Expression VisitProjection(ProjectionExpression proj) {
        found = proj;
        return proj;
      }
    }

    class VariableSubstitutor : DbExpressionVisitor {
      Dictionary<string, Expression> map;

      private VariableSubstitutor(Dictionary<string, Expression> map) {
        this.map = map;
      }

      public static Expression Substitute(Dictionary<string, Expression> map, Expression expression) => new VariableSubstitutor(map).Visit(expression);

      protected override Expression VisitVariable(VariableExpression vex) {
        Expression sub;
        if (map.TryGetValue(vex.Name, out sub)) {
          return sub;
        }
        return vex;
      }
    }

    class EntityFinder : DbExpressionVisitor {
      MappingEntity entity;

      public static MappingEntity Find(Expression expression) {
        var finder = new EntityFinder();
        finder.Visit(expression);
        return finder.entity;
      }

      public override Expression Visit(Expression exp) {
        if (entity == null)
          return base.Visit(exp);
        return exp;
      }

      protected override Expression VisitEntity(EntityExpression entity) {
        if (this.entity == null)
          this.entity = entity.Entity;
        return entity;
      }

      protected override NewExpression VisitNew(NewExpression nex) => nex;

      protected override Expression VisitMemberInit(MemberInitExpression init) => init;
    }
  }
}
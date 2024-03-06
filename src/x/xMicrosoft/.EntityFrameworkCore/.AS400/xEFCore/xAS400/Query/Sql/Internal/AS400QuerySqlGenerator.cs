using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using xEFCore.xAS400.Query.Expressions.Internal;
using xEFCore.xAS400.Query.Sql.Internal.Visitors;

namespace xEFCore.xAS400.Query.Sql.Internal {

  public class AS400QuerySqlGenerator : DefaultQuerySqlGenerator, IAS400ExpressionVisitor {

    public AS400QuerySqlGenerator(
      [NotNull] QuerySqlGeneratorDependencies dependencies,
      [NotNull] SelectExpression selectExpression,
      bool rowNumberPagingEnabled) : base(dependencies, selectExpression) {

      if (rowNumberPagingEnabled) {
        new RowNumberPagingExpressionVisitor_SqlServer().Visit(selectExpression);
      }
    }

    Dictionary<int, TypeCode> _overrideColumnReturnTypes;
    RelationalTypeMapping _typeMapping;
    RelationalNullsExpandingVisitor _relationalNullsExpandingVisitor;
    PredicateReductionExpressionOptimizer _predicateReductionExpressionOptimizer;
    PredicateNegationExpressionOptimizer _predicateNegationExpressionOptimizer;
    ReducingExpressionVisitor _reducingExpressionVisitor;
    BooleanExpressionTranslatingVisitor_DB2 _booleanExpressionTranslatingVisitor;

    static readonly Dictionary<ExpressionType, string> _operatorMap = new OperatorMap_DB2();


    #region "Private"

    Expression Db2ApplyNullSemantics(Expression expression) {
      RelationalNullsOptimizedExpandingVisitor relationalNullsOptimizedExpandingVisitor = new RelationalNullsOptimizedExpandingVisitor();
      Expression result = (relationalNullsOptimizedExpandingVisitor).Visit(expression);
      if (!relationalNullsOptimizedExpandingVisitor.IsOptimalExpansion) {
        return _relationalNullsExpandingVisitor.Visit(expression);
      }
      return result;
    }

    Expression Db2ApplyOptimizations(Expression expression, bool searchCondition, bool joinCondition = false) {
      var expression2 = (new NullComparisonTransformingVisitor_DB2(ParameterValues)).Visit(expression);
      var binaryExpression = (expression2 is BinaryExpression) ? (expression2 as BinaryExpression) : null;
      if (_relationalNullsExpandingVisitor == null) {
        _relationalNullsExpandingVisitor = new RelationalNullsExpandingVisitor();
      }
      if (_predicateReductionExpressionOptimizer == null) {
        _predicateReductionExpressionOptimizer = new PredicateReductionExpressionOptimizer();
      }
      if (_predicateNegationExpressionOptimizer == null) {
        _predicateNegationExpressionOptimizer = new PredicateNegationExpressionOptimizer();
      }
      if (_reducingExpressionVisitor == null) {
        _reducingExpressionVisitor = new ReducingExpressionVisitor();
      }
      if (_booleanExpressionTranslatingVisitor == null) {
        _booleanExpressionTranslatingVisitor = new BooleanExpressionTranslatingVisitor_DB2();
      }
      expression2 = ((!joinCondition || binaryExpression == null || binaryExpression.NodeType != ExpressionType.Equal) ? Db2ApplyNullSemantics(expression2) : Expression.MakeBinary(binaryExpression.NodeType, Db2ApplyNullSemantics(binaryExpression.Left), Db2ApplyNullSemantics(binaryExpression.Right)));
      expression2 = _predicateReductionExpressionOptimizer.Visit(expression2);
      expression2 = _predicateNegationExpressionOptimizer.Visit(expression2);
      expression2 = _reducingExpressionVisitor.Visit(expression2);
      if (binaryExpression == null || (expression.NodeType != ExpressionType.Or && expression.NodeType != ExpressionType.And)) {
        expression2 = _booleanExpressionTranslatingVisitor.Translate(expression2, searchCondition);
      }
      return expression2;
    }

    void GenerateFunctionCallWithCast([NotNull] string funName, [CanBeNull] IReadOnlyList<Expression> arguments, [NotNull] string typeName) {
      Sql.Append(" CAST(" + funName + "(");
      if (arguments != null) {
        VisitJoin(arguments.ToList(), null);
      } else {
        Sql.Append("*");
      }
      Sql.Append(") AS " + typeName + ")");
    }

    void GenerateReplaceSQL(SqlFunctionExpression fun) {
      string o = "";
      Sql.Append(fun.FunctionName + "(");
      foreach (Expression argument in fun.Arguments) {
        Sql.Append(o);
        if (argument.NodeType == ExpressionType.Parameter && ParameterValues.ContainsKey(argument.ToString())) {
          Sql.Append("'" + ParameterValues[argument.ToString()] + "'");
        } else if (argument.NodeType == ExpressionType.Constant) {
          Sql.Append(argument.ToString().StartsWith("\"", StringComparison.Ordinal) ? argument.ToString().Replace("\"", "'") : ("'" + argument + "'"));
        } else {
          (this).Visit(argument);
        }
        o = ", ";
      }
      Sql.Append(")");
    }

    static bool? GetBooleanConstantValue(Expression expression) {
      ConstantExpression constantExpression;
      if ((constantExpression = (expression as ConstantExpression)) != null && constantExpression.Type.UnwrapNullableType() == typeof(bool)) {
        return (bool?)constantExpression.Value;
      }
      return null;
    }

    void HandleStringContainExpression(LikeExpression likeExpression) {
      Expression expression = VisitLikeExpression((BinaryExpression)likeExpression.Pattern);
      if (expression.NodeType == ExpressionType.Constant || expression.NodeType == ExpressionType.Parameter) {
        Visit(likeExpression.Match);
        Sql.Append(" LIKE '%' || ");
        Visit(expression);
        Sql.Append(" || '%'");
      } else {
        Sql.Append(" LOCATE (");
        Visit(expression);
        Sql.Append(" , ");
        Visit(likeExpression.Match);
        Sql.Append(" ) >= cast(1 AS int)");
      }
    }

    void HandleStringStartsWithExpression(LikeExpression likeExpression) {
      Expression left = ((BinaryExpression)likeExpression.Pattern).Left;
      if (left.NodeType == ExpressionType.Constant || left.NodeType == ExpressionType.Parameter) {
        Visit(likeExpression.Match);
        Sql.Append(" LIKE ");
        Visit(likeExpression.Pattern);
      } else {
        Sql.Append(" LOCATE (");
        Visit(left);
        Sql.Append(" , ");
        Visit(likeExpression.Match);
        Sql.Append(" ) = cast(1 AS int)");
      }
    }

    void HandleStringEndsWithExpression(LikeExpression likeExpression) {
      Expression right = ((BinaryExpression)likeExpression.Pattern).Right;
      string text = " LOCATE (";
      if (right.NodeType == ExpressionType.Constant || right.NodeType == ExpressionType.Parameter) {
        Visit(likeExpression.Match);
        Sql.Append(" LIKE ");
        Visit(likeExpression.Pattern);
      } else {
        Sql.Append(text);
        Visit(right);
        Sql.Append(" , RIGHT( ");
        Visit(likeExpression.Match);
        Sql.Append(" , ( LENGTH(");
        Visit(likeExpression.Match);
        Sql.Append(") + 1) - " + text);
        Visit(right);
        Sql.Append(" , ");
        Visit(likeExpression.Match);
        Sql.Append(" )))  = cast(1 AS int)");
      }
    }

    void ProcessExpressionList(IReadOnlyList<Expression> expressions, Action<IRelationalCommandBuilder> joinAction = null) {
      ProcessExpressionList(expressions, delegate (Expression e) {
        Visit(e);
      }, joinAction);
    }

    void ProcessExpressionList<T>(IReadOnlyList<T> items, Action<T> itemAction, Action<IRelationalCommandBuilder> joinAction = null) {
      joinAction = (joinAction ?? delegate (IRelationalCommandBuilder isb) {
        isb.Append(", ");
      });
      for (int i = 0; i < items.Count; i++) {
        if (i > 0) {
          joinAction(Sql);
        }
        itemAction(items[i]);
      }
    }

    void VisitJoin(IReadOnlyList<Expression> expressions, Action<IRelationalCommandBuilder> joinAction = null) {
      VisitJoin(expressions, delegate (Expression e) { Visit(e); }, joinAction);
    }

    void VisitJoin<T>(IReadOnlyList<T> items, Action<T> itemAction, Action<IRelationalCommandBuilder> joinAction = null) {
      joinAction = (joinAction ?? delegate (IRelationalCommandBuilder isb) {
        isb.Append(", ");
      });
      for (int i = 0; i < items.Count; i++) {
        if (i > 0) {
          joinAction(Sql);
        }
        itemAction(items[i]);
      }
    }

    Expression VisitLikeExpression(BinaryExpression pattern) {
      if (pattern.Left.NodeType == ExpressionType.Add) {
        return VisitLikeExpression((BinaryExpression)pattern.Left);
      }
      if (pattern.Left.NodeType == ExpressionType.Extension) {
        return pattern.Left;
      }
      if (pattern.Left.NodeType == ExpressionType.Constant) {
        if (!pattern.Left.ToString().Equals("\"%\"")) {
          return pattern.Left;
        }
        return pattern.Right;
      }
      return pattern;
    }

    #endregion

    #region "Overrides"

    public override IRelationalValueBufferFactory CreateValueBufferFactory(IRelationalValueBufferFactoryFactory relationalValueBufferFactoryFactory, DbDataReader dataReader) {
      Check.NotNull(relationalValueBufferFactoryFactory, "relationalValueBufferFactoryFactory");
      Type[] array = SelectExpression.GetProjectionTypes().ToArray();
      if (array.Contains(Type.GetType("System.Boolean")) || array.Contains(Type.GetType("System.Nullable`1[System.Boolean]"))) {
        int num = 0;
        Type[] array2 = array;
        for (int i = 0; i < array2.Length; i++) {
          if (array2[i].FullName.Contains("System.Boolean")) {
            if (_overrideColumnReturnTypes == null) {
              _overrideColumnReturnTypes = new Dictionary<int, TypeCode>();
            }
            _overrideColumnReturnTypes.Add(num, TypeCode.Boolean);
          }
          num++;
        }
        //TODO:IsPrivate  ((DB2DataReader)dataReader).m_overrideColumnReturnTypes = _overrideColumnReturnTypes;
      }
      return relationalValueBufferFactoryFactory.Create(array, null);
    }

    protected override string GenerateBinaryOperator(ExpressionType op) => _operatorMap[op];

    protected override void GenerateLimitOffset(SelectExpression selectExpression) {
      Check.NotNull(selectExpression, "selectExpression");
      if (!selectExpression.Projection.OfType<RowNumberExpression_DB2>().Any() && selectExpression.Limit != null) {
        if (selectExpression.Limit.NodeType == ExpressionType.Parameter && ParameterValues.ContainsKey(selectExpression.Limit.ToString())) {
          Sql.Append(" FETCH FIRST ").Append(ParameterValues[selectExpression.Limit.ToString()].ToString()).Append(" ROWS ONLY");
        } else if (selectExpression.Limit.NodeType == ExpressionType.Constant) {
          Sql.Append(" FETCH FIRST ").Append(selectExpression.Limit).Append(" ROWS ONLY");
        }
      }
    }

    protected override string GenerateOperator([NotNull] Expression expression) {
      switch (expression.NodeType) {
        case ExpressionType.Extension: {
            StringCompareExpression stringCompareExpression;
            if ((stringCompareExpression = (expression as StringCompareExpression)) == null) {
              break;
            }
            return GenerateBinaryOperator(stringCompareExpression.Operator);
          }
        case ExpressionType.Add:
          if (!(expression.Type == typeof(string))) {
            return " + ";
          }
          return " || ";
      }
      string result;
      if (expression is BinaryExpression) {
        if (!TryGenerateBinaryOperator(expression.NodeType, out result)) {
          throw new ArgumentOutOfRangeException();
        }
        return result;
      }
      if (!_operatorMap.TryGetValue(expression.NodeType, out result)) {
        throw new ArgumentOutOfRangeException();
      }
      return result;
    }

    protected override void GeneratePredicate([NotNull] Expression predicate) {
      Expression expression = Db2ApplyOptimizations(predicate, true, false);
      BinaryExpression binaryExpression;
      if ((binaryExpression = (expression as BinaryExpression)) != null) {
        bool? booleanConstantValue = GetBooleanConstantValue(binaryExpression.Left);
        bool? booleanConstantValue2 = GetBooleanConstantValue(binaryExpression.Right);
        if (binaryExpression.NodeType == ExpressionType.Equal && booleanConstantValue == true && booleanConstantValue2 == true) {
          return;
        }
        if (binaryExpression.NodeType == ExpressionType.NotEqual && booleanConstantValue == false && booleanConstantValue2 == false) {
          return;
        }
      }
      Sql.AppendLine().Append("WHERE ");
      (this).Visit(expression);
    }

    protected override void GenerateTop(SelectExpression selectExpression) {
      Check.NotNull(selectExpression, nameof(selectExpression));
      if (selectExpression.Offset != null) {
        if (selectExpression.Offset.NodeType == ExpressionType.Parameter && ParameterValues.ContainsKey(selectExpression.Offset.ToString())) {
          Sql.Append(" SKIP ").Append(ParameterValues[selectExpression.Offset.ToString()].ToString()).Append(" ");
        } else if (selectExpression.Offset.NodeType == ExpressionType.Constant) {
          Sql.Append(" SKIP ").Append(selectExpression.Offset).Append(" ");
        }
      }
      if (selectExpression.Limit != null) {
        if (selectExpression.Limit.NodeType == ExpressionType.Parameter && ParameterValues.ContainsKey(selectExpression.Limit.ToString())) {
          Sql.Append("FIRST ").Append(ParameterValues[selectExpression.Limit.ToString()].ToString()).Append(" ");
        } else if (selectExpression.Limit.NodeType == ExpressionType.Constant) {
          Sql.Append("FIRST ").Append(selectExpression.Limit).Append(" ");
        }
      }
    }

    protected override bool TryGenerateBinaryOperator(ExpressionType op, [NotNull] out string result) => _operatorMap.TryGetValue(op, out result);
    protected override string TypedTrueLiteral => "CAST(1 AS SMALLINT)";
    protected override string TypedFalseLiteral => "CAST(0 AS SMALLINT)";

    protected override Expression VisitBinary(BinaryExpression binaryExpression) {
      Check.NotNull(binaryExpression, "expression");
      ExpressionType nodeType = binaryExpression.NodeType;
      if (nodeType == ExpressionType.Coalesce) {
        Sql.Append("COALESCE(");
        Visit(binaryExpression.Left);
        Sql.Append(", ");
        Visit(binaryExpression.Right);
        Sql.Append(")");
      } else {
        RelationalTypeMapping typeMapping = _typeMapping;
        if (binaryExpression.IsComparisonOperation() || binaryExpression.NodeType == ExpressionType.Add) {
          _typeMapping = (InferTypeMappingFromColumn(binaryExpression.Left) ?? InferTypeMappingFromColumn(binaryExpression.Right) ?? typeMapping);
        }
        bool num = binaryExpression.Left.RemoveConvert() is BinaryExpression;
        if (num) {
          Sql.Append("(");
        }
        Visit(binaryExpression.Left);
        if (num) {
          Sql.Append(")");
        }
        Sql.Append(GenerateOperator(binaryExpression));
        bool num2 = binaryExpression.Right.RemoveConvert() is BinaryExpression;
        if (num2) {
          Sql.Append("(");
        }
        Visit(binaryExpression.Right);
        if (num2) {
          Sql.Append(")");
        }
        _typeMapping = typeMapping;
      }
      return binaryExpression;
    }

    protected override Expression VisitConditional(ConditionalExpression conditionalExpression) {
      Check.NotNull(conditionalExpression, nameof(conditionalExpression));
      Sql.AppendLine("CASE");
      using (Sql.Indent()) {
        Sql.Append("WHEN ");
        Visit(conditionalExpression.Test);
        Sql.AppendLine();
        Sql.Append("THEN ");
        if (conditionalExpression.IfTrue is ConstantExpression constantExpression && constantExpression.Type == typeof(bool)) {
          Sql.Append(((bool)constantExpression.Value) ? TypedTrueLiteral : TypedFalseLiteral);
        } else {
          Visit(conditionalExpression.IfTrue);
        }
        Sql.Append(" ELSE ");
        if (conditionalExpression.IfFalse is ConstantExpression constantExpression2 && constantExpression2.Type == typeof(bool)) {
          Sql.Append(((bool)constantExpression2.Value) ? TypedTrueLiteral : TypedFalseLiteral);
        } else {
          Visit(conditionalExpression.IfFalse);
        }
        Sql.AppendLine();
      }
      Sql.Append("END");
      return conditionalExpression;
    }

    public override Expression VisitCrossJoinLateral(CrossJoinLateralExpression crossJoinLateralExpression) {
      Check.NotNull(crossJoinLateralExpression, nameof(crossJoinLateralExpression));
      Sql.Append("INNER JOIN LATERAL ");
      Visit(crossJoinLateralExpression.TableExpression);
      Sql.Append(" ON 1 = 1 ");
      return crossJoinLateralExpression;
    }

    public override Expression VisitLike(LikeExpression likeExpression) {
      Check.NotNull(likeExpression, nameof(likeExpression));
      if (!(likeExpression.Pattern is BinaryExpression)) {
        return base.VisitLike(likeExpression);
      }
      Expression left = ((BinaryExpression)likeExpression.Pattern).Left;
      RelationalTypeMapping typeMapping = _typeMapping;
      _typeMapping = (InferTypeMappingFromColumn(likeExpression.Match) ?? typeMapping);
      if (left.NodeType == ExpressionType.Add) {
        HandleStringContainExpression(likeExpression);
      } else if (left.NodeType == ExpressionType.Constant && left.ToString().Equals("\"%\"")) {
        HandleStringEndsWithExpression(likeExpression);
      } else {
        HandleStringStartsWithExpression(likeExpression);
      }
      _typeMapping = typeMapping;
      return likeExpression;
    }

    public override Expression VisitSelect(SelectExpression selectExpression) {
      Check.NotNull(selectExpression, "selectExpression");
      bool flag = true;
      IDisposable disposable = null;
      if (selectExpression.Alias != null) {
        Sql.AppendLine("(");
        disposable = Sql.Indent();
      }
      Sql.Append("SELECT ");
      //if (Db2GetConnInfo.m_bIDS) {
      //  GenerateTop(selectExpression);
      //}
      if (selectExpression.IsDistinct) {
        Sql.Append("DISTINCT ");
      }
      bool flag2 = false;
      if (selectExpression.IsProjectStar) {
        Sql.Append(SqlGenerator.DelimitIdentifier(selectExpression.Tables.Last().Alias)).Append(".*");
        flag2 = true;
      }
      if (selectExpression.Projection.Any()) {
        if (selectExpression.IsProjectStar) {
          Sql.Append(", ");
        }
        if (selectExpression.Projection.Count == 1 && selectExpression.Projection[0] is AliasExpression) {
          if ((selectExpression.Projection[0] as AliasExpression).Expression is ConstantExpression constantExpression && constantExpression.Value == null) {
            flag = false;
            //if (Db2GetConnInfo.m_bIDS) {
            //  Sql.Append(" 1 FROM INFORMIX.SYSTABLES WHERE TABID = 1 ");
            //} else {
            Sql.Append("CAST (1 AS int) AS X FROM SYSIBM.SYSDUMMY1 ");
            //}
          } else {
            ProcessExpressionList(selectExpression.Projection, GenerateProjection, null);
          }
        } else {
          ProcessExpressionList(selectExpression.Projection, GenerateProjection, null);
        }
        flag2 = true;
      }
      if (!flag2) {
        Sql.Append("1");
      }
      if (selectExpression.Tables.Any()) {
        Sql.AppendLine().Append("FROM ");
        ProcessExpressionList(selectExpression.Tables, delegate (IRelationalCommandBuilder sql) {
          sql.AppendLine();
        });
      } else if (flag) {
        //if (Db2GetConnInfo.m_bIDS) {
        //  Sql.Append(" AS C1 FROM ( SELECT 1 FROM INFORMIX.SYSTABLES WHERE TABID = 1 ) ");
        //} else {
        Sql.Append(" AS C1 FROM  ( SELECT CAST(1 AS int) AS X FROM SYSIBM.SYSDUMMY1 ) ");
        //}
        Sql.Append("AS SingleRowTable1 ");
      }
      if (selectExpression.Predicate != null) {
        GeneratePredicate(selectExpression.Predicate);
      }
      if (selectExpression.OrderBy.Any()) {
        Sql.AppendLine();
        GenerateOrderBy(selectExpression.OrderBy);
      }
      //if (!Db2GetConnInfo.m_bIDS) {
      //  GenerateLimitOffset(selectExpression);
      //}
      if (disposable != null) {
        disposable.Dispose();
        Sql.AppendLine().Append(")");
        if (selectExpression.Alias.Length > 0) {
          Sql.Append(" AS ").Append(SqlGenerator.DelimitIdentifier(selectExpression.Alias));
        }
      }
      return selectExpression;
    }

    public override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression) {
      if (sqlFunctionExpression.FunctionName.StartsWith("@@", StringComparison.Ordinal)) {
        Sql.Append(sqlFunctionExpression.FunctionName);
        return sqlFunctionExpression;
      }
      if (sqlFunctionExpression.FunctionName.Equals("COUNT", StringComparison.OrdinalIgnoreCase) && sqlFunctionExpression.Type == typeof(long)) {
        GenerateFunctionCallWithCast("COUNT_BIG", null, "bigint");
      }
      return sqlFunctionExpression;
    }

    public override Expression VisitStringCompare(StringCompareExpression stringCompareExpression) {
      SqlFunctionExpression sqlFunctionExpression = stringCompareExpression.Left as SqlFunctionExpression;
      if (sqlFunctionExpression != null && sqlFunctionExpression.FunctionName.Equals("REPLACE", StringComparison.OrdinalIgnoreCase)) {
        GenerateReplaceSQL(sqlFunctionExpression);
      }
      Sql.Append(GenerateBinaryOperator(stringCompareExpression.Operator));
      sqlFunctionExpression = (stringCompareExpression.Right as SqlFunctionExpression);
      if (sqlFunctionExpression != null && sqlFunctionExpression.FunctionName.Equals("REPLACE", StringComparison.OrdinalIgnoreCase)) {
        GenerateReplaceSQL(sqlFunctionExpression);
      }
      return stringCompareExpression;
    }

    #endregion

    #region "public"

    public virtual Expression VisitDatePartExpression_DB2(DatePartExpression_DB2 expression) {
      Check.NotNull(expression, nameof(expression));
      Sql.Append(expression.DatePart).Append("( ");
      VisitJoin(expression.Argument.ToList(), null);
      Sql.Append(")");
      return expression;
    }

    public virtual Expression VisitDayOfYearExpression_DB2(DayOfYearExpression_DB2 expression) {
      Check.NotNull(expression, nameof(expression));
      Sql.Append("(");
      Visit(expression.DateExpression);
      Sql.Append(" - ");
      VisitDatePartExpression_DB2(expression.MDYExpression);
      Sql.Append(")");
      return expression;
    }

    public virtual Expression VisitRowNumber_DB2(RowNumberExpression_DB2 expression) {
      Check.NotNull(expression, nameof(expression));
      Sql.Append("ROW_NUMBER() OVER(");
      if (expression.Orderings.Any()) {
        GenerateOrderBy(expression.Orderings);
      }
      Sql.Append(")");
      return expression;
    }

    public virtual Expression VisitRowNumber_SqlServer(RowNumberExpression_SqlServer expression) {
      Check.NotNull(expression, nameof(expression));
      Sql.Append("ROW_NUMBER() OVER(");
      GenerateOrderBy(expression.Orderings);
      Sql.Append(")");
      return expression;
    }

    public Expression VisitTrimFunction_DB2(TrimFunctionExpression_DB2 expression) {
      Check.NotNull(expression, nameof(expression));
      Sql.Append(expression.FunctionName);
      Sql.Append("(");
      VisitJoinForTrim(expression.Arguments.ToList(), null);
      Sql.Append(")");
      return expression;
    }

    void VisitJoinForTrim(IReadOnlyList<Expression> expressions, Action<IRelationalCommandBuilder> joinAction = null) {
      VisitJoinForTrim(expressions, delegate (Expression e) { Visit(e); }, joinAction);
    }

    void VisitJoinForTrim<T>(IReadOnlyList<T> items, Action<T> itemAction, Action<IRelationalCommandBuilder> joinAction = null) {
      joinAction = (joinAction ?? delegate (IRelationalCommandBuilder isb) {
        isb.Append(" FROM ");
      });
      for (int num = items.Count - 1; num >= 0; num--) {
        if (num == 0 && items.Count != num + 1) {
          joinAction(Sql);
        }
        itemAction(items[num]);
      }
    }

    #endregion

    protected override void GenerateProjection(Expression projection) {
      var aliasedProjection = projection as AliasExpression;
      var expressionToProcess = aliasedProjection?.Expression ?? projection;
      var updatedExperssion = ExplicitCastToBool(expressionToProcess);
      expressionToProcess = aliasedProjection != null
          ? new AliasExpression(aliasedProjection.Alias, updatedExperssion)
          : updatedExperssion;
      base.GenerateProjection(expressionToProcess);
    }

    Expression ExplicitCastToBool(Expression expression) {
      return (expression as BinaryExpression)?.NodeType == ExpressionType.Coalesce
             && expression.Type.UnwrapNullableType() == typeof(bool)
          ? new ExplicitCastExpression(expression, expression.Type)
          : expression;
    }

  }
}
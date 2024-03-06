//using System.Diagnostics;
//using System.Linq.Expressions;

//namespace Common.ToSql;

///// <summary>
///// https://stackoverflow.com/questions/7731905/how-to-convert-an-expression-tree-to-a-partial-sql-query
///// </summary>
//public class SimpleExpressionToSQL : ExpressionVisitor {
//  /*
//   * Original By Ryan Wright: https://stackoverflow.com/questions/7731905/how-to-convert-an-expression-tree-to-a-partial-sql-query
//   */

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<string> _groupBy = new List<string>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<string> _orderBy = new List<string>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<SqlParameter> _parameters = new List<SqlParameter>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<string> _select = new List<string>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<string> _update = new List<string>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private readonly List<string> _where = new List<string>();

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private int? _skip;

//  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//  private int? _take;

//  public SimpleExpressionToSQL(IQueryable queryable) {
//    if (queryable is null) {
//      throw new ArgumentNullException(nameof(queryable));
//    }

//    var expression = queryable.Expression;
//    Visit(expression);
//    var entityType = (GetEntityType(expression) as IQueryable).ElementType;
//    TableName = queryable.GetTableName(entityType);
//    DbContext = queryable.GetDbContext();
//  }

//  public string CommandText => BuildSqlStatement().Join(Environment.NewLine);

//  public DbContext DbContext { get; private set; }

//  public string From => $"FROM [{TableName}]";

//  public string GroupBy => _groupBy.Count == 0 ? null : "GROUP BY " + _groupBy.Join(", ");
//  public bool IsDelete { get; private set; } = false;
//  public bool IsDistinct { get; private set; }
//  public string OrderBy => BuildOrderByStatement().Join(" ");
//  public SqlParameter[] Parameters => _parameters.ToArray();
//  public string Select => BuildSelectStatement().Join(" ");
//  public int? Skip => _skip;
//  public string TableName { get; private set; }
//  public int? Take => _take;
//  public string Update => "SET " + _update.Join(", ");

//  public string Where => _where.Count == 0 ? null : "WHERE " + _where.Join(" ");

//  public static implicit operator string(SimpleExpressionToSQL simpleExpression) => simpleExpression.ToString();

//  public int ExecuteNonQuery(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead) {
//    DbConnection connection = DbContext.Database.GetDbConnection();
//    using (DbCommand command = connection.CreateCommand()) {
//      command.CommandText = CommandText;
//      command.CommandType = CommandType.Text;
//      command.Parameters.AddRange(Parameters);

//#if DEBUG
//      Debug.WriteLine(ToString());
//#endif

//      if (command.Connection.State != ConnectionState.Open)
//        command.Connection.Open();

//      using (DbTransaction transaction = connection.BeginTransaction(isolationLevel)) {
//        command.Transaction = transaction;
//        int result = command.ExecuteNonQuery();
//        transaction.Commit();

//        return result;
//      }
//    }
//  }

//  public async Task<int> ExecuteNonQueryAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead) {
//    DbConnection connection = DbContext.Database.GetDbConnection();
//    using (DbCommand command = connection.CreateCommand()) {
//      command.CommandText = CommandText;
//      command.CommandType = CommandType.Text;
//      command.Parameters.AddRange(Parameters);

//#if DEBUG
//      Debug.WriteLine(ToString());
//#endif

//      if (command.Connection.State != ConnectionState.Open)
//        await command.Connection.OpenAsync();

//      using (DbTransaction transaction = connection.BeginTransaction(isolationLevel)) {
//        command.Transaction = transaction;
//        int result = await command.ExecuteNonQueryAsync();
//        transaction.Commit();

//        return result;
//      }
//    }
//  }

//  public override string ToString() =>
//      BuildDeclaration()
//          .Union(BuildSqlStatement())
//          .Join(Environment.NewLine);

//  protected override Expression VisitBinary(BinaryExpression binaryExpression) {
//    _where.Add("(");
//    Visit(binaryExpression.Left);

//    switch (binaryExpression.NodeType) {
//      case ExpressionType.And:
//        _where.Add("AND");
//        break;

//      case ExpressionType.AndAlso:
//        _where.Add("AND");
//        break;

//      case ExpressionType.Or:
//      case ExpressionType.OrElse:
//        _where.Add("OR");
//        break;

//      case ExpressionType.Equal:
//        if (IsNullConstant(binaryExpression.Right)) {
//          _where.Add("IS");
//        } else {
//          _where.Add("=");
//        }
//        break;

//      case ExpressionType.NotEqual:
//        if (IsNullConstant(binaryExpression.Right)) {
//          _where.Add("IS NOT");
//        } else {
//          _where.Add("<>");
//        }
//        break;

//      case ExpressionType.LessThan:
//        _where.Add("<");
//        break;

//      case ExpressionType.LessThanOrEqual:
//        _where.Add("<=");
//        break;

//      case ExpressionType.GreaterThan:
//        _where.Add(">");
//        break;

//      case ExpressionType.GreaterThanOrEqual:
//        _where.Add(">=");
//        break;

//      default:
//        throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", binaryExpression.NodeType));
//    }

//    Visit(binaryExpression.Right);
//    _where.Add(")");
//    return binaryExpression;
//  }

//  protected override Expression VisitConstant(ConstantExpression constantExpression) {
//    switch (constantExpression.Value) {
//      case null when constantExpression.Value == null:
//        _where.Add("NULL");
//        break;

//      default:

//        if (constantExpression.Type.CanConvertToSqlDbType()) {
//          _where.Add(CreateParameter(constantExpression.Value).ParameterName);
//        }

//        break;
//    }

//    return constantExpression;
//  }

//  protected override Expression VisitMember(MemberExpression memberExpression) {
//    Expression VisitMemberLocal(Expression expression) {
//      switch (expression.NodeType) {
//        case ExpressionType.Parameter:
//          _where.Add($"[{memberExpression.Member.Name}]");
//          return memberExpression;

//        case ExpressionType.Constant:
//          _where.Add(CreateParameter(GetValue(memberExpression)).ParameterName);

//          return memberExpression;

//        case ExpressionType.MemberAccess:
//          _where.Add(CreateParameter(GetValue(memberExpression)).ParameterName);

//          return memberExpression;
//      }

//      throw new NotSupportedException(string.Format("The member '{0}' is not supported", memberExpression.Member.Name));
//    }

//    if (memberExpression.Expression == null) {
//      return VisitMemberLocal(memberExpression);
//    }

//    return VisitMemberLocal(memberExpression.Expression);
//  }

//  protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression) {
//    switch (methodCallExpression.Method.Name) {
//      case nameof(Queryable.Where) when methodCallExpression.Method.DeclaringType == typeof(Queryable):

//        Visit(methodCallExpression.Arguments[0]);
//        var lambda = (LambdaExpression)StripQuotes(methodCallExpression.Arguments[1]);
//        Visit(lambda.Body);

//        return methodCallExpression;

//      case nameof(Queryable.Select):
//        return ParseExpression(methodCallExpression, _select);

//      case nameof(Queryable.GroupBy):
//        return ParseExpression(methodCallExpression, _groupBy);

//      case nameof(Queryable.Take):
//        return ParseExpression(methodCallExpression, ref _take);

//      case nameof(Queryable.Skip):
//        return ParseExpression(methodCallExpression, ref _skip);

//      case nameof(Queryable.OrderBy):
//      case nameof(Queryable.ThenBy):
//        return ParseExpression(methodCallExpression, _orderBy, "ASC");

//      case nameof(Queryable.OrderByDescending):
//      case nameof(Queryable.ThenByDescending):
//        return ParseExpression(methodCallExpression, _orderBy, "DESC");

//      case nameof(Queryable.Distinct):
//        IsDistinct = true;
//        return Visit(methodCallExpression.Arguments[0]);

//      case nameof(string.StartsWith):
//        _where.AddRange(ParseExpression(methodCallExpression, methodCallExpression.Object));
//        _where.Add("LIKE");
//        _where.Add(CreateParameter(GetValue(methodCallExpression.Arguments[0]).ToString() + "%").ParameterName);
//        return methodCallExpression.Arguments[0];

//      case nameof(string.EndsWith):
//        _where.AddRange(ParseExpression(methodCallExpression, methodCallExpression.Object));
//        _where.Add("LIKE");
//        _where.Add(CreateParameter("%" + GetValue(methodCallExpression.Arguments[0]).ToString()).ParameterName);
//        return methodCallExpression.Arguments[0];

//      case nameof(string.Contains):
//        _where.AddRange(ParseExpression(methodCallExpression, methodCallExpression.Object));
//        _where.Add("LIKE");
//        _where.Add(CreateParameter("%" + GetValue(methodCallExpression.Arguments[0]).ToString() + "%").ParameterName);
//        return methodCallExpression.Arguments[0];

//      case nameof(Extensions.ToSqlString):
//        return Visit(methodCallExpression.Arguments[0]);

//      case nameof(Extensions.Delete):
//      case nameof(Extensions.DeleteAsync):
//        IsDelete = true;
//        return Visit(methodCallExpression.Arguments[0]);

//      case nameof(Extensions.Update):
//        return ParseExpression(methodCallExpression, _update);

//      default:
//        if (methodCallExpression.Object != null) {
//          _where.Add(CreateParameter(GetValue(methodCallExpression)).ParameterName);
//          return methodCallExpression;
//        }
//        break;
//    }

//    throw new NotSupportedException($"The method '{methodCallExpression.Method.Name}' is not supported");
//  }

//  protected override Expression VisitUnary(UnaryExpression unaryExpression) {
//    switch (unaryExpression.NodeType) {
//      case ExpressionType.Not:
//        _where.Add("NOT");
//        Visit(unaryExpression.Operand);
//        break;

//      case ExpressionType.Convert:
//        Visit(unaryExpression.Operand);
//        break;

//      default:
//        throw new NotSupportedException($"The unary operator '{unaryExpression.NodeType}' is not supported");
//    }
//    return unaryExpression;
//  }

//  private static Expression StripQuotes(Expression expression) {
//    while (expression.NodeType == ExpressionType.Quote) {
//      expression = ((UnaryExpression)expression).Operand;
//    }
//    return expression;
//  }

//  [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Easier to read")]
//  private IEnumerable<string> BuildDeclaration() {
//    if (Parameters.Length == 0)                        /**/    yield break;
//    foreach (SqlParameter parameter in Parameters)     /**/    yield return $"DECLARE {parameter.ParameterName} {parameter.SqlDbType}";

//    foreach (SqlParameter parameter in Parameters)     /**/
//      if (parameter.SqlDbType.RequiresQuotes())      /**/    yield return $"SET {parameter.ParameterName} = '{parameter.SqlValue?.ToString().Replace("'", "''") ?? "NULL"}'";
//      else                                           /**/    yield return $"SET {parameter.ParameterName} = {parameter.SqlValue}";
//  }

//  [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Easier to read")]
//  private IEnumerable<string> BuildOrderByStatement() {
//    if (Skip.HasValue && _orderBy.Count == 0)                       /**/   yield return "ORDER BY (SELECT NULL)";
//    else if (_orderBy.Count == 0)                                   /**/   yield break;
//    else if (_groupBy.Count > 0 && _orderBy[0].StartsWith("[Key]")) /**/   yield return "ORDER BY " + _groupBy.Join(", ");
//    else                                                            /**/   yield return "ORDER BY " + _orderBy.Join(", ");

//    if (Skip.HasValue && Take.HasValue)                             /**/   yield return $"OFFSET {Skip} ROWS FETCH NEXT {Take} ROWS ONLY";
//    else if (Skip.HasValue && !Take.HasValue)                       /**/   yield return $"OFFSET {Skip} ROWS";
//  }

//  [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Easier to read")]
//  private IEnumerable<string> BuildSelectStatement() {
//    yield return "SELECT";

//    if (IsDistinct)                                 /**/    yield return "DISTINCT";

//    if (Take.HasValue && !Skip.HasValue)            /**/    yield return $"TOP ({Take.Value})";

//    if (_select.Count == 0 && _groupBy.Count > 0)   /**/    yield return _groupBy.Select(x => $"MAX({x})").Join(", ");
//    else if (_select.Count == 0)                    /**/    yield return "*";
//    else                                            /**/    yield return _select.Join(", ");
//  }

//  [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Easier to read")]
//  private IEnumerable<string> BuildSqlStatement() {
//    if (IsDelete)                   /**/   yield return "DELETE";
//    else if (_update.Count > 0)     /**/   yield return $"UPDATE [{TableName}]";
//    else                            /**/   yield return Select;

//    if (_update.Count == 0)         /**/   yield return From;
//    else if (_update.Count > 0)     /**/   yield return Update;

//    if (Where != null)              /**/   yield return Where;
//    if (GroupBy != null)            /**/   yield return GroupBy;
//    if (OrderBy != null)            /**/   yield return OrderBy;
//  }

//  private SqlParameter CreateParameter(object value) {
//    var parameterName = $"@p{_parameters.Count}";

//    var parameter = new SqlParameter() {
//      ParameterName = parameterName,
//      Value = value
//    };

//    _parameters.Add(parameter);

//    return parameter;
//  }

//  private object GetEntityType(Expression expression) {
//    while (true) {
//      switch (expression) {
//        case ConstantExpression constantExpression:
//          return constantExpression.Value;

//        case MethodCallExpression methodCallExpression:
//          expression = methodCallExpression.Arguments[0];
//          continue;

//        default:
//          return null;
//      }
//    }
//  }

//  private IEnumerable<string> GetNewExpressionString(NewExpression newExpression, string appendString = null) {
//    for (var i = 0; i < newExpression.Members.Count; i++) {
//      if (newExpression.Arguments[i].NodeType == ExpressionType.MemberAccess) {
//        yield return
//            appendString == null ?
//            $"[{newExpression.Members[i].Name}]" :
//            $"[{newExpression.Members[i].Name}] {appendString}";
//      } else {
//        yield return
//            appendString == null ?
//            $"[{newExpression.Members[i].Name}] = {CreateParameter(GetValue(newExpression.Arguments[i])).ParameterName}" :
//            $"[{newExpression.Members[i].Name}] = {CreateParameter(GetValue(newExpression.Arguments[i])).ParameterName}";
//      }
//    }
//  }

//  private object GetValue(Expression expression) {
//    object GetMemberValue(MemberInfo memberInfo, object container = null) {
//      switch (memberInfo) {
//        case FieldInfo fieldInfo:
//          return fieldInfo.GetValue(container);

//        case PropertyInfo propertyInfo:
//          return propertyInfo.GetValue(container);

//        default: return null;
//      }
//    }

//    switch (expression) {
//      case ConstantExpression constantExpression:
//        return constantExpression.Value;

//      case MemberExpression memberExpression when memberExpression.Expression is ConstantExpression constantExpression:
//        return GetMemberValue(memberExpression.Member, constantExpression.Value);

//      case MemberExpression memberExpression when memberExpression.Expression is null: // static
//        return GetMemberValue(memberExpression.Member);

//      case MethodCallExpression methodCallExpression:
//        return Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();

//      case null:
//        return null;
//    }

//    throw new NotSupportedException();
//  }

//  private bool IsNullConstant(Expression expression) => expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == null;

//  private IEnumerable<string> ParseExpression(Expression parent, Expression body, string appendString = null) {
//    switch (body) {
//      case MemberExpression memberExpression:
//        return appendString == null ?
//            new string[] { $"[{memberExpression.Member.Name}]" } :
//            new string[] { $"[{memberExpression.Member.Name}] {appendString}" };

//      case NewExpression newExpression:
//        return GetNewExpressionString(newExpression, appendString);

//      case ParameterExpression parameterExpression when parent is LambdaExpression lambdaExpression && lambdaExpression.ReturnType == parameterExpression.Type:
//        return new string[0];

//      case ConstantExpression constantExpression:
//        return constantExpression
//            .Type
//            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
//            .Select(x => $"[{x.Name}] = {CreateParameter(x.GetValue(constantExpression.Value)).ParameterName}");
//    }

//    throw new NotSupportedException();
//  }

//  private Expression ParseExpression(MethodCallExpression expression, List<string> commandList, string appendString = null) {
//    var unary = (UnaryExpression)expression.Arguments[1];
//    var lambdaExpression = (LambdaExpression)unary.Operand;

//    lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

//    commandList.AddRange(ParseExpression(lambdaExpression, lambdaExpression.Body, appendString));

//    return Visit(expression.Arguments[0]);
//  }

//  private Expression ParseExpression(MethodCallExpression expression, ref int? size) {
//    var sizeExpression = (ConstantExpression)expression.Arguments[1];

//    if (int.TryParse(sizeExpression.Value.ToString(), out var value)) {
//      size = value;
//      return Visit(expression.Arguments[0]);
//    }

//    throw new NotSupportedException();
//  }
//}

//public static class SimpleExpressionToSQLExtensions {
//  private static readonly MethodInfo _deleteMethod;
//  private static readonly MethodInfo _deleteMethodAsync;
//  private static readonly MethodInfo _toSqlStringMethod;
//  private static readonly MethodInfo _updateMethod;
//  private static readonly MethodInfo _updateMethodAsync;

//  static Extensions() {
//    var extensionType = typeof(Extensions);
//    _deleteMethod = extensionType.GetMethod(nameof(Extensions.Delete), BindingFlags.Static | BindingFlags.Public);
//    _updateMethod = extensionType.GetMethod(nameof(Extensions.Update), BindingFlags.Static | BindingFlags.Public);
//    _deleteMethodAsync = extensionType.GetMethod(nameof(Extensions.DeleteAsync), BindingFlags.Static | BindingFlags.Public);
//    _updateMethodAsync = extensionType.GetMethod(nameof(Extensions.Update), BindingFlags.Static | BindingFlags.Public);
//    _toSqlStringMethod = extensionType.GetMethod(nameof(Extensions.ToSqlString), BindingFlags.Static | BindingFlags.Public);
//  }

//  public static bool CanConvertToSqlDbType(this Type type) => type.ToSqlDbTypeInternal().HasValue;

//  public static int Delete<T>(this IQueryable<T> queryable) {
//    var simpleExpressionToSQL = new SimpleExpressionToSQL(queryable.AppendCall(_deleteMethod));
//    return simpleExpressionToSQL.ExecuteNonQuery();
//  }

//  public static async Task<int> DeleteAsync<T>(this IQueryable<T> queryable) {
//    var simpleExpressionToSQL = new SimpleExpressionToSQL(queryable.AppendCall(_deleteMethodAsync));
//    return await simpleExpressionToSQL.ExecuteNonQueryAsync();
//  }

//  public static string GetTableName<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class {
//    DbContext context = dbSet.GetService<ICurrentDbContext>().Context;
//    IModel model = context.Model;
//    IEntityType entityTypeOfFooBar = model
//        .GetEntityTypes()
//        .First(t => t.ClrType == typeof(TEntity));

//    IAnnotation tableNameAnnotation = entityTypeOfFooBar.GetAnnotation("Relational:TableName");

//    return tableNameAnnotation.Value.ToString();
//  }

//  public static string GetTableName(this IQueryable query, Type entity) {
//    var compiler = query.Provider.GetValueOfField<QueryCompiler>("_queryCompiler");
//    var model = compiler.GetValueOfField<IModel>("_model");
//    IEntityType entityTypeOfFooBar = model
//        .GetEntityTypes()
//        .First(t => t.ClrType == entity);

//    IAnnotation tableNameAnnotation = entityTypeOfFooBar.GetAnnotation("Relational:TableName");

//    return tableNameAnnotation.Value.ToString();
//  }

//  public static SqlDbType ToSqlDbType(this Type type) =>
//      type.ToSqlDbTypeInternal() ?? throw new InvalidCastException($"Unable to cast from '{type}' to '{typeof(DbType)}'.");

//  public static string ToSqlString<T>(this IQueryable<T> queryable) => new SimpleExpressionToSQL(queryable.AppendCall(_toSqlStringMethod));

//  public static int Update<TSource, TResult>(this IQueryable<TSource> queryable, Expression<Func<TSource, TResult>> selector) {
//    var simpleExpressionToSQL = new SimpleExpressionToSQL(queryable.AppendCall(_updateMethod, selector));
//    return simpleExpressionToSQL.ExecuteNonQuery();
//  }

//  public static async Task<int> UpdateAsync<TSource, TResult>(this IQueryable<TSource> queryable, Expression<Func<TSource, TResult>> selector) {
//    var simpleExpressionToSQL = new SimpleExpressionToSQL(queryable.AppendCall(_updateMethodAsync, selector));
//    return await simpleExpressionToSQL.ExecuteNonQueryAsync();
//  }

//  internal static DbContext GetDbContext(this IQueryable query) {
//    var compiler = query.Provider.GetValueOfField<QueryCompiler>("_queryCompiler");
//    var queryContextFactory = compiler.GetValueOfField<RelationalQueryContextFactory>("_queryContextFactory");
//    var dependencies = queryContextFactory.GetValueOfField<QueryContextDependencies>("_dependencies");

//    return dependencies.CurrentContext.Context;
//  }

//  internal static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

//  internal static bool RequiresQuotes(this SqlDbType sqlDbType) {
//    switch (sqlDbType) {
//      case SqlDbType.Char:
//      case SqlDbType.Date:
//      case SqlDbType.DateTime:
//      case SqlDbType.DateTime2:
//      case SqlDbType.DateTimeOffset:
//      case SqlDbType.NChar:
//      case SqlDbType.NText:
//      case SqlDbType.Time:
//      case SqlDbType.SmallDateTime:
//      case SqlDbType.Text:
//      case SqlDbType.UniqueIdentifier:
//      case SqlDbType.Timestamp:
//      case SqlDbType.VarChar:
//      case SqlDbType.Xml:
//      case SqlDbType.Variant:
//      case SqlDbType.NVarChar:
//        return true;

//      default:
//        return false;
//    }
//  }

//  internal static unsafe string ToCamelCase(this string value) {
//    if (value == null || value.Length == 0) {
//      return value;
//    }

//    var result = string.Copy(value);

//    fixed (char* chr = result) {
//      var valueChar = *chr;
//      *chr = char.ToLowerInvariant(valueChar);
//    }

//    return result;
//  }

//  private static IQueryable<TResult> AppendCall<TSource, TResult>(this IQueryable<TSource> queryable, MethodInfo methodInfo, Expression<Func<TSource, TResult>> selector) {
//    MethodInfo methodInfoGeneric = methodInfo.MakeGenericMethod(typeof(TSource), typeof(TResult));
//    var methodCallExpression = Expression.Call(methodInfoGeneric, queryable.Expression, selector);

//    return new EntityQueryable<TResult>(queryable.Provider as IAsyncQueryProvider, methodCallExpression);
//  }

//  private static IQueryable<T> AppendCall<T>(this IQueryable<T> queryable, MethodInfo methodInfo) {
//    MethodInfo methodInfoGeneric = methodInfo.MakeGenericMethod(typeof(T));
//    var methodCallExpression = Expression.Call(methodInfoGeneric, queryable.Expression);

//    return new EntityQueryable<T>(queryable.Provider as IAsyncQueryProvider, methodCallExpression);
//  }

//  private static T GetValueOfField<T>(this object obj, string name) {
//    FieldInfo field = obj
//        .GetType()
//        .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

//    return (T)field.GetValue(obj);
//  }

//  [SuppressMessage("Style", "IDE0011:Add braces", Justification = "Easier to read than with Allman braces")]
//  private static SqlDbType? ToSqlDbTypeInternal(this Type type) {
//    if (Nullable.GetUnderlyingType(type) is Type nullableType)
//      return nullableType.ToSqlDbTypeInternal();

//    if (type.IsEnum)
//      return Enum.GetUnderlyingType(type).ToSqlDbTypeInternal();

//    if (type == typeof(long))            /**/                return SqlDbType.BigInt;
//    if (type == typeof(byte[]))          /**/                return SqlDbType.VarBinary;
//    if (type == typeof(bool))            /**/                return SqlDbType.Bit;
//    if (type == typeof(string))          /**/                return SqlDbType.NVarChar;
//    if (type == typeof(DateTime))        /**/                return SqlDbType.DateTime2;
//    if (type == typeof(decimal))         /**/                return SqlDbType.Decimal;
//    if (type == typeof(double))          /**/                return SqlDbType.Float;
//    if (type == typeof(int))             /**/                return SqlDbType.Int;
//    if (type == typeof(float))           /**/                return SqlDbType.Real;
//    if (type == typeof(Guid))            /**/                return SqlDbType.UniqueIdentifier;
//    if (type == typeof(short))           /**/                return SqlDbType.SmallInt;
//    if (type == typeof(object))          /**/                return SqlDbType.Variant;
//    if (type == typeof(DateTimeOffset))  /**/                return SqlDbType.DateTimeOffset;
//    if (type == typeof(TimeSpan))        /**/                return SqlDbType.Time;
//    if (type == typeof(byte))            /**/                return SqlDbType.TinyInt;

//    return null;
//  }
//}
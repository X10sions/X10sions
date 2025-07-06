using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq.SqlClient.Common;
internal class SqlFactory {
    private MetaModel model;
    internal TypeSystemProvider TypeProvider { get; }

    internal SqlFactory(TypeSystemProvider typeProvider, MetaModel model) {
      TypeProvider = typeProvider;
      this.model = model;
    }

    internal SqlExpression ConvertTo(Type clrType, ProviderType sqlType, SqlExpression expr) => UnaryConvert(clrType, sqlType, expr, expr.SourceExpression);

    internal SqlExpression ConvertTo(Type clrType, SqlExpression expr) {
      if (clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
        clrType = clrType.GetGenericArguments()[0];
      }
      var flag = clrType == typeof(TimeSpan);
      if (IsSqlTimeType(expr)) {
        if (flag) {
          return expr;
        }
        expr = ConvertToDateTime(expr);
      }
      return UnaryConvert(clrType, TypeProvider.From(clrType), expr, expr.SourceExpression);
    }

    internal SqlExpression ConvertToBigint(SqlExpression expr) => ConvertTo(typeof(long), expr);
    internal SqlExpression ConvertToInt(SqlExpression expr) => ConvertTo(typeof(int), expr);
    internal SqlExpression ConvertToDouble(SqlExpression expr) => ConvertTo(typeof(double), expr);
    internal SqlExpression ConvertTimeToDouble(SqlExpression exp) {
      if (!IsSqlTimeType(exp)) {
        return exp;
      }
      return ConvertToDouble(exp);
    }

    internal SqlExpression ConvertToBool(SqlExpression expr) => ConvertTo(typeof(bool), expr);

    internal SqlExpression ConvertToDateTime(SqlExpression expr) => UnaryConvert(typeof(DateTime), TypeProvider.From(typeof(DateTime)), expr, expr.SourceExpression);

    internal SqlExpression AndAccumulate(SqlExpression left, SqlExpression right) {
      if (left == null) {
        return right;
      }
      if (right == null) {
        return left;
      }
      return Binary(SqlNodeType.And, left, right);
    }

    internal SqlExpression OrAccumulate(SqlExpression left, SqlExpression right) {
      if (left == null) {
        return right;
      }
      if (right == null) {
        return left;
      }
      return Binary(SqlNodeType.Or, left, right);
    }

    internal SqlExpression Concat(params SqlExpression[] expressions) {
      var sqlExpression = expressions[expressions.Length - 1];
      for (var num = expressions.Length - 2; num >= 0; num--) {
        sqlExpression = Binary(SqlNodeType.Concat, expressions[num], sqlExpression);
      }
      return sqlExpression;
    }

    internal SqlExpression Add(params SqlExpression[] expressions) {
      var sqlExpression = expressions[expressions.Length - 1];
      for (var num = expressions.Length - 2; num >= 0; num--) {
        sqlExpression = Binary(SqlNodeType.Add, expressions[num], sqlExpression);
      }
      return sqlExpression;
    }

    internal SqlExpression Subtract(SqlExpression first, SqlExpression second) => Binary(SqlNodeType.Sub, first, second);

    internal SqlExpression Multiply(params SqlExpression[] expressions) {
      var sqlExpression = expressions[expressions.Length - 1];
      for (var num = expressions.Length - 2; num >= 0; num--) {
        sqlExpression = Binary(SqlNodeType.Mul, expressions[num], sqlExpression);
      }
      return sqlExpression;
    }

    internal SqlExpression Divide(SqlExpression first, SqlExpression second) => Binary(SqlNodeType.Div, first, second);

    internal SqlExpression Add(SqlExpression expr, int second) => Binary(SqlNodeType.Add, expr, ValueFromObject(second, false, expr.SourceExpression));

    internal SqlExpression Subtract(SqlExpression expr, int second) => Binary(SqlNodeType.Sub, expr, ValueFromObject(second, false, expr.SourceExpression));

    internal SqlExpression Multiply(SqlExpression expr, long second) => Binary(SqlNodeType.Mul, expr, ValueFromObject(second, false, expr.SourceExpression));

    internal SqlExpression Divide(SqlExpression expr, long second) => Binary(SqlNodeType.Div, expr, ValueFromObject(second, false, expr.SourceExpression));

    internal SqlExpression Mod(SqlExpression expr, long second) => Binary(SqlNodeType.Mod, expr, ValueFromObject(second, false, expr.SourceExpression));

    internal SqlExpression LEN(SqlExpression expr) => FunctionCall(typeof(int), "LEN", new SqlExpression[1]
      {
      expr
      }, expr.SourceExpression);

    internal SqlExpression DATALENGTH(SqlExpression expr) => FunctionCall(typeof(int), "DATALENGTH", new SqlExpression[1]
      {
      expr
      }, expr.SourceExpression);

    internal SqlExpression CLRLENGTH(SqlExpression expr) => Unary(SqlNodeType.ClrLength, expr);

    internal SqlExpression DATEPART(string partName, SqlExpression expr) => FunctionCall(typeof(int), "DATEPART", new SqlExpression[2]
      {
      new SqlVariable(typeof(void), null, partName, expr.SourceExpression),
      expr
      }, expr.SourceExpression);

    internal SqlExpression DATEADD(string partName, SqlExpression value, SqlExpression expr) => DATEADD(partName, value, expr, expr.SourceExpression, false);

    internal SqlExpression DATEADD(string partName, SqlExpression value, SqlExpression expr, Expression sourceExpression, bool asNullable) {
      var clrType = asNullable ? typeof(DateTime?) : typeof(DateTime);
      return FunctionCall(clrType, "DATEADD", new SqlExpression[3]
      {
      new SqlVariable(typeof(void), null, partName, sourceExpression),
      value,
      expr
      }, sourceExpression);
    }

    internal SqlExpression DATETIMEOFFSETADD(string partName, SqlExpression value, SqlExpression expr) => DATETIMEOFFSETADD(partName, value, expr, expr.SourceExpression, false);

    internal SqlExpression DATETIMEOFFSETADD(string partName, SqlExpression value, SqlExpression expr, Expression sourceExpression, bool asNullable) {
      var clrType = asNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
      return FunctionCall(clrType, "DATEADD", new SqlExpression[3]
      {
      new SqlVariable(typeof(void), null, partName, sourceExpression),
      value,
      expr
      }, sourceExpression);
    }

    internal SqlExpression AddTimeSpan(SqlExpression dateTime, SqlExpression timeSpan) => AddTimeSpan(dateTime, timeSpan, false);

    internal SqlExpression AddTimeSpan(SqlExpression dateTime, SqlExpression timeSpan, bool asNullable) {
      var value = DATEPART("NANOSECOND", timeSpan);
      var value2 = DATEPART("MILLISECOND", timeSpan);
      var value3 = DATEPART("SECOND", timeSpan);
      var value4 = DATEPART("MINUTE", timeSpan);
      var value5 = DATEPART("HOUR", timeSpan);
      var expr = (!IsSqlHighPrecisionDateTimeType(dateTime)) ? DATEADD("MILLISECOND", value2, dateTime, dateTime.SourceExpression, asNullable) : DATEADD("NANOSECOND", value, dateTime, dateTime.SourceExpression, asNullable);
      expr = DATEADD("SECOND", value3, expr, dateTime.SourceExpression, asNullable);
      expr = DATEADD("MINUTE", value4, expr, dateTime.SourceExpression, asNullable);
      expr = DATEADD("HOUR", value5, expr, dateTime.SourceExpression, asNullable);
      if (IsSqlDateTimeOffsetType(dateTime)) {
        return ConvertTo(typeof(DateTimeOffset), expr);
      }
      return expr;
    }

    internal static bool IsSqlDateTimeType(SqlExpression exp) {
      var sqlDbType = ((SqlTypeSystem.SqlType)exp.SqlType).SqlDbType;
      if (sqlDbType != SqlDbType.DateTime) {
        return sqlDbType == SqlDbType.SmallDateTime;
      }
      return true;
    }

    internal static bool IsSqlDateType(SqlExpression exp) => ((SqlTypeSystem.SqlType)exp.SqlType).SqlDbType == SqlDbType.Date;

    internal static bool IsSqlTimeType(SqlExpression exp) => ((SqlTypeSystem.SqlType)exp.SqlType).SqlDbType == SqlDbType.Time;

    internal static bool IsSqlDateTimeOffsetType(SqlExpression exp) => ((SqlTypeSystem.SqlType)exp.SqlType).SqlDbType == SqlDbType.DateTimeOffset;

    internal static bool IsSqlHighPrecisionDateTimeType(SqlExpression exp) {
      var sqlDbType = ((SqlTypeSystem.SqlType)exp.SqlType).SqlDbType;
      if (sqlDbType != SqlDbType.Time && sqlDbType != SqlDbType.DateTime2) {
        return sqlDbType == SqlDbType.DateTimeOffset;
      }
      return true;
    }

    internal SqlExpression Value(Type clrType, ProviderType sqlType, object value, bool isClientSpecified, Expression sourceExpression) {
      if (typeof(Type).IsAssignableFrom(clrType) && value != null) {
        var metaType = model.GetMetaType((Type)value);
        return StaticType(metaType, sourceExpression);
      }
      return new SqlValue(clrType, sqlType, value, isClientSpecified, sourceExpression);
    }

    internal SqlExpression StaticType(MetaType typeOf, Expression sourceExpression) {
      if (typeOf == null) {
        throw System.Data.Linq.SqlClient.Error.ArgumentNull("typeOf");
      }
      if (typeOf.InheritanceCode == null) {
        return new SqlValue(typeof(Type), TypeProvider.From(typeof(Type)), typeOf.Type, false, sourceExpression);
      }
      var type = typeOf.InheritanceCode.GetType();
      var discriminator = new SqlValue(type, TypeProvider.From(type), typeOf.InheritanceCode, true, sourceExpression);
      return DiscriminatedType(discriminator, typeOf);
    }

    internal SqlExpression DiscriminatedType(SqlExpression discriminator, MetaType targetType) => new SqlDiscriminatedType(TypeProvider.From(typeof(Type)), discriminator, targetType, discriminator.SourceExpression);

    internal SqlTable Table(MetaTable table, MetaType rowType, Expression sourceExpression) => new SqlTable(table, rowType, TypeProvider.GetApplicationType(0), sourceExpression);

    internal SqlUnary Unary(SqlNodeType nodeType, SqlExpression expression) => Unary(nodeType, expression, expression.SourceExpression);

    internal SqlRowNumber RowNumber(List<SqlOrderExpression> orderBy, Expression sourceExpression) => new SqlRowNumber(typeof(long), TypeProvider.From(typeof(long)), orderBy, sourceExpression);

    internal SqlUnary Unary(SqlNodeType nodeType, SqlExpression expression, Expression sourceExpression) => Unary(nodeType, expression, null, sourceExpression);

    internal SqlUnary Unary(SqlNodeType nodeType, SqlExpression expression, MethodInfo method, Expression sourceExpression) {
      Type type = null;
      ProviderType providerType = null;
      switch (nodeType) {
        case SqlNodeType.Count:
          type = typeof(int);
          providerType = TypeProvider.From(typeof(int));
          break;
        case SqlNodeType.LongCount:
          type = typeof(long);
          providerType = TypeProvider.From(typeof(long));
          break;
        case SqlNodeType.ClrLength:
          type = typeof(int);
          providerType = TypeProvider.From(typeof(int));
          break;
        default:
          type = ((!nodeType.IsPredicateUnaryOperator()) ? expression.ClrType : (expression.ClrType.Equals(typeof(bool?)) ? typeof(bool?) : typeof(bool)));
          providerType = TypeProvider.PredictTypeForUnary(nodeType, expression.SqlType);
          break;
      }
      return new SqlUnary(nodeType, type, providerType, expression, method, sourceExpression);
    }

    internal SqlUnary UnaryConvert(Type targetClrType, ProviderType targetSqlType, SqlExpression expression, Expression sourceExpression) => new SqlUnary(SqlNodeType.Convert, targetClrType, targetSqlType, expression, null, sourceExpression);

    internal SqlUnary UnaryValueOf(SqlExpression expression, Expression sourceExpression) {
      var nonNullableType = TypeSystem.GetNonNullableType(expression.ClrType);
      return new SqlUnary(SqlNodeType.ValueOf, nonNullableType, expression.SqlType, expression, null, sourceExpression);
    }

    internal SqlBinary Binary(SqlNodeType nodeType, SqlExpression left, SqlExpression right) => Binary(nodeType, left, right, null, null);

    internal SqlBinary Binary(SqlNodeType nodeType, SqlExpression left, SqlExpression right, MethodInfo method) => Binary(nodeType, left, right, method, null);

    internal SqlBinary Binary(SqlNodeType nodeType, SqlExpression left, SqlExpression right, Type clrType) => Binary(nodeType, left, right, null, clrType);

    internal SqlBinary Binary(SqlNodeType nodeType, SqlExpression left, SqlExpression right, MethodInfo method, Type clrType) {
      ProviderType providerType = null;
      if (nodeType.IsPredicateBinaryOperator()) {
        if (clrType == null) {
          clrType = typeof(bool);
        }
        providerType = TypeProvider.From(clrType);
      } else {
        var providerType2 = TypeProvider.PredictTypeForBinary(nodeType, left.SqlType, right.SqlType);
        if (providerType2 == right.SqlType) {
          if (clrType == null) {
            clrType = right.ClrType;
          }
          providerType = right.SqlType;
        } else if (providerType2 == left.SqlType) {
          if (clrType == null) {
            clrType = left.ClrType;
          }
          providerType = left.SqlType;
        } else {
          providerType = providerType2;
          if (clrType == null) {
            clrType = providerType2.GetClosestRuntimeType();
          }
        }
      }
      return new SqlBinary(nodeType, clrType, providerType, left, right, method);
    }

    internal SqlBetween Between(SqlExpression expr, SqlExpression start, SqlExpression end, Expression source) => new SqlBetween(typeof(bool), TypeProvider.From(typeof(bool)), expr, start, end, source);

    internal SqlIn In(SqlExpression expr, IEnumerable<SqlExpression> values, Expression source) => new SqlIn(typeof(bool), TypeProvider.From(typeof(bool)), expr, values, source);

    internal SqlLike Like(SqlExpression expr, SqlExpression pattern, SqlExpression escape, Expression source) => new SqlLike(typeof(bool), TypeProvider.From(typeof(bool)), expr, pattern, escape, source);

    internal SqlSearchedCase SearchedCase(SqlWhen[] whens, SqlExpression @else, Expression sourceExpression) => new SqlSearchedCase(whens[0].Value.ClrType, whens, @else, sourceExpression);

    internal SqlExpression Case(Type clrType, SqlExpression discriminator, List<SqlExpression> matches, List<SqlExpression> values, Expression sourceExpression) {
      if (values.Count == 0) {
        throw System.Data.Linq.SqlClient.Error.EmptyCaseNotSupported();
      }
      var flag = false;
      foreach (var value in values) {
        flag |= value.IsClientAidedExpression();
      }
      if (flag) {
        var list = new List<SqlClientWhen>();
        var i = 0;
        for (var count = matches.Count; i < count; i++) {
          list.Add(new SqlClientWhen(matches[i], values[i]));
        }
        return new SqlClientCase(clrType, discriminator, list, sourceExpression);
      }
      var list2 = new List<SqlWhen>();
      var j = 0;
      for (var count2 = matches.Count; j < count2; j++) {
        list2.Add(new SqlWhen(matches[j], values[j]));
      }
      return new SqlSimpleCase(clrType, discriminator, list2, sourceExpression);
    }

    internal SqlExpression Parameter(object value, Expression source) {
      var type = value.GetType();
      return Value(type, TypeProvider.From(value), value, true, source);
    }

    internal SqlExpression ValueFromObject(object value, Expression sourceExpression) => ValueFromObject(value, false, sourceExpression);

    internal SqlExpression ValueFromObject(object value, bool isClientSpecified, Expression sourceExpression) {
      if (value == null) {
        throw System.Data.Linq.SqlClient.Error.ArgumentNull("value");
      }
      var type = value.GetType();
      return ValueFromObject(value, type, isClientSpecified, sourceExpression);
    }

    internal SqlExpression ValueFromObject(object value, Type clrType, bool isClientSpecified, Expression sourceExpression) {
      if (clrType == null) {
        throw System.Data.Linq.SqlClient.Error.ArgumentNull("clrType");
      }
      var sqlType = (value == null) ? TypeProvider.From(clrType) : TypeProvider.From(value);
      return Value(clrType, sqlType, value, isClientSpecified, sourceExpression);
    }

    public SqlExpression TypedLiteralNull(Type type, Expression sourceExpression) => ValueFromObject(null, type, false, sourceExpression);

    internal SqlMember Member(SqlExpression expr, MetaDataMember member) => new SqlMember(member.Type, Default(member), expr, member.Member);

    internal SqlMember Member(SqlExpression expr, MemberInfo member) {
      var memberType = TypeSystem.GetMemberType(member);
      var metaType = model.GetMetaType(member.DeclaringType);
      var dataMember = metaType.GetDataMember(member);
      if (metaType != null && dataMember != null) {
        return new SqlMember(memberType, Default(dataMember), expr, member);
      }
      return new SqlMember(memberType, Default(memberType), expr, member);
    }

    internal SqlExpression TypeCase(Type clrType, MetaType rowType, SqlExpression discriminator, IEnumerable<SqlTypeCaseWhen> whens, Expression sourceExpression) => new SqlTypeCase(clrType, TypeProvider.From(clrType), rowType, discriminator, whens, sourceExpression);

    internal SqlNew New(MetaType type, ConstructorInfo cons, IEnumerable<SqlExpression> args, IEnumerable<MemberInfo> argMembers, IEnumerable<SqlMemberAssign> bindings, Expression sourceExpression) => new SqlNew(type, TypeProvider.From(type.Type), cons, args, argMembers, bindings, sourceExpression);

    internal SqlMethodCall MethodCall(MethodInfo method, SqlExpression obj, SqlExpression[] args, Expression sourceExpression) => new SqlMethodCall(method.ReturnType, Default(method.ReturnType), method, obj, args, sourceExpression);

    internal SqlMethodCall MethodCall(Type returnType, MethodInfo method, SqlExpression obj, SqlExpression[] args, Expression sourceExpression) => new SqlMethodCall(returnType, Default(returnType), method, obj, args, sourceExpression);

    internal SqlExprSet ExprSet(SqlExpression[] exprs, Expression sourceExpression) => new SqlExprSet(exprs[0].ClrType, exprs, sourceExpression);

    internal SqlSubSelect SubSelect(SqlNodeType nt, SqlSelect select) => SubSelect(nt, select, null);

    internal SqlSubSelect SubSelect(SqlNodeType nt, SqlSelect select, Type clrType) {
      ProviderType sqlType = null;
      switch (nt) {
        case SqlNodeType.Element:
        case SqlNodeType.ScalarSubSelect:
          clrType = select.Selection.ClrType;
          sqlType = select.Selection.SqlType;
          break;
        case SqlNodeType.Multiset:
          if (clrType == null) {
            clrType = typeof(List<>).MakeGenericType(select.Selection.ClrType);
          }
          sqlType = TypeProvider.GetApplicationType(1);
          break;
        case SqlNodeType.Exists:
          clrType = typeof(bool);
          sqlType = TypeProvider.From(typeof(bool));
          break;
      }
      return new SqlSubSelect(nt, clrType, sqlType, select);
    }

    internal SqlDoNotVisitExpression DoNotVisitExpression(SqlExpression expr) => new SqlDoNotVisitExpression(expr);

    internal SqlFunctionCall FunctionCall(Type clrType, string name, IEnumerable<SqlExpression> args, Expression source) => new SqlFunctionCall(clrType, Default(clrType), name, args, source);

    internal SqlFunctionCall FunctionCall(Type clrType, ProviderType sqlType, string name, IEnumerable<SqlExpression> args, Expression source) => new SqlFunctionCall(clrType, sqlType, name, args, source);

    internal SqlTableValuedFunctionCall TableValuedFunctionCall(MetaType rowType, Type clrType, string name, IEnumerable<SqlExpression> args, Expression source) => new SqlTableValuedFunctionCall(rowType, clrType, Default(clrType), name, args, source);

    internal ProviderType Default(Type clrType) => TypeProvider.From(clrType);

    internal ProviderType Default(MetaDataMember member) {
      if (member == null) {
        throw System.Data.Linq.SqlClient.Error.ArgumentNull("member");
      }
      if (member.DbType != null) {
        return TypeProvider.Parse(member.DbType);
      }
      return TypeProvider.From(member.Type);
    }

    internal SqlJoin MakeJoin(SqlJoinType joinType, SqlSource location, SqlAlias alias, SqlExpression condition, Expression source) {
      if (joinType == SqlJoinType.LeftOuter) {
        var sqlSelect = alias.Node as SqlSelect;
        if (sqlSelect != null && sqlSelect.Selection != null && sqlSelect.Selection.NodeType != SqlNodeType.OptionalValue) {
          sqlSelect.Selection = new SqlOptionalValue(new SqlColumn("test", Unary(SqlNodeType.OuterJoinedValue, Value(typeof(int?), TypeProvider.From(typeof(int)), 1, false, source))), sqlSelect.Selection);
        }
      }
      return new SqlJoin(joinType, location, alias, condition, source);
    }
  }

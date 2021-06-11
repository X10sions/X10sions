using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  public class DB2iSeriesSqlBuilder_TB : BasicSqlBuilder {
    public DB2iSeriesSqlBuilder_TB(DB2iSeriesDataProvider_TB provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(mappingSchema, sqlOptimizer, sqlProviderFlags) {
      Provider = provider;
    }

    public DB2iSeriesDataProvider_TB Provider { get; }

    protected override string? LimitFormat(SelectQuery selectQuery) => selectQuery.Select.SkipValue == null ? " FETCH FIRST {0} ROWS ONLY " : null;

    protected override void BuildColumnExpression(SelectQuery selectQuery, ISqlExpression expr, string alias, ref bool addAlias) {
      var wrap = false;
      if (expr.SystemType == typeof(bool)) {
        if (expr is SqlSearchCondition)
          wrap = true;
        else {
          var ex = expr as SqlExpression;
          wrap = ex != null && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SqlSearchCondition;
        }
      }
      // If TypeOf expr Is SqlParameter Then
      // If DirectCast(expr, SqlParameter).Name IsNot Nothing Then
      // Dim dataType = SqlDataType.GetDataType(expr.SystemType)
      // expr = New SqlFunction(expr.SystemType, dataType.DataType.ToString, expr)
      // End If
      // ElseIf (TypeOf expr Is SqlValue) AndAlso DirectCast(expr, SqlValue).Value Is Nothing Then
      // Dim colType As String = "CHAR"
      // If expr.SystemType IsNot Nothing Then
      // Dim actualType = SqlDataType.GetDataType(expr.SystemType)
      // colType = DB2iSeriesMappingSchema.GetiSeriesType(actualType)
      // End If
      // expr = New SqlExpression(expr.SystemType, "Cast({0} as {1})", Precedence.Primary, expr, New SqlExpression(colType, Precedence.Primary))
      // End If

      if (wrap)
        StringBuilder.Append("CASE WHEN ");
      base.BuildColumnExpression(selectQuery, expr, alias, ref addAlias);
      if (wrap)
        StringBuilder.Append(" THEN 1 ELSE 0 END");
    }

    protected override void BuildCommand(SqlStatement statement, int commandNumber) => StringBuilder.AppendLine(Provider.NamingConvention.SelectIdentityFromDummyTableSql());

    protected override void BuildCreateTableIdentityAttribute1(SqlField field) => StringBuilder.Append(" GENERATED ALWAYS As IDENTITY ");

    protected void xBuildDataType(SqlDataType type, bool createDbType) {
      switch (type.Type.DataType) {
        case DataType.DateTime: {
            StringBuilder.Append("timestamp");
            break;
          }
        case DataType.DateTime2: {
            StringBuilder.Append("timestamp");
            break;
          }
        default: {
            base.BuildDataType(type, createDbType);
            break;
          }
      }
    }

    //protected override void BuildEmptyInsert() => base.BuildEmptyInsert(insertClause);
    protected override void BuildEmptyInsert(SqlInsertClause insertClause) {
      StringBuilder.Append("VALUES");
      foreach (var col in insertClause.Into.Fields)
        StringBuilder.Append("(Default)");
      StringBuilder.AppendLine();
    }

    //protected override void BuildFunction(SqlFunction func) {
    //  // http://blog.linq2db.com/
    //  func =  ConvertFunctionParameters(func);
    //  switch (func.Name.ToLower()) {
    //    case "coalesce": {
    //        func = new SqlFunction(func.SystemType, "Value", func.Parameters[0], func.Parameters[1]);
    //        break;
    //      }
    //    case "replicate": {
    //        func = new SqlFunction(func.SystemType, "Repeat", func.Parameters[0], func.Parameters[1]);
    //        break;
    //      }
    //    case "x_upper": {
    //        func = new SqlFunction(func.SystemType, "sqlbuilder_UCase", func.Parameters[0]);
    //        break;
    //      }
    //  }
    //  base.BuildFunction(func);
    //}

    protected override void BuildFromClause(SqlStatement statement, SelectQuery selectQuery) {
      if (!statement.IsUpdate()) {
        if (selectQuery.From.Tables.Count == 0) StringBuilder.Append($"FROM {Provider.NamingConvention.DummyTableWithSchema()}").AppendLine();
        base.BuildFromClause(statement, selectQuery);
      }
    }

    protected override void BuildInsertOrUpdateQuery(SqlInsertOrUpdateStatement insertOrUpdate) {
      if (Provider.VersionRelease.SupportsMergeStatement())
        base.BuildInsertOrUpdateQuery(insertOrUpdate);
      else
        BuildInsertOrUpdateQueryAsMerge(insertOrUpdate, $"FROM {Provider.NamingConvention.DummyTableWithSchema()} FETCH FIRST 1 ROW ONLY");
    }

    // Protected Overrides Sub BuildPredicate(predicate As ISqlPredicate)
    // Select Case predicate.ElementType
    // 'Case QueryElementType.ExprPredicate
    // '    Dim p As SelectQuery.Predicate.Expr = DirectCast(predicate, SelectQuery.Predicate.Expr)
    // '    If TypeOf p.Expr1 Is SqlValue Then
    // '      Dim value As Object = DirectCast(p.Expr1, SqlValue).Value
    // '      If TypeOf value Is Boolean Then
    // '        Me.StringBuilder.Append(If(CBool(value), "1 = 1", "1 = 0"))
    // '        Return
    // '      End If
    // '    End If
    // '    Me.BuildExpression(BasicSqlBuilder.GetPrecedence(p), p.Expr1)
    // 'Case QueryElementType.NotExprPredicate
    // '    Dim p2 As SelectQuery.Predicate.NotExpr = DirectCast(predicate, SelectQuery.Predicate.NotExpr)
    // '    If p2.[IsNot] Then
    // '      Me.StringBuilder.Append("Not ")
    // '    End If
    // '    Me.BuildExpression(If(p2.[IsNot], 30, BasicSqlBuilder.GetPrecedence(p2)), p2.Expr1)
    // Case QueryElementType.ExprExprPredicate
    // Dim expr As SelectQuery.Predicate.ExprExpr = DirectCast(predicate, SelectQuery.Predicate.ExprExpr)
    // Select Case expr.Operator
    // Case SelectQuery.Predicate.Operator.Equal, SelectQuery.Predicate.Operator.NotEqual
    // Dim e As ISqlExpression = Nothing
    // If TypeOf expr.Expr1 Is IValueContainer AndAlso DirectCast(expr.Expr1, IValueContainer).Value Is Nothing Then
    // e = expr.Expr2
    // ElseIf TypeOf expr.Expr2 Is IValueContainer AndAlso DirectCast(expr.Expr2, IValueContainer).Value Is Nothing Then
    // e = expr.Expr1
    // End If
    // If e IsNot Nothing Then
    // Me.BuildExpression(BasicSqlBuilder.GetPrecedence(expr), e)
    // Me.StringBuilder.Append(If((expr.Operator = SelectQuery.Predicate.[Operator].Equal), " Is NULL", " Is Not NULL"))
    // Return
    // End If
    // End Select
    // Me.BuildExpression(BasicSqlBuilder.GetPrecedence(expr), expr.Expr1)
    // Me.StringBuilder.Append("xxx")
    // Select Case expr.Operator
    // Case SelectQuery.Predicate.Operator.Equal : Me.StringBuilder.Append(" = ")
    // Case SelectQuery.Predicate.Operator.NotEqual : Me.StringBuilder.Append(" <> ")
    // Case SelectQuery.Predicate.Operator.Greater : Me.StringBuilder.Append(" > ")
    // Case SelectQuery.Predicate.Operator.GreaterOrEqual : Me.StringBuilder.Append(" >= ")
    // Case SelectQuery.Predicate.Operator.NotGreater : Me.StringBuilder.Append(" !> ")
    // Case SelectQuery.Predicate.Operator.Less : Me.StringBuilder.Append(" < ")
    // Case SelectQuery.Predicate.Operator.LessOrEqual : Me.StringBuilder.Append(" <= ")
    // Case SelectQuery.Predicate.Operator.NotLess : Me.StringBuilder.Append(" !< ")
    // End Select
    // Me.BuildExpression(BasicSqlBuilder.GetPrecedence(expr), expr.Expr2)
    // Case Else
    // MyBase.BuildPredicate(predicate)
    // End Select
    // End Sub

    protected override void BuildSelectClause(SelectQuery selectQuery) {
      if (selectQuery.From.Tables.Count == 0) {
        AppendIndent().AppendLine("Select");
        BuildColumns(selectQuery);
        AppendIndent().AppendLine($"FROM {Provider.NamingConvention.DummyTableWithSchema()} FETCH FIRST 1 ROW ONLY");
      } else
        base.BuildSelectClause(selectQuery);
    }

    #region Obsolete

    //protected override void BuildSql() => AlternativeBuildSql_Obsolete(true, base.BuildSql, "\t0");

    //[Obsolete]
    //protected void AlternativeBuildSql_Obsolete(bool implementOrderBy, Action buildSql, string emptyOrderByValue) {
    //  var selectQuery = Statement.SelectQuery;
    //  if (selectQuery != null && NeedSkip(selectQuery.Select.TakeValue, selectQuery.Select.SkipValue)) {
    //    SkipAlias = false;
    //    var aliases = GetTempAliases(2, "t");
    //    var rnaliase = GetTempAliases(1, "rn")[0];
    //    AppendIndent().Append("SELECT *").AppendLine();
    //    AppendIndent().Append("FROM").AppendLine();
    //    AppendIndent().Append("(").AppendLine();
    //    Indent++;
    //    AppendIndent().Append("SELECT").AppendLine();
    //    Indent++;
    //    AppendIndent().AppendFormat("{0}.*,", aliases[0]).AppendLine();
    //    AppendIndent().Append("ROW_NUMBER() OVER");
    //    if (!selectQuery.OrderBy.IsEmpty && !implementOrderBy)
    //      StringBuilder.Append("()");
    //    else {
    //      StringBuilder.AppendLine();
    //      AppendIndent().Append("(").AppendLine();
    //      Indent++;
    //      if (selectQuery.OrderBy.IsEmpty) {
    //        AppendIndent().Append("ORDER BY").AppendLine();
    //        if (selectQuery.Select.Columns.Count > 0)
    //          BuildAliases_Obsolete(aliases[0], selectQuery.Select.Columns.Take(1).ToList(), null);
    //        else
    //          AppendIndent().Append(emptyOrderByValue).AppendLine();
    //      } else
    //        BuildAlternativeOrderBy_Obsolete(true);
    //      Indent--;
    //      AppendIndent().Append(")");
    //    }
    //    StringBuilder.Append(" as ").Append(rnaliase).AppendLine();
    //    Indent--;
    //    AppendIndent().Append("FROM").AppendLine();
    //    AppendIndent().Append("(").AppendLine();
    //    Indent++;
    //    buildSql();
    //    Indent--;
    //    AppendIndent().AppendFormat(") {0}", aliases[0]).AppendLine();
    //    Indent--;
    //    AppendIndent().AppendFormat(") {0}", aliases[1]).AppendLine();
    //    AppendIndent().Append("WHERE").AppendLine();
    //    Indent++;
    //    if (NeedTake(selectQuery)) {
    //      var expr1 = Add_Obsolete(selectQuery.Select.SkipValue!, 1);
    //      var expr2 = Add_Obsolete<int>(selectQuery.Select.SkipValue!, selectQuery.Select.TakeValue!);
    //      if (expr1 is SqlValue value1 && expr2 is SqlValue value2 && Equals(value1.Value, value2.Value)) {
    //        AppendIndent().AppendFormat("{0}.{1} = ", aliases[1], rnaliase);
    //        BuildExpression(expr1);
    //      } else {
    //        AppendIndent().AppendFormat("{0}.{1} BETWEEN ", aliases[1], rnaliase);
    //        BuildExpression(expr1);
    //        StringBuilder.Append(" AND ");
    //        BuildExpression(expr2);
    //      }
    //    } else {
    //      AppendIndent().AppendFormat("{0}.{1} > ", aliases[1], rnaliase);
    //      BuildExpression(selectQuery.Select.SkipValue!);
    //    }
    //    StringBuilder.AppendLine();
    //    Indent--;
    //  } else
    //    buildSql();
    //}

    //[Obsolete]
    //ISqlExpression Add_Obsolete(ISqlExpression expr1, ISqlExpression expr2, Type type)
    //  => SqlOptimizer.ConvertExpression(new SqlBinaryExpression(type, expr1, "+", expr2, Precedence.Additive), false);

    //[Obsolete] protected ISqlExpression Add_Obsolete<T>(ISqlExpression expr1, ISqlExpression expr2) => Add_Obsolete(expr1, expr2, typeof(T));

    //[Obsolete]
    //void BuildAliases_Obsolete(string table, List<SqlColumn> columns, string? postfix) {
    //  Indent++;
    //  var first = true;
    //  foreach (var col in columns) {
    //    if (!first)
    //      StringBuilder.AppendLine(Comma);
    //    first = false;
    //    AppendIndent().Append(table).Append('.');
    //    Convert(StringBuilder, col.Alias!, ConvertType.NameToQueryFieldAlias);
    //    if (postfix != null)
    //      StringBuilder.Append(postfix);
    //  }
    //  Indent--;
    //  StringBuilder.AppendLine();
    //}

    //[Obsolete]
    //void BuildAlternativeOrderBy_Obsolete(bool ascending) {
    //  var selectQuery = Statement.SelectQuery;
    //  if (selectQuery == null)
    //    return;
    //  SkipAlias = false;
    //  AppendIndent().Append("ORDER BY").AppendLine();
    //  var obys = GetTempAliases(selectQuery.OrderBy.Items.Count, "oby");
    //  Indent++;
    //  for (var i = 0; i < obys.Length; i++) {
    //    AppendIndent().Append(obys[i]);
    //    if (ascending && selectQuery.OrderBy.Items[i].IsDescending ||
    //      !ascending && !selectQuery.OrderBy.Items[i].IsDescending)
    //      StringBuilder.Append(" DESC");
    //    if (i + 1 < obys.Length)
    //      StringBuilder.Append(',');
    //    StringBuilder.AppendLine();
    //  }
    //  Indent--;
    //}

    #endregion

    public override int CommandCount(SqlStatement statement) => statement.NeedsIdentity() ? 2 : 1;

    //public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) => base.Convert(sb, value, convertType);
    public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) {
      switch (convertType) {
        case ConvertType.NameToQueryParameter:
          return sb.Append(Provider.SqlProviderFlags.IsParameterOrderDependent ? "?" : $"@{value}");
        //return sb.Append(Provider.ConnectionType.SupportsNamedParameters() ? $"@{value}" : "?");

        case ConvertType.NameToCommandParameter:
        case ConvertType.NameToSprocParameter:
          return sb.Append(Provider.SqlProviderFlags.IsParameterOrderDependent ? "?" : $":{value}");
        //return sb.Append(Provider.ConnectionType.SupportsNamedParameters() ? $":{value}" : "?");

        case ConvertType.SprocParameterToName:
          return sb.Append(value.Length > 0 && value[0] == ':' ? value.Substring(1) : value);

        case ConvertType.NameToQueryField:
        case ConvertType.NameToQueryFieldAlias:
        case ConvertType.NameToQueryTable:
        case ConvertType.NameToQueryTableAlias:
          if (Provider.IdentifierQuoteMode != DB2iSeriesIdentifierQuoteMode.None) {
            if (value.Length > 0 && value[0] == '"') {
              return sb.Append(value);
            }
            if (Provider.IdentifierQuoteMode == DB2iSeriesIdentifierQuoteMode.Quote ||
              value.StartsWith("_") ||
              value.Any(c => char.IsLower(c) || char.IsWhiteSpace(c)))
              return sb.Append('"' + value + '"');
          }
          break;
      }
      return sb.Append(value);
    }

    protected override ISqlBuilder CreateSqlBuilder() => new DB2iSeriesSqlBuilder_TB(Provider, MappingSchema, SqlOptimizer, SqlProviderFlags);

    protected override void BuildCreateTableNullAttribute(SqlField field, DefaultNullable defaulNullable) {
      if (defaulNullable == DefaultNullable.Null && field.CanBeNull)
        return;
      else if (defaulNullable == DefaultNullable.NotNull && !field.CanBeNull)
        return;
      StringBuilder.Append(field.CanBeNull ? "        " : "NOT NULL");
    }

    protected override string GetProviderTypeName(IDbDataParameter parameter) {
      // If parameter.DbType = DbType.Decimal AndAlso TypeOf parameter.Value Is Decimal Then
      // Dim d = New SqlDecimal(parameter.Value)
      // Return "(" & d.Precision & "," & d.Scale & ")"
      // End If
      var p = (iDB2Parameter)parameter;
      return p.iDB2DbType.ToString();
    }
  }
}

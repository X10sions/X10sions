using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public class xDB2iSeriesSqlBuilder<TDbConnection> : BasicSqlBuilder where TDbConnection : IDbConnection, new() {
    private xDB2iSeriesDataProviderBase<TDbConnection> dataProvider;

    protected override bool OffsetFirst {
      get {
        if (!dataProvider.Options.IsVersion7_2orLater) {
          return base.OffsetFirst;
        }
        return true;
      }
    }

    public xDB2iSeriesSqlBuilder(xDB2iSeriesDataProviderBase<TDbConnection> dataProvider, MappingSchema mappingSchema)
      : base(mappingSchema, dataProvider.GetSqlOptimizer(), dataProvider.SqlProviderFlags) {
      this.dataProvider = dataProvider;
    }

    public override int CommandCount(SqlStatement statement) {
      var insertStatement = statement as SqlInsertStatement;
      if (insertStatement == null || !insertStatement.Insert.WithIdentity) {
        return 1;
      }
      return 2;
    }

    public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) {
      switch (convertType) {
        case ConvertType.NameToQueryParameter: return sb.Append(dataProvider.DoUseNamedParameters ? ("@" + value) : "?");
        case ConvertType.NameToCommandParameter:
        case ConvertType.NameToSprocParameter: return sb.Append(dataProvider.DoUseNamedParameters ? (":" + value) : "?");
        case ConvertType.SprocParameterToName:
          if (value.Length <= 0 || value[0] != ':') {
            return sb.Append(value);
          }
          return sb.Append(value.Substring(1));
        case ConvertType.NameToQueryField:
        case ConvertType.NameToQueryFieldAlias:
        case ConvertType.NameToQueryTable:
        case ConvertType.NameToQueryTableAlias:
          if (dataProvider.Options.IdentifierQuoteMode != 0) {
            if (value.Length > 0 && value[0] == '"') {
              return sb.Append(value);
            }
            if (dataProvider.Options.IdentifierQuoteMode == DB2iSeriesIdentifierQuoteMode.Quote || value.StartsWith("_") || value.StartsWith("_") || value.Any((char c) => char.IsLower(c) || char.IsWhiteSpace(c))) {
              return sb.Append('"').Append(value).Append('"');
            }
          }
          break;
      }
      return sb.Append(value);
    }

    protected override void BuildColumnExpression(SelectQuery? selectQuery, ISqlExpression expr, string? alias, ref bool addAlias) {
      int num;
      if (!(expr.SystemType == typeof(bool))) {
        num = 0;
      } else if (!(expr is SqlSearchCondition)) {
        var ex = expr as SqlExpression;
        num = ((ex != null && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SqlSearchCondition) ? 1 : 0);
      } else {
        num = 1;
      }
      var wrap = (byte)num != 0;
      var parameter = expr as SqlParameter;
      if (parameter == null || parameter.Name == null) {
        var value = expr as SqlValue;
        if (value == null || value.Value != null) {
          goto IL_00d4;
        }
      }
      var colType = SqlDataType.GetDataType(expr.SystemType ?? typeof(object)).GetiSeriesType();
      expr = new SqlExpression(expr.SystemType, "Cast({0} as {1})", 100, expr, new SqlExpression(colType, 100));
      goto IL_00d4;
IL_00d4:
      if (wrap) {
        StringBuilder.Append("CASE WHEN ");
      }
      base.BuildColumnExpression(selectQuery, expr, alias, ref addAlias);
      if (wrap) {
        StringBuilder.Append(" THEN 1 ELSE 0 END");
      }
    }

    protected override void BuildCommand(SqlStatement selectQuery, int commandNumber)
      => StringBuilder.AppendLine("SELECT "+ DB2iSeriesConstants.IdentityColumnSql +" FROM " +  dataProvider.Options.NamingConvention.DummyTableWithSchema());

    protected override void BuildCreateTableIdentityAttribute1(SqlField field) => StringBuilder.Append("GENERATED ALWAYS AS IDENTITY");

    protected override void BuildCreateTableNullAttribute(SqlField field, DefaultNullable defaulNullable) {
      if ((defaulNullable != DefaultNullable.Null || !field.CanBeNull) && (defaulNullable != DefaultNullable.NotNull || field.CanBeNull)) {
        StringBuilder.Append(field.CanBeNull ? " " : "NOT NULL");
      }
    }

    protected override void BuildDataTypeFromDataType(SqlDataType type, bool forCreateTable) {
      switch (type.Type.DataType) {
        case DataType.DateTime:
        case DataType.DateTime2:
          StringBuilder.Append("timestamp");
          if (type.Type.Precision.HasValue && type.Type.Precision != 6) {
            StringBuilder.Append($"({type.Type.Precision})");
          }
          return;
        case DataType.UInt64:
          StringBuilder.Append("DECIMAL(28,0)");
          return;
        case DataType.Byte:
          StringBuilder.Append("smallint");
          return;
        case DataType.VarBinary:
          if (!type.Type.Length.HasValue || type.Type.Length > 32704 || type.Type.Length < 1) {
            StringBuilder.Append($"{type.Type.DataType}(32704)");
            return;
          }
          break;
      }
      base.BuildDataTypeFromDataType(type, forCreateTable);
    }

    protected override void BuildDeleteQuery(SqlDeleteStatement deleteStatement) {
      if (deleteStatement.With != null) {
        throw new NotSupportedException("iSeries doesn't support Cte in Delete statement");
      }
      base.BuildDeleteQuery(deleteStatement);
    }

    protected override void BuildEmptyInsert(SqlInsertClause insertClause) {
      StringBuilder.Append("VALUES");
      foreach (var field in insertClause.Into!.Fields) {
        _ = field;
        StringBuilder.Append("(DEFAULT)");
      }
      StringBuilder.AppendLine();
    }

    protected override void BuildFromClause(SqlStatement statement, SelectQuery selectQuery) {
      if (!statement.IsUpdate()) {
        base.BuildFromClause(statement, selectQuery);
      }
    }

    //protected override void BuildFunction(SqlFunction func) => base.BuildFunction(ConvertFunctionParameters(func));

    protected override void BuildInsertOrUpdateQuery(SqlInsertOrUpdateStatement insertOrUpdate) => BuildInsertOrUpdateQueryAsMerge(insertOrUpdate, "FROM " + dataProvider.Options.NamingConvention.DummyTableWithSchema() + " FETCH FIRST 1 ROW ONLY");

    //protected override void BuildInsertOrUpdateQueryAsMerge(SqlInsertOrUpdateStatement insertOrUpdate, string? fromDummyTable) {
    //  var table = insertOrUpdate.Insert.Into;
    //  var targetAlias = Convert(new StringBuilder(), insertOrUpdate.SelectQuery!.From.Tables[0].Alias, ConvertType.NameToQueryTableAlias).ToString();
    //  var sourceAlias = Convert(new StringBuilder(), GetTempAliases(1, "s")[0], ConvertType.NameToQueryTableAlias).ToString();
    //  var keys = insertOrUpdate.Update.Keys;
    //  AppendIndent().Append("MERGE INTO ");
    //  BuildPhysicalTable(table, null);
    //  StringBuilder.Append(' ').AppendLine(targetAlias);
    //  AppendIndent().Append("USING (SELECT ");
    //  ExtractMergeParametersIfCannotCombine(insertOrUpdate, keys);
    //  for (var j = 0; j < keys.Count; j++) {
    //    var key = keys[j];
    //    var expr = key.Expression;
    //    if (MergeSourceValueTypeRequired(expr)) {
    //      BuildTypedExpression(SqlDataType.GetDataType(expr.SystemType), expr);
    //    } else {
    //      BuildExpression(expr, buildTableName: false, checkParentheses: false);
    //    }
    //    StringBuilder.Append(" AS ");
    //    BuildExpression(key.Column, buildTableName: false, checkParentheses: false);
    //    if (j + 1 < keys.Count) {
    //      StringBuilder.Append(", ");
    //    }
    //  }
    //  if (!string.IsNullOrEmpty(fromDummyTable)) {
    //    StringBuilder.Append(' ').Append(fromDummyTable);
    //  }
    //  StringBuilder.Append(") ").Append(sourceAlias).AppendLine(" ON");
    //  AppendIndent().AppendLine("(");
    //  Indent++;
    //  for (var i = 0; i < keys.Count; i++) {
    //    var key2 = keys[i];
    //    AppendIndent();
    //    StringBuilder.Append(targetAlias).Append('.');
    //    BuildExpression(key2.Column, buildTableName: false, checkParentheses: false);
    //    StringBuilder.Append(" = ").Append(sourceAlias).Append('.');
    //    BuildExpression(key2.Column, buildTableName: false, checkParentheses: false);
    //    if (i + 1 < keys.Count) {
    //      StringBuilder.Append(" AND");
    //    }
    //    StringBuilder.AppendLine();
    //  }
    //  Indent--;
    //  AppendIndent().AppendLine(")");
    //  if (insertOrUpdate.Update.Items.Any()) {
    //    AppendIndent().AppendLine("WHEN MATCHED THEN");
    //    Indent++;
    //    AppendIndent().AppendLine("UPDATE ");
    //    BuildUpdateSet(insertOrUpdate.SelectQuery, insertOrUpdate.Update);
    //    Indent--;
    //  }
    //  AppendIndent().AppendLine("WHEN NOT MATCHED THEN");
    //  Indent++;
    //  BuildInsertClause(insertOrUpdate, insertOrUpdate.Insert, "INSERT", appendTableName: false, addAlias: false);
    //  Indent--;
    //  while (EndLine.Contains(StringBuilder[StringBuilder.Length - 1])) {
    //    StringBuilder.Length--;
    //  }
    //}

    protected override void BuildInsertQuery(SqlStatement statement, SqlInsertClause insertClause, bool addAlias) {
      BuildStep = Step.InsertClause;
      BuildInsertClause(statement, insertClause, addAlias);
      BuildStep = Step.WithClause;
      BuildWithClause(statement.GetWithClause());
      if (statement.QueryType == QueryType.Insert && statement.SelectQuery!.From.Tables.Count != 0) {
        BuildStep = Step.SelectClause;
        BuildSelectClause(statement.SelectQuery);
        BuildStep = Step.FromClause;
        BuildFromClause(statement, statement.SelectQuery);
        BuildStep = Step.WhereClause;
        BuildWhereClause(statement.SelectQuery);
        BuildStep = Step.GroupByClause;
        BuildGroupByClause(statement.SelectQuery);
        BuildStep = Step.HavingClause;
        BuildHavingClause(statement.SelectQuery);
        BuildStep = Step.OrderByClause;
        BuildOrderByClause(statement.SelectQuery);
        BuildStep = Step.OffsetLimit;
        BuildOffsetLimit(statement.SelectQuery);
      }
      if (insertClause.WithIdentity) {
        BuildGetIdentity(insertClause);
      }
    }

    protected override void BuildSelectClause(SelectQuery selectQuery) {
      if (selectQuery.HasSetOperators) {
        var topquery = selectQuery;
        while (topquery.ParentSelect != null && topquery.ParentSelect!.HasSetOperators) {
          topquery = topquery.ParentSelect;
        }
        var alias = selectQuery.Select.Columns.Select((SqlColumn c) => c.Alias).ToArray();
        selectQuery.SetOperators.ForEach(delegate (SqlSetOperator u) {
          var colNo = 0;
          u.SelectQuery.Select.Columns.ForEach(delegate (SqlColumn c) {
            c.Alias = alias[colNo];
            colNo++;
          });
        });
      }
      if (selectQuery.From.Tables.Count == 0) {
        AppendIndent().AppendLine("SELECT");
        BuildColumns(selectQuery);
        AppendIndent().AppendLine("FROM " + dataProvider.Options.NamingConvention.DummyTableWithSchema() + " FETCH FIRST 1 ROW ONLY");
      } else {
        base.BuildSelectClause(selectQuery);
      }
    }

    protected override void BuildTruncateTableStatement(SqlTruncateTableStatement truncateTable) {
      if (dataProvider.Options.IsVersion7_2orLater) {
        var table = truncateTable.Table;
        AppendIndent();
        StringBuilder.Append("TRUNCATE TABLE ");
        BuildPhysicalTable(table, null);
        if (truncateTable.ResetIdentity) {
          StringBuilder.Append(" RESTART IDENTITY");
        }
      } else {
        base.BuildTruncateTableStatement(truncateTable);
      }
    }

    protected override void BuildUpdateQuery(SqlStatement statement, SelectQuery selectQuery, SqlUpdateClause updateClause) {
      if (statement.GetWithClause() != null) {
        throw new NotSupportedException("iSeries doesn't support Cte in Update statement");
      }
      base.BuildUpdateQuery(statement, selectQuery, updateClause);
    }

    protected override ISqlBuilder CreateSqlBuilder() => new xDB2iSeriesSqlBuilder<TDbConnection>(dataProvider, MappingSchema);

    protected override string? GetProviderTypeName(IDbDataParameter parameter) {
      var name = dataProvider.GetProviderTypeName(parameter);
      if (name != null) {
        return name;
      }
      return base.GetProviderTypeName(parameter);
    }

    protected override IEnumerable<SqlColumn> GetSelectedColumns(SelectQuery selectQuery)
      => NeedSkip(selectQuery.Select.TakeValue, selectQuery.Select.SkipValue) && selectQuery.OrderBy.IsEmpty
      ? AlternativeGetSelectedColumns(selectQuery, () => base.GetSelectedColumns(selectQuery))
      : base.GetSelectedColumns(selectQuery);

    protected override string? LimitFormat(SelectQuery selectQuery) => "FETCH FIRST {0} ROWS ONLY";

    protected override bool MergeSourceValueTypeRequired(SqlValuesTable source, IReadOnlyList<ISqlExpression[]> rows, int row, int column) {
      if (row == -1) {
        return true;
      }
      var expr = rows[row][column];
      if (MergeSourceValueTypeRequired(expr)) {
        return true;
      }
      switch (row) {
        case -1:
          return true;
        default:
          return false;
        case 0:
          return rows.All(delegate (ISqlExpression[] r) {
            var sqlValue = r[column] as SqlValue;
            return sqlValue != null && (sqlValue.Value == null || ((sqlValue as INullable)?.IsNull ?? false));
          });
      }
    }

    protected override string? OffsetFormat(SelectQuery selectQuery) {
      if (!dataProvider.Options.IsVersion7_2orLater) {
        return base.OffsetFormat(selectQuery);
      }
      return "OFFSET {0} ROWS";
    }

    private bool MergeSourceValueTypeRequired(ISqlExpression expression) {
      if (!(expression is SqlParameter)) {
        var value = expression as SqlValue;
        if (value != null) {
          return value.Value == null;
        }
        return false;
      }
      return true;
    }
  }

}

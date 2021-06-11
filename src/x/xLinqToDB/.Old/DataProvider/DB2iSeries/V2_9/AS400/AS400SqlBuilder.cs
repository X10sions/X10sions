using Common;
using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  [IsCustom(IsCustomReason.ThirdPartyExtension)]
  public class AS400SqlBuilder : BasicSqlBuilder {
    private SqlField _identityField;
    //public static DB2IdentifierQuoteMode IdentifierQuoteMode = DB2IdentifierQuoteMode.Auto;
    //protected abstract DB2Version Version { get; }

    public AS400SqlBuilder(ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags, ValueToSqlConverter valueToSqlConverter)
      : base(sqlOptimizer, sqlProviderFlags, valueToSqlConverter) {
    }

    protected override ISqlBuilder CreateSqlBuilder() => new AS400SqlBuilder(SqlOptimizer, SqlProviderFlags, ValueToSqlConverter);

    public override int CommandCount(SqlStatement statement) {
      SqlTruncateTableStatement sqlTruncateTableStatement;
      if ((sqlTruncateTableStatement = (statement as SqlTruncateTableStatement)) != null) {
        if (!sqlTruncateTableStatement.ResetIdentity) {
          return 1;
        }
        return 1 + sqlTruncateTableStatement.Table.Fields.Values.Count((SqlField f) => f.IsIdentity);
      }
      SqlInsertStatement sqlInsertStatement;
      if ((sqlInsertStatement = (statement as SqlInsertStatement)) != null && sqlInsertStatement.Insert.WithIdentity) {
        _identityField = sqlInsertStatement.Insert.Into.GetIdentityField();
        if (_identityField == null) {
          return 2;
        }
      }
      return 1;
    }

    protected override void BuildCommand(SqlStatement statement, int commandNumber) {
      SqlTruncateTableStatement sqlTruncateTableStatement;
      if ((sqlTruncateTableStatement = (statement as SqlTruncateTableStatement)) != null) {
        var sqlField = sqlTruncateTableStatement.Table.Fields.Values.Skip(commandNumber - 1).First((SqlField f) => f.IsIdentity);
        base.StringBuilder.Append("ALTER TABLE ");
        ConvertTableName(base.StringBuilder, sqlTruncateTableStatement.Table.Database, sqlTruncateTableStatement.Table.Schema, sqlTruncateTableStatement.Table.PhysicalName);
        base.StringBuilder.Append(" ALTER ").Append(Convert(sqlField.PhysicalName, ConvertType.NameToQueryField)).AppendLine(" RESTART WITH 1");
      } else {
        base.StringBuilder.AppendLine($"SELECT {iDB2Constants.IdentityColumnSql} FROM {iDB2Constants.DummyTableName()}");
      }
    }

    protected override void BuildTruncateTableStatement(SqlTruncateTableStatement truncateTable) {
      var table = truncateTable.Table;
      base.AppendIndent();
      base.StringBuilder.Append("TRUNCATE TABLE ");
      base.BuildPhysicalTable(table, null);
      base.StringBuilder.Append(" IMMEDIATE");
      base.StringBuilder.AppendLine();
    }

    protected override void BuildSql(int commandNumber, SqlStatement statement, StringBuilder sb, OptimizationContext optimizationContext, int indent, bool skipAlias) {
      base.Statement = statement;
      base.StringBuilder = sb;
      base.Indent = indent;
      base.SkipAlias = skipAlias;
      if (_identityField != null) {
        indent += 2;
        base.AppendIndent().AppendLine("SELECT");
        base.AppendIndent().Append("\t");
        base.BuildExpression(_identityField, false, true, true);
        sb.AppendLine();
        base.AppendIndent().AppendLine("FROM");
        base.AppendIndent().AppendLine("\tNEW TABLE");
        base.AppendIndent().AppendLine("\t(");
      }
      base.BuildSql(commandNumber, statement, sb, optimizationContext, indent, skipAlias);
      if (_identityField != null) {
        sb.AppendLine("\t)");
      }
    }

    protected override void BuildGetIdentity(SqlInsertClause insertClause) {
      //if (Version == DB2Version.zOS) {
      //  base.StringBuilder.AppendLine(";").AppendLine($"SELECT {AS400Tools.IdentityColumnSql} FROM {AS400Tools.DummyTableName()}");
      //}
    }

    protected override void BuildSql() => base.AlternativeBuildSql(false, base.BuildSql, "\t0");

    protected override void BuildSelectClause(SelectQuery selectQuery) {
      if (selectQuery.From.Tables.Count == 0) {
        base.AppendIndent().AppendLine("SELECT");
        BuildColumns(selectQuery);
        base.AppendIndent().AppendLine($"FROM {DB2iSeriesConstants.DummyTableName} FETCH FIRST 1 ROW ONLY");
      } else {
        base.BuildSelectClause(selectQuery);
      }
    }

    protected override string LimitFormat(SelectQuery selectQuery) {
      if (selectQuery.Select.SkipValue != null) {
        return null;
      }
      return "FETCH FIRST {0} ROWS ONLY";
    }

    protected override void BuildFunction(SqlFunction func) {
      func = base.ConvertFunctionParameters(func);
      base.BuildFunction(func);
    }

    protected override void BuildFromClause(SqlStatement statement, SelectQuery selectQuery) {
      if (!statement.IsUpdate()) {
        base.BuildFromClause(statement, selectQuery);
      }
    }

    protected override void BuildColumnExpression(SelectQuery selectQuery, ISqlExpression expr, string alias, ref bool addAlias) {
      var flag = false;
      if (expr.SystemType == typeof(bool)) {
        SqlExpression sqlExpression;
        flag = (expr is SqlSearchCondition || ((sqlExpression = (expr as SqlExpression)) != null && sqlExpression.Expr == "{0}" && sqlExpression.Parameters.Length == 1 && sqlExpression.Parameters[0] is SqlSearchCondition));
      }
      if (flag) {
        base.StringBuilder.Append("CASE WHEN ");
      }
      base.BuildColumnExpression(selectQuery, expr, alias, ref addAlias);
      if (flag) {
        base.StringBuilder.Append(" THEN 1 ELSE 0 END");
      }
    }

    protected override void BuildDataTypeFromDataType(SqlDataType type, bool forCreateTable)  {
      switch (type.Type.DataType) {
        case DataType.DateTime:
          base.StringBuilder.Append("timestamp");
          break;
        case DataType.DateTime2:
          base.StringBuilder.Append("timestamp");
          break;
        default:
          base.BuildDataType(type, forCreateTable);
          break;
      }
    }

    public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType)  {
      switch (convertType) {
        case ConvertType.NameToQueryParameter:
          sb.Append("@" + value);
          break;
        case ConvertType.NameToCommandParameter:
        case ConvertType.NameToSprocParameter:
          sb.Append(":" + value);
          break;
        case ConvertType.SprocParameterToName: {
            if (value == null) {
              break;
            }
            var text2 = value.ToString();
            if (text2.Length > 0 && text2[0] == ':') {
              sb.Append(text2.Substring(1));
            }
            sb.Append(text2);
          }
          break;
        case ConvertType.NameToQueryField:
        case ConvertType.NameToQueryFieldAlias:
        case ConvertType.NameToQueryTable:
        case ConvertType.NameToQueryTableAlias: {
            if (value != null) {
              var name = value.ToString().Replace("\"", "");
            }
            break;
          }
      }
      return sb;
    }

    protected override void BuildInsertOrUpdateQuery(SqlInsertOrUpdateStatement insertOrUpdate) => BuildInsertOrUpdateQueryAsMerge(insertOrUpdate, $"FROM {iDB2Constants.DummyTableName()} FETCH FIRST 1 ROW ONLY");

    protected override void BuildEmptyInsert(SqlInsertClause insertClause) {
      StringBuilder.Append("VALUES ");
      foreach (var field in insertClause.Into.Fields) {
        StringBuilder.Append("(DEFAULT)");
      }
      StringBuilder.AppendLine();
    }

    protected override void BuildCreateTableIdentityAttribute1(SqlField field) => StringBuilder.Append("GENERATED ALWAYS AS IDENTITY");

    public override StringBuilder BuildTableName(StringBuilder sb, string server, string database, string schema, string table, TableOptions tableOptions)  {
      if (database != null && database.Length == 0) { database = null; }
      if (schema != null && schema.Length == 0) { schema = null; }
      if (database != null && schema == null) {
        throw new LinqToDBException($"{nameof(AS400SqlBuilder)} requires schema name if database name provided.");
      }
      return base.BuildTableName(sb, server, database, schema, table, tableOptions);
    }

    protected override string GetProviderTypeName(IDbDataParameter parameter) {
      if (parameter.DbType == DbType.Decimal && parameter.Value is decimal) {
        var d = new SqlDecimal((decimal)parameter.Value);
        return "(" + d.Precision + "," + d.Scale + ")";
      }
      dynamic p = parameter;
      return p.iDB2DbType.ToString();
    }

  }
}
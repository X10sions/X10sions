using LinqToDB.SqlProvider;
using System.Data;
using System.Data.OleDb;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public class AccessSqlBuilder_OleDb : AccessSqlBuilder_Base {

    public AccessSqlBuilder_OleDb(ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags, ValueToSqlConverter valueToSqlConverter)
      : base(sqlOptimizer, sqlProviderFlags, valueToSqlConverter) {
    }

    protected override ISqlBuilder CreateSqlBuilder() => new AccessSqlBuilder_OleDb(SqlOptimizer, SqlProviderFlags, ValueToSqlConverter);

    protected override string GetProviderTypeName(IDbDataParameter parameter) => ((OleDbParameter)parameter).OleDbType.ToString();

  }
}
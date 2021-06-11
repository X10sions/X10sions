using LinqToDB.SqlProvider;
using System.Data;
using System.Data.Odbc;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public class AccessSqlBuilder_Odbc : AccessSqlBuilder_Base {

    public AccessSqlBuilder_Odbc(ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags, ValueToSqlConverter valueToSqlConverter)
      : base(sqlOptimizer, sqlProviderFlags, valueToSqlConverter) {
    }

    protected override ISqlBuilder CreateSqlBuilder() => new AccessSqlBuilder_Odbc(SqlOptimizer, SqlProviderFlags, ValueToSqlConverter);

    protected override string GetProviderTypeName(IDbDataParameter parameter) => ((OdbcParameter)parameter).OdbcType.ToString();

    public override object Convert(object value, ConvertType convertType) {
      switch (convertType) {
        case ConvertType.NameToQueryParameter:
        case ConvertType.NameToCommandParameter:
        case ConvertType.NameToSprocParameter:
          // Use positional parameters rather than named parameters;
          return "?";
        default:
          return base.Convert(value, convertType);
      }
    }

  }
}
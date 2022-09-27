using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using System.Text;

namespace LinqToDB.DataProvider {
  public class GenericSqlBuilder : BasicSqlBuilder {
    public GenericSqlBuilder(IDataProvider? provider, DataSourceInformationRow dataSourceInformationRow, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags) {
      this.dataSourceInformationRow = dataSourceInformationRow;
    }

    DataSourceInformationRow dataSourceInformationRow;
    protected override ISqlBuilder CreateSqlBuilder() => new GenericSqlBuilder(DataProvider, dataSourceInformationRow, MappingSchema, SqlOptimizer, SqlProviderFlags);

    public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) {
      value = convertType switch {
        ConvertType.NameToQueryParameter => dataSourceInformationRow.UsesPositionalParameters() ? dataSourceInformationRow.ParameterMarker() : $"@{value}",
        _ => value
      };
      return base.Convert(sb, value, convertType);
    }

  }

}
using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using System.Text;

namespace LinqToDB.DataProvider {
  public class GenericSqlBuilder : BasicSqlBuilder {
    public GenericSqlBuilder(DataSourceInformationRow dataSourceInformationRow, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(mappingSchema, sqlOptimizer, sqlProviderFlags) {
      this.dataSourceInformationRow = dataSourceInformationRow;
    }

    DataSourceInformationRow dataSourceInformationRow;
    protected override ISqlBuilder CreateSqlBuilder() => new GenericSqlBuilder(dataSourceInformationRow, MappingSchema, SqlOptimizer, SqlProviderFlags);

    public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) {
      value = convertType switch {
        ConvertType.NameToQueryParameter =>  dataSourceInformationRow.UsesPositionalParameters ? dataSourceInformationRow.ParameterMarker : $"@{value}",
        _ => value
      };
      return base.Convert(sb, value, convertType);
    }

  }

}

namespace LinqToDB.DataProvider.DB2iSeries {

  public class DB2iSeriesV5R4SqlBuilder : BasicSqlBuilder {
    public DB2iSeriesV5R4SqlBuilder(MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(mappingSchema, sqlOptimizer, sqlProviderFlags) {
      //this.dataSourceInformationRow = dataSourceInformationRow;
    }

    //DataSourceInformationRow dataSourceInformationRow;
    protected override ISqlBuilder CreateSqlBuilder() => new DB2iSeriesV5R4SqlBuilder(MappingSchema, SqlOptimizer, SqlProviderFlags);
  }

  public class DB2iSeriesV7R4SqlBuilder : DB2iSeriesV5R4SqlBuilder {
    public DB2iSeriesV7R4SqlBuilder(MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(mappingSchema, sqlOptimizer, sqlProviderFlags) {
    }

    protected override ISqlBuilder CreateSqlBuilder() => new DB2iSeriesV7R4SqlBuilder(MappingSchema, SqlOptimizer, SqlProviderFlags);
  }

}
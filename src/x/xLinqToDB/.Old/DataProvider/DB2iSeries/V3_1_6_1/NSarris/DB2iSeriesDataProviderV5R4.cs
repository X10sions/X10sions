using LinqToDB.DataProvider.DB2iSeries;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.NSarris {
  public class DB2iSeriesDataProviderV5R4 : DB2iSeriesDataProvider {
    public DB2iSeriesDataProviderV5R4(DB2iSeriesProviderType providerType) : base($"DB2.iSeries.{providerType}.54", providerType, version: DB2iSeriesVersionRelease.V7_1) {
      sqlOptimizer = new DB2iSeriesSqlOptimizerV5R4(SqlProviderFlags);
    }
    DB2iSeriesSqlOptimizerV5R4 sqlOptimizer;

    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesSqlBuilderV5R4(this, mappingSchema, GetSqlOptimizer(), SqlProviderFlags) { };
    public override ISqlOptimizer GetSqlOptimizer() => sqlOptimizer;
  }

}

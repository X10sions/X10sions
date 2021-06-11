using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.NSarris {
  public class DB2iSeriesSqlBuilderV5R4 : xDB2iSeriesSqlBuilder {
    public DB2iSeriesSqlBuilderV5R4(DB2iSeriesDataProviderV5R4 provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
      : base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags) { }
  }

}

using IBM.Data.DB2.iSeries;
using LinqToDB.Configuration;
using LinqToDB.DataProvider;
using System.Collections.Generic;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB {
  public class DB2iSeriesFactory_TB : IDataProviderFactory {

    public IDataProvider GetDataProvider(IEnumerable<NamedValue> attributes) => new DB2iSeriesDataProvider_TB(
      DB2iSeriesVersionRelease.V5R4,
      DB2iSeriesNamingConvention.System,
      typeof(iDB2Connection),
      //typeof(iDB2DataReader),
      (connectionString) => new iDB2Connection(connectionString)
      );

  }
}

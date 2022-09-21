using LinqToDB.DataProvider.DB2iSeries;
using System.Data.Common;
using X10sions.Fake.Data.Enums;

namespace LinqToDB.DataProvider.X10sions {
  public class XDB2iSeriesDataProvider<TConn, TDataReader> : BaseDataProvider<TConn, TDataReader> where TConn : DbConnection, new() where TDataReader : DbDataReader {
    public XDB2iSeriesDataProvider(ConnectionStringName name, int id, DB2iSeriesProviderType providerType) : base(name.ToString(), id, DB2iSeriesTools.GetDataProvider(DB2iSeriesVersion.V7_1, providerType, GetMappingOptions(name, providerType))) { }

    static DB2iSeriesMappingOptions GetMappingOptions(ConnectionStringName name, DB2iSeriesProviderType providerType) => new DB2iSeriesMappingOptions(new DB2iSeriesProviderOptions(name.ToString(), providerType));

  }
}

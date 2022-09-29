using Common.Data;
using LinqToDB.DataProvider.DB2iSeries;
using System.Data.Common;

namespace LinqToDB.DataProvider.X10sions {
  public class DB2iSeriesWrappedDataProvider<TConn, TDataReader> : WrappedDataProvider<TConn, TDataReader> where TConn : DbConnection, new() where TDataReader : DbDataReader {
    public DB2iSeriesWrappedDataProvider(int id, DB2iSeriesProviderType providerType) : base(DbSystem.DB2iSeries, id, DB2iSeriesTools.GetDataProvider(DB2iSeriesVersion.V7_1, providerType, GetMappingOptions(providerType))) { }

    static DB2iSeriesMappingOptions GetMappingOptions(DB2iSeriesProviderType providerType) => new DB2iSeriesMappingOptions(new DB2iSeriesProviderOptions(GetProviderName(DbSystem.DB2iSeries), providerType));

  }
}

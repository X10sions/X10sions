using LinqToDB.DataProvider;
using System.Data;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public static class xDB2iSeriesProviderName {
    public const string Default = "DB2iSeries";

    public static string ForDynamicProviderAdapter<T>() where T : IDynamicProviderAdapter => "DB2iSeries_" + typeof(T).Name.Replace("DB2iSeries", "");

    public static string ForConnection<T>() where T : IDbConnection => "DB2iSeries_" + typeof(T).Name.Replace("DB2iSeries", "");
  }

}

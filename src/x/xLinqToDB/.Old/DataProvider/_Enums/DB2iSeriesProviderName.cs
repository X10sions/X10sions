using LinqToDB.DataProvider;
using System;
using System.Data;

namespace xLinqToDB.DataProvider.DB2iSeries {
  public static class DB2iSeriesProviderName {

    public const string Default = nameof(DB2iSeries);

    public static string ForDynamicProviderAdapter<T>() where T : IDynamicProviderAdapter => Default + "_" + typeof(T).Name.Replace(Default, "");

    public static string ForConnection<T>() where T : IDbConnection => Default + "_" + typeof(T).Name.Replace(Default, "");

  }
}
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace System.Data.Common {
  public static class DbConnectionExtensions {

    public static IDbConnection AsProfiledDbConnection(this DbConnection dbConnection, IDbProfiler dbProfiler = null) => new ProfiledDbConnection(dbConnection, dbProfiler ?? MiniProfiler.Current);

  }
}
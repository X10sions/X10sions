using System.Data.Common;

namespace StackExchange.Profiling.Data {
  public class ProfiledDbConnection<TConnection> : ProfiledDbConnection where TConnection : DbConnection, new() {

    public ProfiledDbConnection(TConnection connection, IDbProfiler? profiler = null) : base(connection, profiler ?? MiniProfiler.Current) { }

    public ProfiledDbConnection(string connectionString, IDbProfiler? profiler = null)
      : this(new TConnection { ConnectionString = connectionString }, profiler) { }

    //public TConnection UnWrappedTypedConnection => (TConnection)WrappedConnection;
  }
}
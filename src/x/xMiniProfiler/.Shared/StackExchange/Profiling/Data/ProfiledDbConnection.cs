using System.Data.Common;

namespace StackExchange.Profiling.Data {
  [System.ComponentModel.DesignerCategory("")]
  public class ProfiledDbConnection<TConnection> : ProfiledDbConnection where TConnection : DbConnection {

    public ProfiledDbConnection(TConnection connection, IDbProfiler profiler) : base(connection, profiler) {
      //   WrappedTypedConnection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public TConnection WrappedTypedConnection => (TConnection)WrappedConnection;

  }
}
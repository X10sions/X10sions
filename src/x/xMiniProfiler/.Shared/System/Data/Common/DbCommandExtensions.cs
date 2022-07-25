using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace System.Data.Common {
  public static class DbCommandExtensions {

    //public static ProfiledDbCommand AsProfiledDbCommand(this DbCommand dbCommand, IDbProfiler? dbProfiler = null) => dbCommand.AsProfiledDbCommand(dbCommand.Connection, dbProfiler);
    public static ProfiledDbCommand AsProfiledDbCommand(this DbCommand dbCommand, DbConnection dbConnection, IDbProfiler? dbProfiler = null) => new ProfiledDbCommand(dbCommand, dbConnection, dbProfiler ?? MiniProfiler.Current);

    public static DbCommand UnwrapCommand(this DbCommand command) => command is ProfiledDbCommand c ? c.InternalCommand : command;

  }
}
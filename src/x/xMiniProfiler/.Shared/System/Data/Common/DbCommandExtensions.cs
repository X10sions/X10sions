using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace System.Data.Common;
public static class DbCommandExtensions {
  public static ProfiledDbCommand AsProfiledDbCommand(this DbCommand dbCommand, DbConnection dbConnection, IDbProfiler? dbProfiler = null) => new ProfiledDbCommand(dbCommand, dbConnection, dbProfiler ?? MiniProfiler.Current);
  public static DbCommand UnwrapCommand(this DbCommand command) => command is ProfiledDbCommand c ? c.WrappedCommand : command;
}
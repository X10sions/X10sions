using System.Data.Common;

namespace StackExchange.Profiling.Data {
  public class ProfiledDbCommand<TCommand> : ProfiledDbCommand where TCommand : DbCommand, new() {
    public ProfiledDbCommand(TCommand command, IDbProfiler? profiler = null) : base(command, command.Connection, profiler ?? MiniProfiler.Current) { }
  }

}
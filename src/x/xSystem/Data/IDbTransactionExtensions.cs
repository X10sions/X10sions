using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data;
public static class IDbTransactionExtensions {

  public static IDbCommand CreateCommand(this IDbTransaction transaction, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) {
    using (var command = transaction.Connection.CreateCommand(commandText, parameters, commandType)) {
      command.Transaction = transaction;
      return command;
    }
  }

  public static IDbCommand CreateCommand(this IDbTransaction transaction, string commandText, KeyValuePair<string, object>[] parameters) {
    using (var cmd = transaction.Connection.CreateCommand(commandText, parameters)) {
      cmd.Transaction = transaction;
      return cmd;
    }
  }

  public static int ExecuteNonQuery(this IDbTransaction transaction, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) => transaction.CreateCommand(commandText, parameters, commandType).ExecuteNonQuery();
  public static IDataReader ExecuteReader(this IDbTransaction transaction, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) => transaction.CreateCommand(commandText, parameters, commandType).ExecuteReader();
  public static T ExecuteScalar<T>(this IDbTransaction transaction, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) => (T)transaction.CreateCommand(commandText, parameters, commandType).ExecuteScalar();

  public static Exception? TryCommit(this IDbTransaction transaction) {
    try {
      transaction.Commit();
      return null;
    } catch (Exception ex) {
      transaction.Rollback();
      return ex;
    }
  }

  public static Task<Exception?> TryCommitAsync(this IDbTransaction transaction, CancellationToken cancellationToken = default) => ((Func<Exception?>)(() => transaction.TryCommit())).Async(cancellationToken);

}
using System.Data.Common;

namespace Common.Data;

public abstract class BaseConnectionProvider<TConnection, TConnectionStringBuilder, TCommand, TDataReader>
  where TConnection : DbConnection, new()
  where TConnectionStringBuilder : DbConnectionStringBuilder, new()
  where TCommand : DbCommand, new()
  where TDataReader : DbDataReader {

  public BaseConnectionProvider(string connectionString) {
    Connection = GetConnection(connectionString);
    ConnectionStringBuilder = GetConnectionStringBuilder(connectionString);
  }

  public TCommand GetCommand(string commandText) => new TCommand {
    CommandText = commandText
  };

  public TConnection Connection { get; }
  public TConnectionStringBuilder ConnectionStringBuilder { get; }

  public int ExecuteNonQuery(string commandText) => GetCommand(commandText).ExecuteNonQuery();
  public TDataReader ExecuteReader(string commandText) => (TDataReader)GetCommand(commandText).ExecuteReader();
  public object ExecuteScalar(string commandText) => GetCommand(commandText).ExecuteScalar();
  public TConnection GetConnection(string connectionString) => new TConnection { ConnectionString = connectionString };
  public TConnectionStringBuilder GetConnectionStringBuilder(string connectionString) => new TConnectionStringBuilder { ConnectionString = connectionString };

}
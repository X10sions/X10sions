using System.Data.Common;

namespace Common.Data;

public interface IConnectionProvider<TConnection> : IDisposable where TConnection : DbConnection, new() {
  TConnection Connection { get; }
  string Namesapce { get; }
  TConnection GetConnection(string connectionString);
}

public interface IConnectionStringBuilderProvider<TConnectionStringBuilder> where TConnectionStringBuilder : DbConnectionStringBuilder, new() {
  TConnectionStringBuilder ConnectionStringBuilder { get; }
  TConnectionStringBuilder GetConnectionStringBuilder(string connectionString);
}

public interface IConnectionProvider<TConnection, TConnectionStringBuilder, TDataReader>
  : IConnectionProvider<TConnection>
  , IConnectionStringBuilderProvider<TConnectionStringBuilder>
  where TConnection : DbConnection, new()
  where TConnectionStringBuilder : DbConnectionStringBuilder, new()
  //where TCommand : DbCommand, new()
  where TDataReader : DbDataReader {
}


public abstract class BaseConnectionProvider<TConnection, TConnectionStringBuilder, TDataReader>
  : IDisposable
  , IConnectionProvider<TConnection,TConnectionStringBuilder, TDataReader>
  where TConnection : DbConnection, new()
  where TConnectionStringBuilder : DbConnectionStringBuilder, new()
  //where TCommand : DbCommand, new()
  where TDataReader : DbDataReader {

  public BaseConnectionProvider(string connectionString) {
    Connection = GetConnection(connectionString);
    ConnectionStringBuilder = GetConnectionStringBuilder(connectionString);
    Namesapce = typeof(TConnection).Name;
  }

  public string Namesapce { get; }
  public TConnection Connection { get; }
  public TConnectionStringBuilder ConnectionStringBuilder { get; }
  public TConnection GetConnection(string connectionString) => new TConnection { ConnectionString = connectionString };
  public TConnectionStringBuilder GetConnectionStringBuilder(string connectionString) => new TConnectionStringBuilder { ConnectionString = connectionString };

  //public TCommand GetCommand(string commandText) {
  //  var command = Connection.CreateCommand();
  //  command .CommandText=commandText;
  //  return command;
  //}

  //public int ExecuteNonQuery(string commandText) => GetCommand(commandText).ExecuteNonQuery();
  //public TDataReader ExecuteReader(string commandText) => (TDataReader)GetCommand(commandText).ExecuteReader();
  //public object ExecuteScalar(string commandText) => GetCommand(commandText).ExecuteScalar();

  public void Dispose() {
    Connection.Dispose();
  }
}
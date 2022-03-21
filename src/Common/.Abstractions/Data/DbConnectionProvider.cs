using System.Data;

namespace Common.Data;

public interface IDbConnectionProvider<TDbConnection> where TDbConnection : IDbConnection, new() {
  string ConnectionString { get; }
  TDbConnection GetDbConnection { get; }
}

public class DbConnectionProvider<TDbConnection> : IDbConnectionProvider<TDbConnection> where TDbConnection : IDbConnection, new() {
  public DbConnectionProvider(string connectionString) {
    ConnectionString = connectionString;
  }

  public string ConnectionString { get; }

  public TDbConnection GetDbConnection => new TDbConnection() { ConnectionString = ConnectionString };

}
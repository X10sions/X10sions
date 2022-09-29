using System.Data.Common;

namespace Common.Data;

public interface IDbConnectionProvider<TDbConnection> where TDbConnection : DbConnection, new() {
  string Namespace { get; }
  string GetConnectionString(string name);
  TDbConnection GetDbConnection(string connectionString);
}

public interface IDbConnectionProvider<TDbConnection, TConnectionStringNamesEnum> : IDbConnectionProvider<TDbConnection>
  where TDbConnection : DbConnection, new()
  where TConnectionStringNamesEnum : Enum {
  string GetConnectionString(TConnectionStringNamesEnum connectionNameEnum);
  TDbConnection GetDbConnection(TConnectionStringNamesEnum connectionNameEnum);
}

public class DbConnectionProvider<TDbConnection> : IDbConnectionProvider<TDbConnection> where TDbConnection : DbConnection, new() {
  public DbConnectionProvider(ConnectionStringsByProviderAppSettings providers) {
    Namespace = typeof(TDbConnection)?.Namespace ?? throw new NotImplementedException("No Namespace:" + typeof(TDbConnection).FullName);
    connectionStrings = providers[Namespace];
  }
  protected ConnectionStringsAppSettings connectionStrings;
  public string Namespace { get; }
  public string GetConnectionString(string name) => connectionStrings[name];
  public TDbConnection GetDbConnection(string connectionString) => new TDbConnection { ConnectionString = connectionString };
}

public class DbConnectionProvider<TDbConnection, TConnectionStringNamesEnum> : DbConnectionProvider<TDbConnection> , IDbConnectionProvider<TDbConnection, TConnectionStringNamesEnum>
  where TDbConnection : DbConnection, new()
  where TConnectionStringNamesEnum : Enum {
  public DbConnectionProvider(ConnectionStringsByProviderAppSettings providers):base(providers) {  }
  public string GetConnectionString(TConnectionStringNamesEnum connectionNameEnum) => GetConnectionString(connectionNameEnum.ToString());
  public TDbConnection GetDbConnection(TConnectionStringNamesEnum connectionNameEnum) => GetDbConnection(GetConnectionString(connectionNameEnum));
}

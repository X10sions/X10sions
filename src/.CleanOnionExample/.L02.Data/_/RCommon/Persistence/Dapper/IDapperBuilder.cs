using RCommon.Persistence.Sql;

namespace RCommon.Persistence.Dapper;

public interface IDapperBuilder : IPersistenceBuilder {
  IDapperBuilder AddDbConnection<TDbConnection>(string dataStoreName, Action<RDbConnectionOptions> options) where TDbConnection : IRDbConnection;
}
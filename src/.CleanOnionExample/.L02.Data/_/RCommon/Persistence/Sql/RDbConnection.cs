using Microsoft.Extensions.Options;
using RCommon.Entities;
using System.Data.Common;

namespace RCommon.Persistence.Sql;

public class RDbConnection : DisposableResource, IRDbConnection {
  private readonly IOptions<RDbConnectionOptions> _options;
  private readonly IEntityEventTracker _entityEventTracker;

  public RDbConnection(IOptions<RDbConnectionOptions> options) {
    _options = options ?? throw new ArgumentNullException(nameof(options));
  }

  public DbConnection GetDbConnection() {
    Guard.Against<RDbConnectionException>(this._options == null, "No options configured for this RDbConnection");
    Guard.Against<RDbConnectionException>(this._options.Value == null, "No options configured for this RDbConnection");
    Guard.Against<RDbConnectionException>(this._options.Value.DbFactory == null, "You must configured a DbProviderFactory for this RDbConnection");
    Guard.Against<RDbConnectionException>(this._options.Value.ConnectionString.IsNullOrEmpty(), "You must configure a conneciton string for this RDbConnection");

    var connection = this._options.Value.DbFactory.CreateConnection();
    connection.ConnectionString = this._options.Value.ConnectionString;

    return connection;
  }

}
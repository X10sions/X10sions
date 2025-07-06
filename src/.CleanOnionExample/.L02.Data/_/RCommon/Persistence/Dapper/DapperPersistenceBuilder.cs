using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RCommon.Persistence.Crud;
using RCommon.Persistence.Dapper.Crud;
using RCommon.Persistence.Sql;

namespace RCommon.Persistence.Dapper;

public class DapperPersistenceBuilder : IDapperBuilder {
  private readonly IServiceCollection _services;
  private List<string> _dbContextTypes = new List<string>();

  public DapperPersistenceBuilder(IServiceCollection services) {
    _services = services ?? throw new ArgumentNullException(nameof(services));

    // Dapper Repository
    services.AddTransient(typeof(ISqlMapperRepository<>), typeof(DapperRepository<>));
    services.AddTransient(typeof(IWriteOnlyRepository<>), typeof(DapperRepository<>));
    services.AddTransient(typeof(IReadOnlyRepository<>), typeof(DapperRepository<>));
  }

  public IDapperBuilder AddDbConnection<TDbConnection>(string dataStoreName, Action<RDbConnectionOptions> options) where TDbConnection : IRDbConnection {
    Guard.Against<UnsupportedDataStoreException>(dataStoreName.IsNullOrEmpty(), "You must set a name for the Data Store");
    Guard.Against<RDbConnectionException>(options == null, "You must configure the options for the RDbConnection for it to be useful");

    var dbContext = typeof(TDbConnection).AssemblyQualifiedName;

    _services.TryAddTransient<IDataStoreFactory, DataStoreFactory>();
    _services.TryAddTransient(Type.GetType(dbContext));
    _services.Configure<DataStoreFactoryOptions>(options => options.Register<TDbConnection>(dataStoreName));
    _services.Configure(options);

    return this;
  }

  public IPersistenceBuilder SetDefaultDataStore(Action<DefaultDataStoreOptions> options) {
    this._services.Configure(options);
    return this;
  }
}

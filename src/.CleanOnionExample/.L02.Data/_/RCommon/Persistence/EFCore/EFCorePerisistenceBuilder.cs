using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RCommon.Persistence.Crud;

namespace RCommon.Persistence.EFCore;

public class EFCorePerisistenceBuilder : IEFCorePersistenceBuilder {
  private readonly IServiceCollection _services;

  public EFCorePerisistenceBuilder(IServiceCollection services) {
    _services = services ?? throw new ArgumentNullException(nameof(services));

    // EF Core Repository
    services.AddTransient(typeof(IReadOnlyRepository<>), typeof(Crud.EFCoreRepository<>));
    services.AddTransient(typeof(IWriteOnlyRepository<>), typeof(Crud.EFCoreRepository<>));
    services.AddTransient(typeof(ILinqRepository<>), typeof(Crud.EFCoreRepository<>));
    services.AddTransient(typeof(IGraphRepository<>), typeof(Crud.EFCoreRepository<>));
  }

  public IEFCorePersistenceBuilder AddDbContext<TDbContext>(string dataStoreName, Action<DbContextOptionsBuilder>? options = null)
      where TDbContext : RCommonDbContext {
    Guard.Against<UnsupportedDataStoreException>(dataStoreName.IsNullOrEmpty(), "You must set a name for the Data Store");

    _services.TryAddTransient<IDataStoreFactory, DataStoreFactory>();
    _services.Configure<DataStoreFactoryOptions>(options => options.Register<TDbContext>(dataStoreName));
    _services.AddDbContext<TDbContext>(options, ServiceLifetime.Scoped);

    return this;
  }

  public IPersistenceBuilder SetDefaultDataStore(Action<DefaultDataStoreOptions> options) {
    _services.Configure(options);
    return this;
  }
}

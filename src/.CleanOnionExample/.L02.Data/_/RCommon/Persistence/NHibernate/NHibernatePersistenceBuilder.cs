using NHibernate;
using NHibernate.Cfg;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RCommon.Persistence.Crud;
using RCommon.Persistence.NHibernate.Crud;

namespace RCommon.Persistence.NHibernate;

public class NHibernatePersistenceBuilder : INHibernatePersistenceBuilder {
  private readonly IServiceCollection _services;

  public NHibernatePersistenceBuilder(IServiceCollection services) {
    _services = services ?? throw new ArgumentNullException(nameof(services));
    // NHibernate Repository
    services.AddTransient(typeof(IReadOnlyRepository<>), typeof(NHibernateRepository<>));
    services.AddTransient(typeof(IWriteOnlyRepository<>), typeof(NHibernateRepository<>));
    services.AddTransient(typeof(ILinqRepository<>), typeof(NHibernateRepository<>));
  }

  public INHibernatePersistenceBuilder AddDataConnection<TDataConnection>(string dataStoreName, Configuration options)
      where TDataConnection : RCommonNHContext{
    Guard.Against<UnsupportedDataStoreException>(dataStoreName.IsNullOrEmpty(), "You must set a name for the Data Store");
    Guard.Against<UnsupportedDataStoreException>(options == null, "You must set options to a value in order for them to be useful");

    _services.TryAddTransient<IDataStoreFactory, DataStoreFactory>();
    _services.Configure<DataStoreFactoryOptions>(options => options.Register<TDataConnection>(dataStoreName));
    _services.AddNHibernateSession(options);
    return this;
  }

  public IPersistenceBuilder SetDefaultDataStore(Action<DefaultDataStoreOptions> options) {
    _services.Configure(options);
    return this;
  }

}



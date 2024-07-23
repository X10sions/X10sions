﻿using LinqToDB;
using LinqToDB.AspNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RCommon.Persistence.Crud;
using RCommon.Persistence.Linq2Db.Crud;

namespace RCommon.Persistence.Linq2Db;

public class Linq2DbPersistenceBuilder : ILinq2DbPersistenceBuilder {

  private readonly IServiceCollection _services;

  public Linq2DbPersistenceBuilder(IServiceCollection services) {
    _services = services ?? throw new ArgumentNullException(nameof(services));

    // Linq2Db Repository
    services.AddTransient(typeof(IReadOnlyRepository<>), typeof(Linq2DbRepository<>));
    services.AddTransient(typeof(IWriteOnlyRepository<>), typeof(Linq2DbRepository<>));
    services.AddTransient(typeof(ILinqRepository<>), typeof(Linq2DbRepository<>));
  }

  public ILinq2DbPersistenceBuilder AddDataConnection<TDataConnection>(string dataStoreName, Func<IServiceProvider, DataOptions, DataOptions> options)
      where TDataConnection : RCommonDataConnection {
    Guard.Against<UnsupportedDataStoreException>(dataStoreName.IsNullOrEmpty(), "You must set a name for the Data Store");
    Guard.Against<UnsupportedDataStoreException>(options == null, "You must set options to a value in order for them to be useful");
    _services.TryAddTransient<IDataStoreFactory, DataStoreFactory>();
    _services.Configure<DataStoreFactoryOptions>(options => options.Register<TDataConnection>(dataStoreName));
    _services.AddLinqToDBContext<TDataConnection>(options);
    return this;
  }

  public IPersistenceBuilder SetDefaultDataStore(Action<DefaultDataStoreOptions> options) {
    _services.Configure(options);
    return this;
  }
}
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Design;

public sealed class DB2iSeriesDesignTimeServices : IDesignTimeServices {
  public void ConfigureDesignTimeServices(IServiceCollection services) {
    new EntityFrameworkRelationalServicesBuilder(services).TryAddCoreServices();
    services.AddSingleton<IDatabaseModelFactory, DB2iSeriesDatabaseModelFactory>();
    services.AddSingleton<IProviderConfigurationCodeGenerator, DB2iSeriesProviderCodeGenerator>();
  }
}
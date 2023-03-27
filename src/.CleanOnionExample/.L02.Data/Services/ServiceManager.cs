using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Services;

public sealed class ServiceManager : IServiceManager {
  private readonly Lazy<IAccountService2> _lazyAccountService2;
  private readonly Lazy<IOwnerService> _lazyOwnerService;
  private readonly Lazy<IPersonService> _lazyPersonService;
  private readonly Lazy<IWeatherForecastService> _lazyWeatherForecastService;

  public ServiceManager(IRepositoryManager repositoryManager) {
    _lazyAccountService2 = new Lazy<IAccountService2>(() => new AccountService2(repositoryManager));
    _lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));
    _lazyPersonService = new Lazy<IPersonService>(() => new PersonService(repositoryManager));
    _lazyWeatherForecastService = new Lazy<IWeatherForecastService>(() => new WeatherForecastService(repositoryManager));
  }

  public IAccountService2 AccountService2 => _lazyAccountService2.Value;
  public IOwnerService OwnerService => _lazyOwnerService.Value;
  public IPersonService PersonService => _lazyPersonService.Value;
  public IWeatherForecastService WeatherForecastService => _lazyWeatherForecastService.Value;

}
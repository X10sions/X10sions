using CleanOnionExample.Data.DbContexts;
using CleanOnionExample.Data.Entities.Services;
using Common.Features.DummyFakeExamples.Account;
using Common.Features.DummyFakeExamples.Owner;
using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities;

public sealed class RepositoryManager : IRepositoryManager {
  private readonly Lazy<IOwnerRepository> _lazyOwnerRepository;
  private readonly Lazy<IAccountRepository> _lazyAccountRepository;
  private readonly Lazy<PersonRepository> _lazyPersonRepository;
  private readonly Lazy<IWeatherForecastRepository> _lazyWeatherForecastApiRepository;
  private readonly Lazy<IWeatherForecastRepository> _lazyWeatherForecastStoreRepository;
  private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

  public RepositoryManager(RepositoryDbContext repositoryDbContext, ApplicationDbContext applicationDbContext, HttpClient httpClient, WeatherForecastDataStore weatherForecastStore) {
    _lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(repositoryDbContext));
    _lazyOwnerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(repositoryDbContext));
    _lazyPersonRepository = new Lazy<PersonRepository>(() => new PersonRepository(applicationDbContext));
    _lazyWeatherForecastApiRepository = new Lazy<IWeatherForecastRepository>(() => new WeatherForecastApiRepository(httpClient));
    _lazyWeatherForecastStoreRepository = new Lazy<IWeatherForecastRepository>(() => new WeatherForecastStoreRepository(weatherForecastStore));

    _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(repositoryDbContext));
  }

  public IOwnerRepository OwnerRepository => _lazyOwnerRepository.Value;
  public IAccountRepository AccountRepository => _lazyAccountRepository.Value;
  public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
  public PersonRepository PersonRepository => _lazyPersonRepository.Value;
  public IWeatherForecastRepository WeatherForecastApiRepository => _lazyWeatherForecastApiRepository.Value;
  public IWeatherForecastRepository WeatherForecastStoreRepository => _lazyWeatherForecastStoreRepository.Value;
}

using CleanOnionExample.Data.DbContexts;
using CleanOnionExample.Data.Entities.Services;
using Common.Features.DummyFakeExamples.Account;
using Common.Features.DummyFakeExamples.Owner;
using Common.Features.DummyFakeExamples.Person;
using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanOnionExample.Data.Entities;

public sealed class RepositoryManager : IRepositoryManager {
  private readonly Lazy<IOwnerRepository> _lazyOwnerRepository;
  private readonly Lazy<IAccountRepository> _lazyAccountRepository;
  private readonly Lazy<IPersonRepository> _lazyPersonRepository;
  private readonly Lazy<IWeatherForecastRepository> _lazyWeatherForecastApiRepository;
  private readonly Lazy<IWeatherForecastRepository> _lazyWeatherForecastStoreRepository;
  private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

  public RepositoryManager(RepositoryDbContext repositoryDbContext, ApplicationDbContext applicationDbContext, HttpClient httpClient, ILogger logger) {
    _lazyAccountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(repositoryDbContext));
    _lazyOwnerRepository = new Lazy<IOwnerRepository>(() => new OwnerRepository(repositoryDbContext));
    _lazyPersonRepository = new Lazy<IPersonRepository>(() => new PersonRepository(applicationDbContext));
    _lazyWeatherForecastApiRepository = new Lazy<IWeatherForecastRepository>(() => new WeatherForecastApiRepository(httpClient, logger));
    _lazyWeatherForecastStoreRepository = new Lazy<IWeatherForecastRepository>(() => new  WeatherForecastStoreRepository(new RandomDataStore(5)));

    _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(repositoryDbContext));
  }

  public IOwnerRepository OwnerRepository => _lazyOwnerRepository.Value;
  public IAccountRepository AccountRepository => _lazyAccountRepository.Value;
  public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
  public IPersonRepository PersonRepository => _lazyPersonRepository.Value;
  public IWeatherForecastRepository WeatherForecastApiRepository => _lazyWeatherForecastApiRepository.Value;
  public IWeatherForecastRepository WeatherForecastStoreRepository => _lazyWeatherForecastStoreRepository.Value;
}

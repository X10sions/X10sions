using Common.Features.DummyFakeExamples.Account;
using Common.Features.DummyFakeExamples.Owner;
using Common.Features.DummyFakeExamples.Person;
using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities.Services;
public interface IRepositoryManager {
  IAccountRepository AccountRepository { get; }
  IOwnerRepository OwnerRepository { get; }
  IPersonRepository PersonRepository { get; }
  IWeatherForecastRepository WeatherForecastApiRepository { get; }
  IWeatherForecastRepository WeatherForecastStoreRepository { get; }
  IUnitOfWork UnitOfWork { get; }
}

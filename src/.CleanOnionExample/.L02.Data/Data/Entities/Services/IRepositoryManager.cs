using Common.Features.DummyFakeExamples.Person;
using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.EntityFrameworkCore;
using X10sions.Fake.Features.Account;
using X10sions.Fake.Features.Owner;

namespace CleanOnionExample.Data.Entities.Services;
public interface IRepositoryManager {
  IAccountRepository AccountRepository { get; }
  IOwnerRepository OwnerRepository { get; }
  IPersonRepository PersonRepository { get; }
  IWeatherForecastRepository WeatherForecastApiRepository { get; }
  IWeatherForecastRepository WeatherForecastStoreRepository { get; }
  IUnitOfWork UnitOfWork { get; }
}

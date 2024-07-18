using Common.Domain;
using X10sions.Fake.Features.Account;
using X10sions.Fake.Features.Owner;
using X10sions.Fake.Features.Person;
using X10sions.Fake.Features.WeatherForecast;

namespace X10sions.Fake.Domain.Services;
public interface IRepositoryManager {
  IAccountRepository AccountRepository { get; }
  IOwnerRepository OwnerRepository { get; }
  IPersonRepository PersonRepository { get; }
  IWeatherForecastRepository WeatherForecastApiRepository { get; }
  IWeatherForecastRepository WeatherForecastStoreRepository { get; }
  //IUnitOfWork UnitOfWork { get; }
}

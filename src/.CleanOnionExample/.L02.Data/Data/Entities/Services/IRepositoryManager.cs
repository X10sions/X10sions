using Common.Data;

namespace CleanOnionExample.Data.Entities.Services;
public interface IRepositoryManager {
  IAccountRepository AccountRepository { get; }
  IOwnerRepository OwnerRepository { get; }
  IPersonRepository PersonRepository { get; }
  IWeatherForecastRepository WeatherForecastApiRepository { get; }
  IWeatherForecastRepository WeatherForecastStoreRepository { get; }
  IUnitOfWork UnitOfWork { get; }
}

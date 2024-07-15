using X10sions.Fake.Features.Person;
using X10sions.Fake.Features.WeatherForecast;

namespace X10sions.Fake.Domain.Services;

public interface IServiceManager {
  //IOwnerService OwnerService { get; }
  //IAccountService2 AccountService2 { get; }
  IPersonService PersonService { get; }
  IWeatherForecastService WeatherForecastService { get; }
}

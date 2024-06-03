using Common.Features.DummyFakeExamples.Person;
using Common.Features.DummyFakeExamples.WeatherForecast;

namespace Common.Features.DummyFakeExamples;

public interface IServiceManager {
  //IOwnerService OwnerService { get; }
  //IAccountService2 AccountService2 { get; }
  IPersonService PersonService { get; }
  IWeatherForecastService WeatherForecastService { get; }
}

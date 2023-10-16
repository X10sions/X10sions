namespace CleanOnionExample.Data.Entities;

public interface IServiceManager {
  //IOwnerService OwnerService { get; }
  //IAccountService2 AccountService2 { get; }
  IPersonService PersonService { get; }
  IWeatherForecastService WeatherForecastService { get; }
}

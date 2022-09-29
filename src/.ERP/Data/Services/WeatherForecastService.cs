using X10sions.ERP.Data.Models;

namespace X10sions.ERP.Data.Services {
  public class WeatherForecastService {
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) {
      var summaries = Enum.GetNames<WeatherForecastSummary>();

      return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast {
        Date = startDate.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Enum.Parse<WeatherForecastSummary>(summaries[Random.Shared.Next(summaries.Length)])
      }).ToArray());
    }

  }
}
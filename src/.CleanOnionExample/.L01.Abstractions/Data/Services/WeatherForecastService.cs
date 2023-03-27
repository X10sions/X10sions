namespace CleanOnionExample.Data.Entities.Services {
  public class WeatherForecastService {
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate) {
      return Task.FromResult(Enumerable.Range(1, 5).Select(index => WeatherForecast.GetRandom(index, startDate)).ToArray());
    }
  }
}
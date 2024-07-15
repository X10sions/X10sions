namespace X10sions.Fake.Features.WeatherForecast;
public class WeatherForecastService {
  private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

  public Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate) => Task.FromResult(Enumerable.Range(1, 5).Select(index => WeatherForecast.GetRandom(index, startDate)).ToArray());

}

using Common.Collections.Paged;

namespace X10sions.Fake.Features.WeatherForecast;
public class WeatherForecastService:IWeatherForecastService {
  private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

  public ValueTask<bool> AddAsync(WeatherForecast.Update.Command weatherForecast, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate) => Task.FromResult(Enumerable.Range(1, 5).Select(index => WeatherForecast.GetRandom(index, startDate)).ToArray());
  public ValueTask<List<WeatherForecast>> GetListAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<List<WeatherForecast>> GetPageAsync(PagedListOptions listOptions, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}

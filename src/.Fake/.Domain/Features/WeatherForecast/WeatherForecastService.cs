using Common.Collections.Paged;

namespace X10sions.Fake.Features.WeatherForecast;
public class WeatherForecastService:IWeatherForecastService {
  private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

  public ValueTask<bool> AddAsync(FakeWeatherForecast.Update.Command weatherForecast, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<FakeWeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<FakeWeatherForecast[]> GetForecastAsync(DateOnly startDate) => Task.FromResult(Enumerable.Range(1, 5).Select(index => FakeWeatherForecast.GetRandom(index, startDate)).ToArray());
  public ValueTask<List<FakeWeatherForecast>> GetListAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public ValueTask<List<FakeWeatherForecast>> GetPageAsync(PagedListOptions listOptions, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}

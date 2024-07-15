using Common.Collections.Paged;

namespace X10sions.Fake.Features.WeatherForecast;

public interface IWeatherForecastRepository {
  ValueTask<bool> AddAsync(WeatherForecast record, CancellationToken cancellationToken = default);
  ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<int> GetCountAsync(CancellationToken cancellationToken = default);
  ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<List<WeatherForecast>> GetListAsync(PagedListOptions options, CancellationToken cancellationToken = default);
}


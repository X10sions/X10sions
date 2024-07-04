using Common.Features.DummyFakeExamples.WeatherForecast;
using Common.Collections.Paged;

namespace CleanOnionExample.Data.Entities;

public class WeatherForecastStoreRepository(RandomDataStore dataStore) : IWeatherForecastRepository {
  public ValueTask<bool> AddAsync(WeatherForecast record, CancellationToken cancellationToken = default)  => dataStore.Insert(record);
  public ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)  => dataStore.Delete(id);
  public ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)  => dataStore.GetById(id);  
  public ValueTask<int> GetCountAsync(CancellationToken cancellationToken = default)  => dataStore.GetRecordCount();
  public ValueTask<List<WeatherForecast>> GetListAsync(PagedListOptions options, CancellationToken cancellationToken = default) => dataStore.GetPagedList(0, int.MaxValue);
}

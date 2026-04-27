using Common.Collections.Paged;

namespace X10sions.Fake.Features.WeatherForecast;

public interface IWeatherForecastService {
  //#region NotificationService
  //event EventHandler<RecordSetChangedEventArgs>? RecordSetChanged;
  //event EventHandler<RecordChangedEventArgs>? RecordChanged;
  //void NotifyRecordSetChanged(object? sender, RecordSetChangedEventArgs e);
  //void NotifyRecordChanged(object? sender, RecordChangedEventArgs e);
  //#endregion
  ValueTask<bool> AddAsync(FakeWeatherForecast.Update.Command weatherForecast, CancellationToken cancellationToken = default);
  ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<FakeWeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<List<FakeWeatherForecast>> GetListAsync(CancellationToken cancellationToken = default);
  ValueTask<List<FakeWeatherForecast>> GetPageAsync(PagedListOptions listOptions, CancellationToken cancellationToken = default);
}


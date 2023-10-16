using Common.Collections.Paged;

namespace Common.Features.DummyFakeExamples.WeatherForecast;

public interface IWeatherForecastService {
  //#region NotificationService
  //event EventHandler<RecordSetChangedEventArgs>? RecordSetChanged;
  //event EventHandler<RecordChangedEventArgs>? RecordChanged;
  //void NotifyRecordSetChanged(object? sender, RecordSetChangedEventArgs e);
  //void NotifyRecordChanged(object? sender, RecordChangedEventArgs e);
  //#endregion
  ValueTask<bool> AddAsync(WeatherForecast.Update.Command weatherForecast, CancellationToken cancellationToken = default);
  ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<List<WeatherForecast>> GetListAsync(CancellationToken cancellationToken = default);
  ValueTask<List<WeatherForecast>> GetPageAsync(PagedListOptions listOptions, CancellationToken cancellationToken = default);
}


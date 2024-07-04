using Common.Data;
using Common.Domain.Entities;
using Common.Data.Events;
using System.Linq.Expressions;
using Common.Data.Repositories;

namespace Common.Features.DummyFakeExamples.WeatherForecast;

public partial class WeatherForecast : EntityBase<Guid> {
  public const decimal CelsiusToFahrenheitScale = 5m / 9m;
  public DateOnly Date { get; set; }
  public int TemperatureC { get; set; }
  public int TemperatureF => 32 + (int)(TemperatureC / CelsiusToFahrenheitScale);
  public WeatherForecastSummary Summary { get; init; }
  public string? FakeNeverUsed { get; init; }
}

public partial class WeatherForecast {
  public static WeatherForecast GetRandom(int addDays, DateOnly? startDate = null) => new WeatherForecast {
    Id = Guid.NewGuid(),
    Date = (startDate ?? DateOnly.FromDateTime(DateTime.Now)).AddDays(addDays),
    TemperatureC = Random.Shared.Next(-20, 55),
    Summary = Enum.GetValues<WeatherForecastSummary>().GetRandom()
  };

  public static Task<WeatherForecast[]> GetRandomListAsync(int count, DateOnly? startDate = null)
    => Task.FromResult(Enumerable.Range(1, count).Select(index => GetRandom(index, startDate)).ToArray());

  public static Expression<Func<WeatherForecast, bool>> GetById(Guid id) => x => x.Id == id;

  public class Update {
    public record Command(Guid Id, DateTime Date, int TemperatureC, WeatherForecastSummary? Summary);
  }

  //public async ValueTask<bool> AddAsync(WeatherForecast.Update.Command record, CancellationToken cancellationToken = default) {
  //  await repository.InsertAsync(record.Adapt<WeatherForecast>(), cancellationToken);
  //  await GetByIdAsync(record.Id);
  //  NotifyRecordSetChanged();
  //  return true;
  //}

  //public async ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
  //  var record = await repository.GetByIdAsync(id, cancellationToken);
  //  NotifyRecordChanged(record);
  //  return record;
  //}

  //public async ValueTask<PagedList<WeatherForecast>> GetListAsync(CancellationToken cancellationToken = default) {
  //  var records = await repository.Query.ToPagedListAsync(0, 20, cancellationToken);
  //  NotifyRecordSetChanged();
  //  return records;
  //}

  #region NotificationService

  public event EventHandler<RecordSetChangedEventArgs>? RecordSetChanged;
  public event EventHandler<RecordChangedEventArgs>? RecordChanged;
  public void NotifyRecordSetChanged(object? sender, RecordSetChangedEventArgs e) => RecordSetChanged?.Invoke(this, e);
  public void NotifyRecordChanged(object? sender, RecordChangedEventArgs e) => RecordChanged?.Invoke(this, e);

  protected void NotifyRecordChanged(WeatherForecast record) => NotifyRecordChanged(this, RecordChangedEventArgs.Create(record.Id));
  protected void NotifyRecordSetChanged() => NotifyRecordSetChanged(this, RecordSetChangedEventArgs.Create<WeatherForecast>());

  #endregion
}


public static class WeatherForecastExtensions {
  public static async Task<WeatherForecast?> GetByIdAsync(this IReadRepository<WeatherForecast> repository, Guid id, CancellationToken token = default)
    => await repository.Query.FirstOrDefaultAsync(WeatherForecast.GetById(id), token);
}

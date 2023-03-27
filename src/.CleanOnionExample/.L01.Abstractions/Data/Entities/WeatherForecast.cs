using CleanOnionExample.Events;
using Common.Collections.Paged;
using Common.Data;
using System.Linq.Expressions;

namespace CleanOnionExample.Data.Entities;

public partial class WeatherForecast : BaseEntity<Guid> {
  public DateOnly Date { get; set; }
  public int TemperatureC { get; set; }
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
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


public interface IWeatherForecastRepository {
  ValueTask<bool> AddAsync(WeatherForecast record, CancellationToken cancellationToken = default);
  ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<int> GetCountAsync(CancellationToken cancellationToken = default);
  ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
  ValueTask<List<WeatherForecast>> GetListAsync(PagedListOptions options, CancellationToken cancellationToken = default);
}

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



//public class GetRandomWeatherForecast {
//  public record Query(int addDays) : IRequest<WeatherForecast> {
//    public class Handler : IRequestHandler<Query, WeatherForecast> {
//      public Task<WeatherForecast> Handle(Query query, CancellationToken cancellationToken)
//        => System.Threading.Tasks.Task.FromResult(WeatherForecast.GetRandom(query.addDays));
//    }

//  }

//  public record Event(int addDays) : INotification {

//    public class Handler : INotificationHandler<Event> {
//      public Handler(IMediator mediator) {
//        this.mediator = mediator;
//      }
//      IMediator mediator;

//      public async System.Threading.Tasks.Task Handle(Event notification, CancellationToken cancellationToken) {
//        await mediator.Send(new Query(notification.addDays));
//      }
//    }

//  }

//}

//public class GetRandomWeatherForecasts {
//  public record Query : IRequest<IEnumerable<WeatherForecast>> {
//    public class Handler : IRequestHandler<Query, IEnumerable<WeatherForecast>> {
//      public async Task<IEnumerable<WeatherForecast>> Handle(Query request, CancellationToken cancellationToken) {
//        var vm = Enumerable.Range(1, 5).Select(index => WeatherForecast.GetRandom(index, DateTime.Now));
//        return await System.Threading.Tasks.Task.FromResult(vm);
//      }
//    }

//  }
//}

//public record xGetWeatherForecast(Guid Id, DateTime Date, int TemperatureC, WeatherForecastSummary? Summary);

//public class WeatherForecastService : IWeatherForecastService {
//  public WeatherForecastService(IRepositoryManager repositoryManager) {
//    _repositoryManager = repositoryManager;
//  }

//  private readonly IRepositoryManager _repositoryManager;
//  public readonly PagedListOptions ListOptions = new PagedListOptions (1,20, 9999 };

//  public async ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
//    var returnValue = await _repositoryManager.WeatherForecastStoreRepository.DeleteAsync(id, cancellationToken);
//    Record = new WeatherForecast();
//    NotifyRecordChanged();
//    NotifyRecordSetChanged();
//    return returnValue;
//  }




//  public async ValueTask<List<WeatherForecast>> GetPageAsync(ListOptions listOptions, CancellationToken cancellationToken = default) {
//    Records = await _repositoryManager.WeatherForecastStoreRepository.GetListAsync(listOptions, cancellationToken);
//    RecordCount = await _repositoryManager.WeatherForecastStoreRepository.GetCountAsync(cancellationToken);
//    return Records;
//  }

//  public virtual async ValueTask<ItemsProviderResult<WeatherForecast>> GetPageAsync(ItemsProviderRequest request, CancellationToken cancellationToken = default) {
//    ListOptions.StartRecord = request.StartIndex;
//    ListOptions.PageSize = request.Count;
//    Records = await _repositoryManager.WeatherForecastStoreRepository.GetListAsync(ListOptions, cancellationToken);
//    var listCount = RecordCount = await _repositoryManager.WeatherForecastStoreRepository.GetCountAsync(cancellationToken);
//    return new ItemsProviderResult<WeatherForecast>(Records, listCount);
//  }



//}

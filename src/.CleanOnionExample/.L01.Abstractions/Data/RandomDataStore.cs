using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Data;

public class RandomDataStore {
  public RandomDataStore(int randomRecordsToGet = 5) {
    WeatherForecasts = WeatherForecast.GetRandomListAsync(randomRecordsToGet).Result.ToList();
  }
  public List<WeatherForecast> WeatherForecasts { get; }

  public ValueTask<bool> Insert(WeatherForecast weatherForecast) {
    var isOverwrite = Delete(weatherForecast.Id);
    WeatherForecasts.Add(weatherForecast);
    return isOverwrite;
  }

  public ValueTask<bool> Delete(Guid Id) {
    var record = WeatherForecasts.FirstOrDefault(item => item.Id == Id);
    if (record != null) {
      WeatherForecasts.Remove(record);
    }
    return ValueTask.FromResult(record != null);
  }

  public ValueTask<int> GetRecordCount() => ValueTask.FromResult(WeatherForecasts.Count);

  public ValueTask<WeatherForecast> GetById(Guid Id) {
    var record = WeatherForecasts.FirstOrDefault(item => item.Id == Id);
    return ValueTask.FromResult(record is null ? new WeatherForecast() : record);
  }

  public ValueTask<List<WeatherForecast>> GetPagedList(int skip, int take) {
    var list = WeatherForecasts
         .OrderBy(item => item.Date)
         .Skip(skip)
         .Take(take)
         .ToList();
    //var returnList = new List<WeatherForecast>();
    //foreach (var item in list)
    //  returnList.Add(item with { });
    return ValueTask.FromResult(list);
  }

  //public WeatherForecast ToDto(WeatherForecastDto record) => new WeatherForecast {
  //  Id = record.Id,
  //  Date = record.Date,
  //  TemperatureC = record.TemperatureC,
  //  Summary = (Domain.Entities.WeatherForecastSummary?)record.Summary
  //};

  //public static WeatherForecastDto FromDto(WeatherForecast record) => new WeatherForecastDto {
  //  Id = record.Id,
  //  Date = record.Date,
  //  TemperatureC = record.TemperatureC,
  //  Summary = (Contracts.WeatherForecastSummary?)record.Summary
  //};

  //public static WeatherForecastDto NewRandom(int index) => FromDto(WeatherForecast.NewRandom(index, DateTime.Now));


  //public void OverrideWeatherForecastDataSet(List<WeatherForecast> list) {
  //  _records.Clear();
  //  list.ForEach(item => _records.Add(item));
  //}

  //public List<WeatherForecast> CreateTestForecasts(int count) => Enumerable.Range(1, count).Select(index => WeatherForecast.GetRandom(index, DateTime.Now)).ToList();

}
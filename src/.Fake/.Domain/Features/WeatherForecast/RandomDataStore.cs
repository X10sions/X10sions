namespace X10sions.Fake.Features.WeatherForecast;

public class RandomDataStore {

  private RandomDataStore(List<FakeWeatherForecast> initialData) {
    WeatherForecasts = initialData;
  }

  public List<FakeWeatherForecast> WeatherForecasts { get; }

  // 3. Safe async initialization instead of blocking in constructor
  public static async Task<RandomDataStore> CreateAsync(int randomRecordsToGet = 5) {
    var data = await FakeWeatherForecast.GetRandomListAsync(randomRecordsToGet);
    return new RandomDataStore(data.ToList());
  }

  public async ValueTask<bool> Insert(FakeWeatherForecast weatherForecast) {
    var isOverwrite = await Delete(weatherForecast.Id);
    WeatherForecasts.Add(weatherForecast);
    return isOverwrite;
  }

  public ValueTask<bool> Delete(Guid id) {
    var record = WeatherForecasts.FirstOrDefault(item => item.Id == id);
    if (record != null) {
      WeatherForecasts.Remove(record);
    }
    return ValueTask.FromResult(record != null);
  }

  public ValueTask<int> GetRecordCount() => ValueTask.FromResult(WeatherForecasts.Count);

  public ValueTask<FakeWeatherForecast?> GetById(Guid id) {
    var record = WeatherForecasts.FirstOrDefault(item => item.Id == id);
    return ValueTask.FromResult(record);
  }

  public ValueTask<List<FakeWeatherForecast>> GetPagedList(int skip, int take) {
    var list = WeatherForecasts.OrderBy(item => item.Date).Skip(skip).Take(take).ToList();
    return ValueTask.FromResult(list);
  }

  public void OverrideWeatherForecastDataSet(List<FakeWeatherForecast> list) {
    WeatherForecasts.Clear();
    WeatherForecasts.AddRange(list);
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

  //public List<WeatherForecast> CreateTestForecasts(int count) => Enumerable.Range(1, count).Select(index => WeatherForecast.GetRandom(index, DateTime.Now)).ToList();


}



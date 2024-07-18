using Azure.Core.GeoJson;
using System.Text.Json;
using System.Text;
using Common.Collections.Paged;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Features.WeatherForecast;

namespace CleanOnionExample.Data.Entities;

public class WeatherForecastApiRepository(HttpClient HttpClient, ILogger Logger) : HttpRepositoryBase(HttpClient, Logger), IWeatherForecastRepository {
  private const string BaseUrl = "http://calculator";
  private const string Url = $"{BaseUrl}/weather";

  public async ValueTask<bool> AddAsync(WeatherForecast record, CancellationToken cancellationToken = default) {
    await base.PutAsync(Url + "/Add",record);
    return true;
  }

  public async ValueTask<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default) {
    await base.DeleteAsync(Url + "/Add", false);
    return true;
  }

  public async Task<WeatherForecast> GetByGeoPointIdAsync(GeoPoint geoPoint) {
    using var request = new HttpRequestMessage(HttpMethod.Get, Url);
    request.Content = new StringContent(JsonSerializer.Serialize(geoPoint), Encoding.UTF8, Application_Json);
    var response = await HttpClient.SendAsync(request);
    var content = await response.Content.ReadAsStringAsync();
    var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(content);
    return weatherForecast;
  }

  public async ValueTask<WeatherForecast> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
    var response = await base.GetAsync<WeatherForecast>(Url + "/Get/" +id );
    return response ?? throw new KeyNotFoundException(id.ToString());
  }

  public async ValueTask<int> GetCountAsync(CancellationToken cancellationToken = default) {
    var response = await base.GetAsync<List<WeatherForecast>>(Url + "/Get") ;
    return response?.Count ?? 0;
  }

  public async ValueTask<List<WeatherForecast>> GetListAsync(PagedListOptions options, CancellationToken cancellationToken = default) {
    var response = await base.GetAsync<List<WeatherForecast>>(Url + "/Get");
    return response ?? [];
  }

}

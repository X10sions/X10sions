using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Services;
using Common.Collections.Paged;
using Common.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanOnionExample.Controllers {
  public class WeatherForecastController : BaseApiController {
    public WeatherForecastController(IRepository<WeatherForecast> dbService, ILogger<WeatherForecastController> logger) {
      this.dbService = dbService;
      _logger = logger;
    }
    private readonly ILogger<WeatherForecastController> _logger;
    IRepository<WeatherForecast> dbService;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get() {
      return Enumerable.Range(1, 5).Select(index => WeatherForecast.GetRandom(index)).ToArray();
    }


    [HttpGet(Name = "RandomList")]
    public async Task<IEnumerable<WeatherForecast>> RandomList(int randomCount = 5, DateOnly? startDate = null) => await WeatherForecast.GetRandomListAsync(randomCount, startDate);


    // https://github.com/ShaunCurtis/Blazor.App/blob/master/Blazor.App.Controllers/Entities/WeatherForecasts/WeatherForecastController.cs

    [HttpPost, Route("/api/weatherforecast/add")]
    public async Task<int> AddAsync([FromBody] WeatherForecast.Update.Command record, CancellationToken cancellationToken = default)
      => await dbService.InsertAsync(record.Adapt<WeatherForecast>(), cancellationToken);

    [HttpPost, Route("/api/weatherforecast/delete")]
    public async Task<int> DeleteAsync([FromBody] Guid Id, CancellationToken cancellationToken = default) {
      var row = await dbService.GetByIdAsync(Id, cancellationToken);
      return row == null ? 0 : await dbService.DeleteAsync(row, cancellationToken);
    }

    [HttpGet, Route("/api/weatherforecast/get")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
      => Ok(await dbService.GetByIdAsync(id, cancellationToken));

    [HttpGet, Route("/api/weatherforecast/list")]
    public async Task<List<WeatherForecast>> ListAsync(CancellationToken cancellationToken = default)
      => await dbService.Query.ToListAsync(cancellationToken);

    [HttpGet, Route("/api/weatherforecast/page")]
    public async Task<List<WeatherForecast>> PageAsync([FromBody] PagedListOptions options, CancellationToken cancellationToken = default)
      => await dbService.Query.ToPagedListAsync(options.PageNumber, options.PageSize, cancellationToken);



  }
}
using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.AspNetCore.Mvc;

namespace X10sions.ERP.Web.ApiControllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ApiControllerBase {
  public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastService weatherForecastService) {
    _logger = logger;
    _weatherForecastService = weatherForecastService;
  }
  private readonly ILogger<WeatherForecastController> _logger;
  WeatherForecastService _weatherForecastService;

  [HttpGet(Name = "GetWeatherForecast")]
  public async Task<IEnumerable<WeatherForecast>> GetAsync() => await _weatherForecastService.GetForecastAsync(DateTime.Now.ToDateOnly());
}

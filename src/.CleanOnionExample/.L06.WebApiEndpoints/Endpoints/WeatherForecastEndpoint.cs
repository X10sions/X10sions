using CleanOnionExample.Data.Entities;
using FastEndpoints;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CleanOnionExample.Endpoints {

  public class WeatherForecastEndpoint : Endpoint<WeatherForecastEndpoint.Contracts.Request, WeatherForecastEndpoint.Contracts.ListResponse, WeatherForecastEndpoint.Mapper> {
    //public WeatherForecastEndpoint(ILogger<WeatherForecastEndpoint> logger) {
    //  this.logger = logger;
    //}

    public ILogger<WeatherForecastEndpoint> Logger { get; init; }

    public override void Configure() {
      Verbs(Http.GET);
      Routes("weather/{days}");
      AllowAnonymous();
      Description(x => x.Produces<Contracts.Response>(200, "application/json"));
    }

    public override async Task HandleAsync(Contracts.Request req, CancellationToken ct) {
      Logger.LogDebug("Retrieving weather for {Days} days.", req.Days);
      var forecasts = Enumerable.Range(1, req.Days).Select(index => WeatherForecast.GetRandom(index)).ToArray();
      var response = new Contracts.ListResponse {
        Forecasts = forecasts.Select(Map.FromEntity)
      };
      await SendAsync(response, cancellation: ct);
    }

    public class Contracts {
      public class Request {
        public int Days { get; init; }

        public class Validator : Validator<Request> {
          public Validator() {
            RuleFor(x => x.Days)
              .GreaterThanOrEqualTo(1).WithMessage("Weather forecast days must be a least 1.")
              .LessThanOrEqualTo(14).WithMessage("Weather forecast be at most 14 days.")
              ;
          }
        }
      }

      public class Response {
        [JsonConverter(typeof(DateOnlyConverter))] public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public WeatherForecastSummary Summary { get; set; }
      }

      public class ListResponse {
        public IEnumerable<Response> Forecasts { get; set; }
      }

    }

    public class Mapper : Mapper<Contracts.Request, Contracts.Response, WeatherForecast> {

      public override Contracts.Response FromEntity(WeatherForecast e) {
        return new Contracts.Response {
          Date = e.Date,
          Summary = e.Summary,
          TemperatureC = e.TemperatureC,
          TemperatureF = e.TemperatureF
        };
      }


    }

  }
}

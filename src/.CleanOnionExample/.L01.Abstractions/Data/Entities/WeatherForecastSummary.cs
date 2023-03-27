namespace CleanOnionExample.Data.Entities;

public enum WeatherForecastSummary { Freezing, Bracing, Chilly, Cool, Mild, Warm, Balmy, Hot, Sweltering, Scorching }


//private static readonly string[] Summaries = new[] {
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//    };

//public class WeatherForecastSummary2 : Common.Enumeration {
//  public static WeatherForecastSummary2 Freezing = new(1, nameof(Freezing));
//  public static WeatherForecastSummary2 Bracing = new(2, nameof(Bracing));
//  public static WeatherForecastSummary2 Chilly = new(3, nameof(Chilly));
//  public static WeatherForecastSummary2 Cool = new(4, nameof(Cool));
//  public static WeatherForecastSummary2 Mild = new(5, nameof(Mild));
//  public static WeatherForecastSummary2 Warm = new(6, nameof(Warm));
//  public static WeatherForecastSummary2 Balmy = new(7, nameof(Balmy));
//  public static WeatherForecastSummary2 Hot = new(8, nameof(Hot));
//  public static WeatherForecastSummary2 Sweltering = new(9, nameof(Sweltering));
//  public static WeatherForecastSummary2 Scorching = new(10, nameof(Scorching));

//  public WeatherForecastSummary2(int id, string name) : base(id, name) {
//  }
//}

public static class WeatherForecastSummaryExtensions {
  public static WeatherForecastSummary GetRandom(this WeatherForecastSummary[] weatherForecastSummarys)
    => weatherForecastSummarys[Random.Shared.Next(weatherForecastSummarys.Length)];
}
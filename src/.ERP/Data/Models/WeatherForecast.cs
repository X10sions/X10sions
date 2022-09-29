namespace X10sions.ERP.Data.Models {
  public class WeatherForecast {
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public WeatherForecastSummary? Summary { get; set; }
  }
}
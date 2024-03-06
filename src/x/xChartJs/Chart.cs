using Newtonsoft.Json;

namespace ChartJs {
  //[DataContract()]
  public class Chart {
    //https://github.com/jdanylko/Charts-RazorPage/blob/master/Charts-RazorPage/Models/Chart/ChartJs.cs

    public ChartType Type { get; set; }
    public ChartJsData Data { get; set; } = new ChartJsData();
    public ChartJsOptions Options { get; set; } = new ChartJsOptions();

    [JsonConverter(typeof(PlainJsonStringConverter))] public string? Plugins { get; set; }

    public enum ChartType { bar, bubble, radar, polarArea, pie, line, doughnut, horizontalBar, scatter }

    public string ToJsonData() => JsonConvert.SerializeObject(this, ChartJsonSerializerSettings.Instance);

    public string CreateChartCode(string canvasId)
      => $"var {canvasId}Element = document.getElementById(\"{canvasId}\").getContext('2d');\r\n"
      + $"var {canvasId} = new Chart({canvasId}Element, {ToJsonData()}\r\n"
      + ");";

  }

}
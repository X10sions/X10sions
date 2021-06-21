using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace ChartJs {
  //[DataContract()]
  public class Chart {
    //https://github.com/jdanylko/Charts-RazorPage/blob/master/Charts-RazorPage/Models/Chart/ChartJs.cs

       public ChartType Type { get; set; }
     public ChartJsData Data { get; set; } = new ChartJsData();
    public ChartJsOptions Options { get; set; } = new ChartJsOptions();

    [JsonConverter(typeof(PlainJsonStringConverter))] public string? Plugins { get; set; }

    public enum ChartType { bar, bubble, radar, polarArea, pie, line, doughnut, horizontalBar, scatter }

    public string ToJsonData() => JsonConvert.SerializeObject(this,ChartJsonSerializerSettings.Instance);

    public string CreateChartCode(string canvasId)
      => $"var {canvasId}Element = document.getElementById(\"{canvasId}\").getContext('2d');\r\n"
      + $"var {canvasId} = new Chart({canvasId}Element, {ToJsonData()}\r\n"
      + ");";

  }


  public class ChartJsonSerializerSettings : JsonSerializerSettings {
    public static readonly ChartJsonSerializerSettings Instance = new ChartJsonSerializerSettings();

    public ChartJsonSerializerSettings() {
      ContractResolver = _ContractResolver.Instance;
      Converters = new[] { CamelCaseStringEnumConverter.Instance };
      Formatting = Formatting.Indented     ;
      NullValueHandling = NullValueHandling.Ignore;
    }

    public class _ContractResolver : CamelCasePropertyNamesContractResolver {
      public static readonly _ContractResolver Instance = new _ContractResolver();
    }


  }

}
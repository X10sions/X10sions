using Common.Html.Css.Enums;
using Newtonsoft.Json;

namespace ChartJs.Options {
  public class Animation {
    public int? Duration { get; set; }
    public AnimationEasing? Easing { get; set; }
    [JsonConverter(typeof(PlainJsonStringConverter))] public string? OnProgress { get; set; }
    [JsonConverter(typeof(PlainJsonStringConverter))] public string? OnComplete { get; set; }
  }
}
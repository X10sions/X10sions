using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace System {
  public static class DateTimeExtensions {
    public static string ToJsonIso(this DateTime d) => JsonConvert.SerializeObject(d);
    public static string ToJsonJavascript(this DateTime d) => JsonConvert.SerializeObject(d, new JavaScriptDateTimeConverter());
    public static string ToJsonMicrosoft(this DateTime d) => JsonConvert.SerializeObject(d, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat });
  }
}
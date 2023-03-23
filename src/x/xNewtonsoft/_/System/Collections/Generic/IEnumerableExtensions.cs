using Newtonsoft.Json;

namespace System.Collections.Generic;
public static class IEnumerableExtensions {

  public static T? GetFromJson<T>(this IEnumerable<KeyValuePair<string, string>> kvps, string key, T defaultValue) {
    var json = kvps.GetValue(key);

    return json is null ? defaultValue : JsonConvert.DeserializeObject<T>(json);
  }

  //private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
  //  DateFormatString = "yyyy-MM-dd HH:mm:ss",
  //  ReferenceLoopHandling = ReferenceLoopHandling.Serialize
  //};

}


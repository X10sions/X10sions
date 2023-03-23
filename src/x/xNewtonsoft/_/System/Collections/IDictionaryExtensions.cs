using Newtonsoft.Json;

namespace System.Collections.Generic;
public static class IDictionaryExtensions {
  public static T? GetNewtonsoft<T>(this IDictionary<string, object?> dic, string key, T? defaultValue = default) {
    dic.TryGetValue(key, out var o);
    return (o == null) ? defaultValue : JsonConvert.DeserializeObject<T>(o.ToString() ?? string.Empty);
  }
  public static void Put<T>(this IDictionary<string, object?> dic, string key, T value) => dic[key] = JsonConvert.SerializeObject(value);

}

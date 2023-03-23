using System.Text.Json;

namespace System.Collections.Generic {
  public static class IEnumerableExtensions {

    public static T? GetFromJson<T>(this IEnumerable<KeyValuePair<string, string>> kvps, string key, T defaultValue) {
      var json = kvps.GetValue(key);
      return json is null ? defaultValue : JsonSerializer.Deserialize<T>(json, options);
    }

    private static readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

  }
}

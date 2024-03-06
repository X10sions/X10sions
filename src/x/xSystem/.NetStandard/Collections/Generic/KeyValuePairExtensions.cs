namespace System.Collections.Generic {
  public static class KeyValuePairExtensions {
    public static string DebugString<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, bool includeChildren = false) => $"{kvp.Key}:{kvp.Value}";

    public static string? GetValue(this IEnumerable<KeyValuePair<string, string>> kvps, string key) => kvps.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;

    public static string? GetValue<T>(this IEnumerable<KeyValuePair<string, string>> kvps) => kvps.GetValue(typeof(T).FullName ?? typeof(T).Name);

  }
}

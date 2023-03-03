namespace System.Collections.Generic {
  public static class KeyValuePairExtensions {

    public static string DebugString<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, bool includeChildren = false) => $"{kvp.Key}:{kvp.Value}";

  }
}

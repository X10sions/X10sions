namespace System.Text.Json.Nodes;

public static class JsonObjectExtensions {

  /// <summary> https://github.com/dotnet/runtime/issues/31433#issuecomment-951173115 </summary>
  public static void AddRange(this JsonObject jsonObject, IEnumerable<KeyValuePair<string, JsonNode?>> properties) {
    foreach (var kvp in properties) {
      jsonObject.Add(kvp);
    }
  }
}

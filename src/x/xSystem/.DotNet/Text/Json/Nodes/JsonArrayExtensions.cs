namespace System.Text.Json.Nodes;

public static class JsonArrayExtensions {
  /// <summary> https://github.com/dotnet/runtime/issues/31433#issuecomment-951173115 </summary>
  public static void AddRange(this JsonArray jsonArray, IEnumerable<JsonNode?> values) {
    foreach (var value in values) {
      jsonArray.Add(value);
    }
  }

}

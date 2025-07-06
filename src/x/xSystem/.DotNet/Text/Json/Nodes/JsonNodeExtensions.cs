namespace System.Text.Json.Nodes;
public static class JsonNodeExtensions {
  /// <summary>https://github.com/dotnet/runtime/issues/31433#issuecomment-2148885279 </summary>
  public static JsonNode? Merge(this JsonNode? jsonBase, JsonNode? jsonMerge)  {
    if (jsonBase == null || jsonMerge == null) return jsonBase;
    switch (jsonBase) {
      case JsonObject jsonBaseObj when jsonMerge is JsonObject jsonMergeObj: {
          var mergeNodesArray = jsonMergeObj.ToArray();
          jsonMergeObj.Clear();
          foreach (var prop in mergeNodesArray) {
            if (jsonBaseObj[prop.Key] is JsonObject jsonBaseChildObj && prop.Value is JsonObject jsonMergeChildObj)
              jsonBaseObj[prop.Key] = jsonBaseChildObj.Merge(jsonMergeChildObj);
            else
              jsonBaseObj[prop.Key] = prop.Value;
          }
          break;
        }
      case JsonArray jsonBaseArray when jsonMerge is JsonArray jsonMergeArray: {
          //NOTE: We must materialize the set (e.g. to an Array), and then clear the merge array,
          //      so they can then be re-assigned to the target/base Json...
          var mergeNodesArray = jsonMergeArray.ToArray();
          jsonMergeArray.Clear();
          foreach (var mergeNode in mergeNodesArray) jsonBaseArray.Add(mergeNode);
          break;
        }
      default:
        throw new ArgumentException($"The JsonNode type [{jsonBase.GetType().Name}] is incompatible for merging with the target/base " +
                                            $"type [{jsonMerge.GetType().Name}]; merge requires the types to be the same.");
    }
    return jsonBase;
  }

  public static JsonNode? MergeDictionary<TKey, TValue>(this JsonNode jsonBase, IDictionary<TKey, TValue> dictionary, JsonSerializerOptions? options = null)
    => jsonBase.Merge(JsonSerializer.SerializeToNode(dictionary, options));

}
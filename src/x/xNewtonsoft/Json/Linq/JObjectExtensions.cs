namespace Newtonsoft.Json.Linq {
  public static class JObjectExtensions {

    public static JObject MergeFromJsonFiles(this JObject jObject, IEnumerable<string> jsonFilePaths, MergeArrayHandling mergeArrayHandling = MergeArrayHandling.Union) {
      foreach (var path in jsonFilePaths) {
        var file = new FileInfo(path);
        var fileText = "";
        if (file.Exists) {
          fileText = File.ReadAllText(path);
        }
        var fileJObject = JObject.Parse(fileText);
        jObject.Merge(fileJObject, new JsonMergeSettings { MergeArrayHandling = mergeArrayHandling });
      }
      return jObject;
    }

    public static FileInfo WriteToJsonFile(this JObject jObject, string targetJsonFileName) {
      File.WriteAllText(targetJsonFileName, jObject.ToString());
      return new FileInfo(targetJsonFileName);
    }

    public static string? GetString(this JObject jObject, object key) => jObject?.Value<string>(key);

  }
}
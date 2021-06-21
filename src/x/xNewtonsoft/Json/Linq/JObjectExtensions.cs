using System.IO;

namespace Newtonsoft.Json.Linq {
  public static class JObjectExtensions {

    public static JObject MergeFromJsonFiles(this JObject jObject, string[] jsonFilePaths) {
      foreach (var path in jsonFilePaths) {
        var fileText = File.ReadAllText(path);
        var fileJObject = JObject.Parse(fileText);
        jObject.Merge(fileJObject);
      }
      return jObject;
    }

    public static string GetString(this JObject jObject, object key) => jObject?.Value<string>(key);

  }
}
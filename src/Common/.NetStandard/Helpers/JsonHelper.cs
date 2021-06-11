using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Common.Helpers {
  public static class JsonHelper {

    public static string MergeJsonStringFromFiles(this string[] jsonFilePaths) {
      var returnJObject = JObject.Parse("{}");
      foreach (var path in jsonFilePaths) {
        var fileText = File.ReadAllText(path);
        var fileJObject = JObject.Parse(fileText);
        returnJObject.Merge(fileJObject);
      }
      return returnJObject.ToString();
    }

    public static T MergeJsonObjectFromFiles<T>(this string[] jsonFilePaths) => JsonConvert.DeserializeObject<T>(jsonFilePaths.MergeJsonStringFromFiles());

  }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System {
  public static class TExtensions {

    public static T PopulateFromJsonFiles<T>(this T obj, params string[] jsonFilePaths) where T: notnull  {
      var jObject = obj.ToJObject().MergeFromJsonFiles(jsonFilePaths);
      var json = jObject.ToString();
      JsonConvert.PopulateObject(json, obj);
      return obj;
    }

  }
}

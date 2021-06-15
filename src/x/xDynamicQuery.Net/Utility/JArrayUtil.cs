using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace DynamicQueryNet.Utility {
  public static class JArrayUtil {
    public static bool IsJArray(object value) => value.GetType() == typeof(JArray);
    public static JArray GetJArray(object value) => (JArray)value;
    public static List<object> GetValues(object value) => GetJArray(value).Select(p => ((JValue)p).Value).ToList();
  }
}

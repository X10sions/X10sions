using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Converters {
  public class CamelCaseStringEnumConverter : StringEnumConverter {
    public static readonly CamelCaseStringEnumConverter Instance = new CamelCaseStringEnumConverter();

    public CamelCaseStringEnumConverter() : base(new CamelCaseNamingStrategy()) { }
  }
}

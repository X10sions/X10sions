using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Converters {
  public class KebabCaseStringEnumConverter : StringEnumConverter {
    public static readonly KebabCaseStringEnumConverter Instance = new KebabCaseStringEnumConverter();

    public KebabCaseStringEnumConverter() : base(new KebabCaseNamingStrategy()) { }
  }

}

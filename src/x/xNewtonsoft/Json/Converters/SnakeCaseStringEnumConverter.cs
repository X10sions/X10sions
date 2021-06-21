using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Converters {
  public class SnakeCaseStringEnumConverter : StringEnumConverter {
    public static readonly SnakeCaseStringEnumConverter Instance = new SnakeCaseStringEnumConverter();

    public SnakeCaseStringEnumConverter() : base(new SnakeCaseNamingStrategy()) { }
  }

}

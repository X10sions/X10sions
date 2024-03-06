using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ChartJs {
  public class ChartJsonSerializerSettings : JsonSerializerSettings {
    public static readonly ChartJsonSerializerSettings Instance = new ChartJsonSerializerSettings();

    public ChartJsonSerializerSettings() {
      ContractResolver = _ContractResolver.Instance;
      Converters = new[] { CamelCaseStringEnumConverter.Instance };
      Formatting = Formatting.Indented     ;
      NullValueHandling = NullValueHandling.Ignore;
    }

    public class _ContractResolver : CamelCasePropertyNamesContractResolver {
      public static readonly _ContractResolver Instance = new _ContractResolver();
    }


  }

}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace DataTables;

public class DataTablesJsonSerializerSettings : JsonSerializerSettings {
  public static readonly DataTablesJsonSerializerSettings Instance = new DataTablesJsonSerializerSettings();

  public DataTablesJsonSerializerSettings() {
    ContractResolver = CamelCaseContractResolver.Instance;
    Converters = [CamelCaseStringEnumConverter.Instance];
    Formatting = Formatting.Indented;
    NullValueHandling = NullValueHandling.Ignore;
  }

  public class CamelCaseContractResolver : CamelCasePropertyNamesContractResolver {
    public static readonly CamelCaseContractResolver Instance = new();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
      var property = base.CreateProperty(member, memberSerialization);
      property.PropertyName = property.PropertyName switch {
        null => "",
        _ => "data-" + property.PropertyName.ToKebabCase()
      };
      //    //if (property.DeclaringType == typeof(MyCustomObject)) {
      //    //  if (property.PropertyName.Equals("LongPropertyName", StringComparison.OrdinalIgnoreCase)) {
      //    //    property.PropertyName = "Short";
      //    //  }
      //    //}
      return property;
    }
  }

}

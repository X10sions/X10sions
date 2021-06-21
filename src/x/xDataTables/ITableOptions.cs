using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace DataTables {

  public class DataTablesJsonSerializerSettings : JsonSerializerSettings {
    public static readonly DataTablesJsonSerializerSettings Instance = new DataTablesJsonSerializerSettings();

    public DataTablesJsonSerializerSettings() {
      ContractResolver = _ContractResolver.Instance;
      Converters = new[] { CamelCaseStringEnumConverter.Instance };
      Formatting = Formatting.Indented;
      NullValueHandling = NullValueHandling.Ignore;
    }

    public class _ContractResolver : CamelCasePropertyNamesContractResolver {
      public static readonly _ContractResolver Instance = new _ContractResolver();

      protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var property = base.CreateProperty(member, memberSerialization);
        property.PropertyName = property.PropertyName switch {
          null => "",
          _ => "data-" + property.PropertyName.xToKebabCase()
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
}
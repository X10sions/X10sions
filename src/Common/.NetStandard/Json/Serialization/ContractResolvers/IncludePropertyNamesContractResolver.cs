using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace Common.Json.Serialization.ContractResolvers {
  public class IncludePropertyNamesContractResolver : DefaultContractResolver {
    public IncludePropertyNamesContractResolver(params string[] includePropertyNames) {
      PropertyNames = includePropertyNames;
    }

    public string[] PropertyNames { get; set; }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
      var jsonProperty = base.CreateProperty(member, memberSerialization);
      jsonProperty.Ignored = !PropertyNames.Contains(jsonProperty.PropertyName);
      return jsonProperty;
    }

  }
}
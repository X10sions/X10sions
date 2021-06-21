using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Serialization {
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
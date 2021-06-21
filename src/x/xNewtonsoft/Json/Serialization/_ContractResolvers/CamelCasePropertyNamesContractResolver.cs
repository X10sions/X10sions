using System;

namespace Newtonsoft.Json.Serialization {
  public class CamelCasePropertyNamesContractResolver : DefaultContractResolver {
    public CamelCasePropertyNamesContractResolver() {
      NamingStrategy = new CamelCaseNamingStrategy();
    }

    protected override string ResolvePropertyName(string propertyName) => propertyName.ToCamelCase();
  }
}
using System;

namespace Newtonsoft.Json.Serialization {
  public class KebabCasePropertyNamesContractResolver: DefaultContractResolver {
    public KebabCasePropertyNamesContractResolver() {
      NamingStrategy = new KebabCaseNamingStrategy();
    }

    protected override string ResolvePropertyName(string propertyName) =>  propertyName.xToKebabCase();
  }
}
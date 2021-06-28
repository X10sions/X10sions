using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization {
  public class KebabCasePropertyNamesContractResolver: DefaultContractResolver {
    public KebabCasePropertyNamesContractResolver() {
      NamingStrategy = new KebabCaseNamingStrategy();
    }

    protected override string ResolvePropertyName(string propertyName) =>  propertyName.ToKebabCase();
  }
}
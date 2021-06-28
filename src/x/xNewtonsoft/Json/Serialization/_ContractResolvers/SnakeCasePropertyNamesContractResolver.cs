using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization {
  //public class SnakeCasePropertyNamesContractResolver : DeliminatorSeparatedPropertyNamesContractResolver {
  //  public SnakeCasePropertyNamesContractResolver() : base('_') {
  //  }
  //}

  public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver {
    public SnakeCasePropertyNamesContractResolver() {
      NamingStrategy = new SnakeCaseNamingStrategy();
    }

    protected override string ResolvePropertyName(string propertyName) => propertyName.ToSnakeCase();
    //protected override string ResolvePropertyName(string propertyName) => propertyName.ToSnakeCase();
  }
}
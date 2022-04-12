namespace Newtonsoft.Json.Serialization;
public class PropertyStartsWithContractResolver : DefaultContractResolver {
  private readonly char _startingWithChar;
  public PropertyStartsWithContractResolver(char startingWithChar) {
    _startingWithChar = startingWithChar;
  }

  protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
    var properties = base.CreateProperties(type, memberSerialization);
    // only serializer properties that start with the specified character
    //if (properties != null) {
    properties = properties.Where(p =>  p.PropertyName != null && p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();
    //}
    return properties;
  }
}

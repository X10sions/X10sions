using System.Net;

namespace Newtonsoft.Json.Converters;
public class IPAddressJsonConverter : JsonConverter {
  public override bool CanConvert(Type objectType) => (objectType == typeof(IPAddress));
  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => reader.Value switch {
    string s => IPAddress.Parse(s),
    _ => null
  };
  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => writer.WriteValue(value?.ToString());

}

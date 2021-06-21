using System;

namespace Newtonsoft.Json {
  public class DoubleStringConverter : JsonConverter {
    public override bool CanConvert(Type objectType) => objectType == typeof(string);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => reader.Value;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
      double doubleValue;
      if (double.TryParse(value?.ToString(), out doubleValue)) {
        writer.WriteValue(doubleValue);
      } else {
        writer.WriteValue(value?.ToString());
      }
    }
  }

}
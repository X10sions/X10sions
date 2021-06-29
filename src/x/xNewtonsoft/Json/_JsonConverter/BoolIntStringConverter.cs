using System;

namespace Newtonsoft.Json {
  public class BoolIntStringConverter : JsonConverter {
    public override bool CanConvert(Type objectType) => objectType == typeof(string);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => reader.Value;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
      var stringValue = value?.ToString();
      int intValue;
      bool boolValue;
      if (bool.TryParse(stringValue, out boolValue)) {
        writer.WriteValue(boolValue);
      } else if (int.TryParse(stringValue, out intValue)) {
        if (stringValue != null && (stringValue.StartsWith("+") || stringValue.StartsWith("-"))) {
          writer.WriteValue(stringValue);
        } else {
          writer.WriteValue(intValue);
        }
      } else {
        writer.WriteValue(stringValue);
      }
    }

  }
}
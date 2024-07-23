﻿using System.Text.Json.Serialization;
using System.Text.Json;

namespace RCommon.Serialization.Json;
public class JsonByteEnumConverter<T> : JsonConverter<T> where T : Enum {
  public override T Read(ref Utf8JsonReader reader, Type typeToConvert,
      JsonSerializerOptions options) {
    T value = (T)(Enum.Parse(typeof(T), reader.GetString()));
    return value;
  }

  public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
    Enum test = (Enum)Enum.Parse(typeof(T), value.ToString());
    writer.WriteNumberValue(Convert.ToByte(test));
  }
}

﻿using System;

namespace Newtonsoft.Json {
  public class ToStringJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) => true;
    public override bool CanRead => false;
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => throw new NotImplementedException();
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => writer.WriteValue(value?.ToString());
  }

}
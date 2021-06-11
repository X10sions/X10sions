﻿using Newtonsoft.Json;
using System;
using System.Net;

namespace Common.Json.JsonConverters {
  public class IPAddressJsonConverter : JsonConverter {

    public override bool CanConvert(Type objectType) => (objectType == typeof(IPAddress));
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => IPAddress.Parse((string)reader.Value);

  }
}
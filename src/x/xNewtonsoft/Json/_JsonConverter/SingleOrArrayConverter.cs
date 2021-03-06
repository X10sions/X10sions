﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json {
  public class SingleOrArrayConverter<T> : JsonConverter {
    public override bool CanConvert(Type objectType) => (objectType == typeof(List<T>));

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
      var token = JToken.Load(reader);
      if (token.Type == JTokenType.Array) {
        return token.ToObject<List<T>>();
      }
      var list = new List<T>();
      var obj = token.ToObject<T>();
      if (obj != null) {
        list.Add(obj);
      }
      return list;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
      if (value == null) return;
      var list = (List<T>)value;
      if (list.Count == 1) {
        value = list[0];
      }
      serializer.Serialize(writer, value);
    }

    public override bool CanWrite => true;
  }

}
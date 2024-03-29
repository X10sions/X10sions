﻿using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters;
public class SqlBooleanJsonConverter : SqlPrimitiveJsonConverterBase<SqlBoolean> {
  protected override object GetValue(SqlBoolean sqlValue) => sqlValue.Value;

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
    if (reader.TokenType == JsonToken.Null)
      return SqlBoolean.Null;
    return (SqlBoolean)serializer.Deserialize<bool>(reader);
  }
}

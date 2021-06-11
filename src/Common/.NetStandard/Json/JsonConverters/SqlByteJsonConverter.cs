using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Common.Json.JsonConverters {
  public class SqlByteJsonConverter : SqlPrimitiveJsonConverterBase<SqlByte> {
    protected override object GetValue(SqlByte sqlValue) { return sqlValue.Value; }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlByte.Null;
      return (SqlByte)serializer.Deserialize<byte>(reader);
    }
  }
}
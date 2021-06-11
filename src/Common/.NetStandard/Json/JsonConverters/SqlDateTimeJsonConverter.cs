using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Common.Json.JsonConverters {
  public class SqlDateTimeJsonConverter : SqlPrimitiveJsonConverterBase<SqlDateTime> {
    protected override object GetValue(SqlDateTime sqlValue) { return sqlValue.Value; }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlDateTime.Null;
      return (SqlDateTime)serializer.Deserialize<DateTime>(reader);
    }
  }
}
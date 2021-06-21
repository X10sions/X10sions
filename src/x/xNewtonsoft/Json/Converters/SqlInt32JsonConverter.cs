using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters {
  public class SqlInt32JsonConverter : SqlPrimitiveJsonConverterBase<SqlInt32> {
    protected override object GetValue(SqlInt32 sqlValue) { return sqlValue.Value; }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlInt32.Null;
      return (SqlInt32)serializer.Deserialize<int>(reader);
    }
  }
}
using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Common.Json.JsonConverters {
  public class SqlStringJsonConverter : SqlPrimitiveJsonConverterBase<SqlString> {
    protected override object GetValue(SqlString sqlValue) { return sqlValue.Value; }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlString.Null;
      return (SqlString)serializer.Deserialize<string>(reader);
    }
  }
}
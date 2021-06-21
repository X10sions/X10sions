using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters {
  public class SqlDecimalJsonConverter : SqlPrimitiveJsonConverterBase<SqlDecimal> {
    protected override object GetValue(SqlDecimal sqlValue) { return sqlValue.Value; }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlDecimal.Null;
      return (SqlDecimal)serializer.Deserialize<decimal>(reader);
    }
  }
}
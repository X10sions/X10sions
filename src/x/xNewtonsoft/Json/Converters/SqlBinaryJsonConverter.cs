using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters {
  public class SqlBinaryJsonConverter : SqlPrimitiveJsonConverterBase<SqlBinary> {
    protected override object GetValue(SqlBinary sqlValue) => sqlValue.Value;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null)
        return SqlBinary.Null;
      return (SqlBinary)serializer.Deserialize<byte[]>(reader);
    }
  }
}
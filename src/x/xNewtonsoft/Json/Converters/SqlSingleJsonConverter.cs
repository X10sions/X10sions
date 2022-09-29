using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters;
public class SqlSingleJsonConverter : SqlPrimitiveJsonConverterBase<SqlSingle> {
  protected override object GetValue(SqlSingle sqlValue) { return sqlValue.Value; }

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
    if (reader.TokenType == JsonToken.Null)
      return SqlSingle.Null;
    return (SqlSingle)serializer.Deserialize<float>(reader);
  }
}

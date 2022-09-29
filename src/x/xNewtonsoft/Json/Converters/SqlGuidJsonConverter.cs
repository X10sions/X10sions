using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters;
public class SqlGuidJsonConverter : SqlPrimitiveJsonConverterBase<SqlGuid> {
  protected override object GetValue(SqlGuid sqlValue) { return sqlValue.Value; }

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
    if (reader.TokenType == JsonToken.Null)
      return SqlGuid.Null;
    return (SqlGuid)serializer.Deserialize<Guid>(reader);
  }
}

using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters;
public abstract class SqlPrimitiveJsonConverterBase<T> : JsonConverter where T : struct, INullable, IComparable {
  protected abstract object GetValue(T sqlValue);

  public override bool CanConvert(Type objectType) => typeof(T) == objectType;

  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
    if (value is T sqlValue) {
      if (sqlValue.IsNull)
        writer.WriteNull();
      else {
        serializer.Serialize(writer, GetValue(sqlValue));
      }
    } else {
      writer.WriteNull();
    }
  }
}

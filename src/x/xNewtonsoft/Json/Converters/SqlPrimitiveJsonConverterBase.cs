using Newtonsoft.Json;
using System;
using System.Data.SqlTypes;

namespace Newtonsoft.Json.Converters {
  public abstract class SqlPrimitiveJsonConverterBase<T> : JsonConverter where T : struct, INullable, IComparable {
    protected abstract object GetValue(T sqlValue);

    public override bool CanConvert(Type objectType) => typeof(T) == objectType;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      T sqlValue = (T)value;
      if (sqlValue.IsNull)
        writer.WriteNull();
      else {
        serializer.Serialize(writer, GetValue(sqlValue));
      }
    }
  }
}
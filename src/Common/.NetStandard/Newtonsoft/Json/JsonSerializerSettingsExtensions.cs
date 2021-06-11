using Common.Json.JsonConverters;

namespace Newtonsoft.Json {
  public static class JsonSerializerSettingsExtensions {

    public static JsonSerializerSettings AddSqlConverters(this JsonSerializerSettings settings) {
      foreach (var converter in SqlJsonConverters) {
        settings.Converters.Add(converter);
      }
      return settings;
    }

    static readonly JsonConverter[] SqlJsonConverters =  {
        new SqlBinaryJsonConverter(),
        new SqlBooleanJsonConverter(),
        new SqlByteJsonConverter(),
        new SqlDateTimeJsonConverter(),
        new SqlDecimalJsonConverter(),
        new SqlDoubleJsonConverter(),
        new SqlGuidJsonConverter(),
        new SqlInt16JsonConverter(),
        new SqlInt32JsonConverter(),
        new SqlInt64JsonConverter(),
        new SqlMoneyJsonConverter(),
        new SqlSingleJsonConverter(),
        new SqlStringJsonConverter(),
        // TODO: converters for primitives from System.Data.SqlTypes that are classes not structs:
        // SqlBytes, SqlChars, SqlXml
        // Maybe SqlFileStream
    };

    public static JsonSerializerSettings AddIpConverters(this JsonSerializerSettings settings) {
      foreach (var converter in IpJsonConverters) {
        settings.Converters.Add(converter);
      }
      return settings;
    }

    static readonly JsonConverter[] IpJsonConverters =  {
        new IPAddressJsonConverter(),
        new IPEndPointJsonConverter(),
    };
  }

}
namespace System.Text.Json;
public static class JsonSerializerExtensions {
  public static T? FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json);
  public static string ToJson<T>(this T value) => JsonSerializer.Serialize(value);
}
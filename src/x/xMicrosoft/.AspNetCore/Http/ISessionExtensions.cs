using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Http {
  public static class ISessionExtensions {

    public static T? GetUsingSystemTextJson<T>(this ISession session, string key, T? defaultValue = default) {
      var value = session.GetString(key);
      return value == null ? defaultValue : JsonSerializer.Deserialize<T>(value, settings);
    }

    public static T? Get<T>(this ISession session) => session.Get<T>(typeof(T).FullName ?? typeof(T).Name);
    public static T? Get<T>(this ISession session, string key, T? defaultValue = default) => session.GetUsingSystemTextJson(key, defaultValue);

    public static bool? GetBoolean(this ISession session, string key) {
      var data = session.Get(key);
      return data == null ? null : BitConverter.ToBoolean(data, 0);
    }

    public static double? GetDouble(this ISession session, string key) {
      var data = session.Get(key);
      return data == null ? null : BitConverter.ToDouble(data, 0);
    }

    public static async Task<T?> GetUsingSystemTextJsonAsync<T>(this ISession session, string key, T? defaultValue = default) {
      if (!session.IsAvailable) {
        await session.LoadAsync();
      }
      return session.GetUsingSystemTextJson(key, defaultValue);
    }

    private static JsonSerializerOptions settings { get; } = new JsonSerializerOptions {
      DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
      //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
      IgnoreReadOnlyProperties = true
    };

    public static T SetUsingSystemTextJson<T>(this ISession session, string key, T value) {
      session.SetString(key, JsonSerializer.Serialize(value, settings));
      return value;
    }

    public static T Set<T>(this ISession session, T value) => session.Set(typeof(T).FullName ?? typeof(T).Name, value);
    public static T Set<T>(this ISession session, string key, T value) => session.SetUsingSystemTextJson(key, value);

    public static bool Set(this ISession session, string key, bool value) {
      session.Set(key, BitConverter.GetBytes(value));
      return value;
    }

    public static double Set(this ISession session, string key, double value) {
      session.Set(key, BitConverter.GetBytes(value));
      return value;
    }

    public static async Task<T> SetSystemTextJsonAsync<T>(this ISession session, string key, T value) {
      if (!session.IsAvailable) {
        await session.LoadAsync();
      }
      return session.SetUsingSystemTextJson(key, value);
    }

    public static void SetBoolean(this ISession session, string key, bool value) => session.Set(key, BitConverter.GetBytes(value));
    public static void SetDouble(this ISession session, string key, double value) => session.Set(key, BitConverter.GetBytes(value));

    public static IEnumerable<KeyValuePair<string, object?>> Values(this ISession session) => session.Keys.Select(x => new KeyValuePair<string, object?>(x, session.Get<object?>(x)));

    public static IDictionary<string, object?> ToDictionary(this ISession session) {
      var dic = new Dictionary<string, object?>();
      foreach (var k in session.Keys) {
        var value = session.Get<object?>(k);
        dic.Add(k, value);
      }
      return dic;
    }

  }
}
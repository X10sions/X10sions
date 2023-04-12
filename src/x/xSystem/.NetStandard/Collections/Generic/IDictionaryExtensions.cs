using System.Net;

namespace System.Collections.Generic {
  public static class IDictionaryExtensions {

    //public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value) => dic[key] = value;
    //public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TValue> valueFunction) => dic[key] = valueFunction();
    //public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory) => dic[key] = valueFactory(key);
    //public static TValue AddOrUpdate<TKey, TValue, TArg>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) =>  dic[key] = valueFactory(key, factoryArgument);

    public static TResult? Get<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key) => dic.Get<TKey, TValue, TResult>(key, default);

    public static TResult Get<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key, TResult defaultValue) => dic.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;

    //public static T Get<T>(this IDictionary<object, object> dic) => dic.Get<object, object, T>(typeof(T).FullName);

    //public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new() => dictionary.GetOrAdd(key, () => new TValue());
    //public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) => dictionary.GetOrAdd(key, () => value);
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFunction) {
      if (!dictionary.TryGetValue(key, out var value)) {
        value = dictionary[key] = valueFunction();
      }
      return value;
    }

    //public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key, Func<TValue> valueFunction) {
    //  var exists = dictionary.TryGetValue(key, out var value);
    //  if (exists && value is TValue) return (TValue)value;
    //  var tValue = valueFunction();
    //  dictionary[key] = tValue;
    //  return tValue;
    //}

    //public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory) => dictionary.GetOrAdd(key,  valueFactory(key));
    //public static TValue GetOrAdd<TKey, TValue, TArg>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) => dictionary.GetOrAdd (key,  valueFactory(key, factoryArgument));

    public static string JoinToHtmlString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string valueSeparator = "=", string parentTag = "ul", string childTag = "li") => $"<{parentTag}>" + string.Join($"</{parentTag}><{parentTag}>", dictionary.Select(x => $"<{childTag}>{x.Key}{valueSeparator}{x.Value}</{childTag}>")) + $"</{parentTag}>";
    public static string JoinToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string keySeparator = ";\n", string valueSeparator = "=", string prefix = "{", string suffix = "}")
      => prefix + string.Join(keySeparator, dictionary.Select(kv => $"{kv.Key}{valueSeparator}{kv.Value}")) + suffix;

    //public static T Set<T>(this IDictionary<TKey, TValue> dic, T value) {
    //  dic[typeof(T).FullName] = value;
    //  return value;
    //}

    //public static void Set<TKey,  TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key, TResult value) {
    //  if (value == null) {
    //    dic.Remove(key);
    //  } else {
    //    dic[key] = (TValue)value;
    //  }
    //}

    public static string ToQueryString(this IDictionary<string, string> parameters) {
      if (!parameters.Any())
        return string.Empty;
      return "?" + string.Join("&", parameters.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
    }

  }
}
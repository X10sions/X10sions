using System.Net;

namespace System.Collections.Generic;

public static class IDictionaryExtensions {
  public static IDictionary<TKey, TValue> AddIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, bool addIfTrue, TKey key, TValue? value) {
    if (addIfTrue) {
      dictionary.Add(key, value);
    }
    return dictionary;
  }

  public static IDictionary<TKey, TValue> AddIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, bool addIfTrue, TKey key, TValue ifTrueValue, TValue ifFalseValue) {
    dictionary.Add(key, addIfTrue ? ifTrueValue : ifFalseValue);
    return dictionary;
  }

  public static IDictionary<TKey, TValue> AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) => dictionary.AddIf(value is not null, key, value);


  public static TResult Get<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey key, TResult defaultValue) => dic.TryGetValue(key, out var value) && value is TResult t ? t : defaultValue;

  public static TResult GetAs<TKey, TDicValue, TResult>(this IDictionary<TKey, TDicValue> dictionary, TKey key, Func<TResult> valueFunction) {
    var exists = dictionary.TryGetValue(key, out var dicValue);
    return exists && dicValue is TResult tResult ? tResult : valueFunction();
  }

  public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFunction) {
    if (dictionary.TryGetValue(key, out var value)) return value;
    var newValue = valueFunction();
    dictionary.Add(key, newValue); // Cleaner and safer than double-indexing
    return newValue;
  }

  public static TResult GetOrAdd<TKey, TResult>(this IDictionary<TKey, object?> dictionary, TKey key, Func<TResult> valueFunction) {
    if (dictionary.TryGetValue(key, out var dicValue) && dicValue is TResult tResult) {
      return tResult;
    }
    var newValue = valueFunction();
    dictionary[key] = newValue;
    return newValue;
  }

  public static TResult GetOrAdd<TResult>(this IDictionary<object, object?> dictionary, Func<TResult> valueFunction) {
    object key = typeof(TResult);
    return dictionary.GetOrAdd(key, valueFunction);
  }

  public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) => dictionary.TryGetValue(key, out var value) ? value : defaultValue;
  public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider) => dictionary.TryGetValue(key, out var value) ? value : defaultValueProvider();


  public static string JoinToHtmlString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string valueSeparator = "=", string parentTag = "ul", string childTag = "li") => $"<{parentTag}>" + string.Join($"</{parentTag}><{parentTag}>", dictionary.Select(x => $"<{childTag}>{x.Key}{valueSeparator}{x.Value}</{childTag}>")) + $"</{parentTag}>";
  public static string JoinToString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string keySeparator = ";\n", string valueSeparator = "=", string prefix = "{", string suffix = "}")
    => prefix + string.Join(keySeparator, dictionary.Select(kv => $"{kv.Key}{valueSeparator}{kv.Value}")) + suffix;

  public static string ToQueryString(this IDictionary<string, string> parameters) {
    if (!parameters.Any())
      return string.Empty;
    return "?" + string.Join("&", parameters.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
  }

  public static IDictionary<TKey, TValue> MergeAttributes<TKey, TValue>(this IDictionary<TKey, TValue> existingAttributes, IDictionary<TKey, TValue> attributes, bool replaceExisting) {
    if (attributes is not null) {
      foreach (var attribute in attributes) {
        existingAttributes.MergeAttribute(attribute.Key, attribute.Value, replaceExisting);
      }
    }
    return existingAttributes;
  }

  public static IDictionary<TKey, TValue> MergeAttribute<TKey, TValue>(this IDictionary<TKey, TValue> attributes, TKey key, TValue value, bool replaceExisting) {
    if (key is null) throw new ArgumentException(nameof(key));
    if (replaceExisting || !attributes.ContainsKey(key)) {
      attributes[key] = value;
    }
    return attributes;
  }

  public static IDictionary<TKey, TValue> RemoveIfExists<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
    if (dictionary.ContainsKey(key)) {
      dictionary.Remove(key);
    }
    return dictionary;
  }

  public static IDictionary<TKey, TValue> ReplaceIf<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, bool replaceIfTrue, TKey key, TValue? value) {
    if (replaceIfTrue) {
      dictionary[key] = value;
    }
    return dictionary;
  }
  public static IDictionary<TKey, TValue> ReplaceIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) => dictionary.ReplaceIf(value is not null, key, value);

  public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
    TValue value;
    var exists = dictionary.TryGetValue(key, out value);
    return exists ? value : defaultValue;
  }

}
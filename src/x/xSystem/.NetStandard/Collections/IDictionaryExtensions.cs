namespace System.Collections;
public static class IDictionaryExtensions {

  public static T? Get<T>(this IDictionary dictionary) => dictionary.Get<T>(typeof(T).FullName ?? typeof(T).Name);

  public static T? Get<T>(this IDictionary dictionary, string key) => dictionary.Get<T>(key, default);

  public static T Get<T>(this IDictionary dictionary, string key, T defaultValue) {
    if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
    var value = dictionary[key];
    return value is not T ? defaultValue : (T)value;
  }

  public static T? GetOrCreate<T>(this IDictionary dictionary, Func<T> setFunc) => dictionary.GetOrCreate(typeof(T).FullName ?? typeof(T).Name, setFunc);

  public static T? GetOrCreate<T>(this IDictionary dictionary, string key, Func<T> setFunc) {
    var value = dictionary.Get<T>(key);
    if (value is not T) {
      value = setFunc();
      dictionary.Set(value);
    }
    return value;
  }

  public static bool HasKey(this IDictionary dictionary, string key) => dictionary[key] != null;

  public static void Set<T>(this IDictionary dictionary, T value) => dictionary.Set(typeof(T).FullName ?? typeof(T).Name, value);

  public static T Set<T>(this IDictionary dictionary, string key, T value) {
    if (dictionary is null) throw new ArgumentNullException(nameof(dictionary));
    dictionary[key] = value;
    return value;
  }

}
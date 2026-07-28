using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Web;

namespace System.Collections.Specialized;

public static class NameValueCollectionExtensions {
  public static bool ContainsKey(this NameValueCollection collection, string key) => collection.Get(key) is not null || collection.AllKeys.Contains(key);
  public static T Get<T>(this NameValueCollection collection, string key, T defaultValue) => collection.Get(key).To(defaultValue);
  public static T GetValueOrDefault<T>(this NameValueCollection collection, string key, T defaultValue) => collection[key].To(defaultValue);


  public static string[] NullKeys(this NameValueCollection collection, char splitOn = ',') => collection[null]?.Split(splitOn) ?? new string[] { };
  public static bool HasNullKey(this NameValueCollection collection, string key, IEqualityComparer<string>? comparer = null) => collection.NullKeys().Contains(key, comparer ?? StringComparer.OrdinalIgnoreCase);

  public static bool KeyExists(this NameValueCollection collection, string key) {
    int count = collection.Count;
    for (int i = 0; i < count; i++) {
      // GetKey(i) performs a direct index lookup on the internal list
      if (string.Equals(collection.GetKey(i), key, StringComparison.OrdinalIgnoreCase)) {
        return true;
      }
    }
    return false;
  }


  public static NameValueCollection RemoveKeyValue(this NameValueCollection collection, string key, object valueToRemove)  => collection.RemoveKeyValues(key, valueToRemove);

  public static NameValueCollection RemoveKeyValues<T>(this NameValueCollection collection, string key, params T[] valuesToRemove) {
    if (collection == null) return null;
    if (valuesToRemove == null || valuesToRemove.Length == 0) return collection;
    string[] currentValues = collection.GetValues(key);
    if (currentValues == null) return collection;
    var removeSet = new HashSet<string>(      valuesToRemove.Select(v => v?.ToString() ?? string.Empty),      StringComparer.OrdinalIgnoreCase    );
    var remainingValues = currentValues.Where(v => !removeSet.Contains(v)).ToList();
    if (remainingValues.Count < currentValues.Length) {
      collection.Remove(key);
      foreach (var val in remainingValues) {
        collection.Add(key, val);
      }
    }
    return collection;
  }

  public static NameValueCollection ToList<T>() where T : struct {
    var result = new NameValueCollection();
    if (!typeof(T).IsEnum) return result;
    var enumType = typeof(T);
    foreach (var value in Enum.GetValues(enumType)) {
      var memInfo = enumType.GetMember(enumType.GetEnumName(value));
      var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
      var description = descriptionAttributes.Length > 0 ? ((DescriptionAttribute)descriptionAttributes.First()).Description : value.ToString();
      result.Add(description, value.ToString());
    }
    return result;
  }

  public static string ToKeyValues(this NameValueCollection collection, string keySuffix = "=", string valueSuffix = ";") {
    var sb = new StringBuilder();
    foreach (var key in collection.AllKeys) {
      //try {
      sb.Append(key);
      sb.Append(keySuffix);
      sb.Append(collection[key]);
      sb.AppendLine(valueSuffix);
      //} catch(Exception ex) {
      //sb.Append(key).Append(keySuffix).Append("Error: "+ ex.Message).Append(valueSuffix);
      //}
    }
    return sb.ToString();
  }
  public static string ToQueryString(this NameValueCollection collection) => string.Join("&", collection.AllKeys.Select(key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(collection[key]))));

}
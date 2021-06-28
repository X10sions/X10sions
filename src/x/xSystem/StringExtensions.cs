using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System {
  public static class StringExtensions {
    //public static decimal ConvertToDecimal(this string value) => value.As<decimal>();
    //public static short ConvertToInt16Ceiling(this string value) => Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(value)));
    //public static short ConvertToInt16Floor(this string value) => Convert.ToInt16(Math.Floor(Convert.ToDecimal(value)));
    //public static short ConvertToInt16Round(this string value) => Convert.ToInt16(Math.Round(Convert.ToDecimal(value)));
    //public static int ConvertToInt32Ceiling(this string value) => Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(value)));
    //public static int ConvertToInt32Floor(this string value) => Convert.ToInt32(Math.Floor(Convert.ToDecimal(value)));
    //public static int ConvertToInt32Round(this string value) => Convert.ToInt32(Math.Round(Convert.ToDecimal(value)));
    //public static long ConvertToInt64Ceiling(this string value) => Convert.ToInt64(Math.Ceiling(Convert.ToDecimal(value)));
    //public static long ConvertToInt64Floor(this string value) => Convert.ToInt64(Math.Floor(Convert.ToDecimal(value)));
    //public static long ConvertToInt64Round(this string value) => Convert.ToInt64(Math.Round(Convert.ToDecimal(value)));

    public static string CsvEscapeQuotes(this string s) => string.IsNullOrEmpty(s) ? s : s.Replace("\"", "\"\"");

    public static bool Equals(this string s1, string s2, bool useOrdinalIgnoreCase) => useOrdinalIgnoreCase ? s1.Equals(s2, StringComparison.OrdinalIgnoreCase) : s1.Equals(s2);
    public static bool EqualsIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
    public static bool EqualsCurrentCultureIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.CurrentCultureIgnoreCase);
    public static bool EqualsInvariantCultureIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase);
    public static bool EqualsOrdinalIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase);

    public static string IfNullOrEmpty(this string s, string defaultValue) => string.IsNullOrEmpty(s) ? defaultValue : s;
    public static string IfNullOrWhiteSpace(this string s, string defaultValue) => string.IsNullOrWhiteSpace(s) ? defaultValue : s;

    public static bool EndsWithAny(this string s, string[] values, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) {
      foreach (var value in values) {
        if (s.EndsWith(value, comparisonType))
          return true;
      }
      return false;
    }

    public static bool StartsWithAny(this string s, string[] values, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) {
      foreach (var value in values) {
        if (s.StartsWith(value, comparisonType))
          return true;
      }
      return false;
    }

    public static string PadRightMax(this string @this, int maxWidth, char paddingChar) => @this.PadRight(maxWidth, paddingChar).Substring(0, maxWidth);
    public static string PadRightMaxWidth(this string @this, int maxWidth, char paddingChar) => @this.PadRight(maxWidth, paddingChar).Substring(0, maxWidth);
    public static string ReplaceFromEnd(this string s, string fromSuffix, string toSuffix, StringComparison comparisonType = StringComparison.CurrentCulture) => s.EndsWith(fromSuffix, comparisonType) ? s.Substring(0, s.Length - fromSuffix.Length) + toSuffix : s;

    public static string ReplaceFromStart(this string s, string fromPrefix, string toPrefix, StringComparison comparisonType = StringComparison.CurrentCulture) => s.StartsWith(fromPrefix, comparisonType) ? toPrefix + s.Substring(fromPrefix.Length) : s;

    public static string[] Split(this string s, string separator, StringSplitOptions splitOptions = StringSplitOptions.None) => s.Split(new[] { separator }, splitOptions);
    public static string[] Split(this string s, string separator, RegexOptions regexOptions) => Regex.Split(s, separator, regexOptions);

    public static IEnumerable<T> SplitIsNumeric<T>(this string s, string separator) => string.IsNullOrWhiteSpace(s) ? Enumerable.Empty<T>() : from x in s.Split(separator) where x.IsNumeric() select x.As<string, T>();
    public static IEnumerable<string> SplitNotNull(this string s, string separator) => from x in s.Split(separator) where x != null select x;
    public static IEnumerable<string> SplitNotNullOrEmpty(this string s, string separator) => from x in s.Split(separator) where !string.IsNullOrEmpty(x) select x;
    public static IEnumerable<string> SplitNotNullOrWhiteSpace(this string s, string separator = "'") => from x in s.Split(separator) where !string.IsNullOrWhiteSpace(x) select x;

    public static IEnumerable<T> SplitOfType<T>(this string s, string separator) => from x in s.Split(separator).OfType<T>() select x;

    public static string SqlLiteral(this string value) => value.SqlLiteral(new SqlStringOptions());
    public static string SqlLiteral(this string value, SqlStringOptions options) => value == null ? SqlOptions.SqlNullString : options.LiteralPrefix + value + options.LiteralSuffix;

    public static Dictionary<string, string> ToKeyValueDictionary(this string s, char keySeparator = ';', char valueSeparator = '=', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) => s.ToKeyValuePairs(keySeparator, valueSeparator, options).ToDictionary(k => k.Key, v => v.Value);
    public static IList<KeyValuePair<string, string>> ToKeyValueList(this string s, char keySeparator = ';', char valueSeparator = '=', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) => s.ToKeyValuePairs(keySeparator, valueSeparator, options).ToList();

    public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(this string s, char keySeparator = ';', char valueSeparator = '=', StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) {
      var segments = s.Split(new char[] { keySeparator }, options);
      var entries = from item in segments select item.Split(new char[] { valueSeparator }, options);
      return from kvp in entries select new KeyValuePair<string, string>(kvp[0], kvp.Length > 1 ? kvp[1] : string.Empty);
    }

    public static string TrimStart(this string target, string trimString) {
      if (string.IsNullOrEmpty(trimString)) return target;
      string result = target;
      while (result.StartsWith(trimString)) {
        result = result.Substring(trimString.Length);
      }
      return result;
    }

    public static string TrimEnd(this string target, string trimString) {
      if (string.IsNullOrEmpty(trimString)) return target;
      string result = target;
      while (result.EndsWith(trimString)) {
        result = result.Substring(0, result.Length - trimString.Length);
      }
      return result;
    }

    public static string WrapIfNotNullOrEmpty(this string s, string prefix = "", string suffix = "", string defaultIfNullOrEmpty = "") => string.IsNullOrEmpty(s) ? defaultIfNullOrEmpty : prefix + s + suffix;
    public static string WrapIfNotNullOrWhiteSpace(this string s, string prefix = "", string suffix = "", string defaultIfNullOrWhiteSpace = "") => string.IsNullOrWhiteSpace(s) ? defaultIfNullOrWhiteSpace : prefix + s + suffix;

    public static T ToEnum<T>(this string s, T defaultValue) => System.Enum.IsDefined(typeof(T), s) ? (T)Enum.Parse(typeof(T), s) : defaultValue;

    #region "BinaryFormatterExtensions"

    public static T? FromBase64String<T>(this string data) => FromBase64String<T>(data, null);

    public static T? FromBase64String<T>(this string data, BinaryFormatter? formatter) {
      using (var stream = new MemoryStream(Convert.FromBase64String(data))) {
        formatter = formatter ?? new BinaryFormatter();
        var obj = formatter.Deserialize(stream);
        return (obj is T) ? (T)obj : default;
      }
    }

    #endregion

    #region "XmlSerializerExtensions"

    public static T LoadFromXmlString<T>(this string xmlString) {
      using (var reader = new StringReader(xmlString)) {
        return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
      }
    }

    #endregion

  }
}
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System {
  public static class StringExtensions {

    public delegate bool TryParse<T>(string str, out T value);

    public static T? AsValue<T>(this string value, TryParse<T> parseFunc) {
      if (string.IsNullOrWhiteSpace(value)) return default;
      parseFunc(value, out var val);
      return val;
    }
    ////public static TValue As<TValue>(this string value) => value.As(default(TValue));
    //public static bool AsBool(this string value) => value.AsBool(defaultValue: false);
    //public static bool AsBool(this string value, bool defaultValue) => !bool.TryParse(value, out var result) ? defaultValue : result;
    //public static DateTime AsDateTime(this string value) => value.AsDateTime(default);
    //public static DateTime AsDateTime(this string value, DateTime defaultValue) => !DateTime.TryParse(value, out var result) ? defaultValue : result;
    //public static decimal AsDecimal(this string value) => value.As(0m);
    //public static decimal AsDecimal(this string value, decimal defaultValue) => value.As(defaultValue);
    //public static float AsFloat(this string value) => value.AsFloat(0f);
    //public static float AsFloat(this string value, float defaultValue) => !float.TryParse(value, out var result) ? defaultValue : result;
    //public static int AsInt(this string value) => value.AsInt(0);
    //public static int AsInt(this string value, int defaultValue) => !int.TryParse(value, out var result) ? defaultValue : result;

    public static IList<T> ToList<T>(this IEnumerable<string> values, TryParse<T> parseFunc) {
      var list = new List<T>();
      foreach (var v in values) {
        var typedValue = v.AsValue(parseFunc);
        if (typedValue != null) {
          list.Add(typedValue);
        }
      }
      return list;
    }

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
    public static bool Contains(this string source, string toCheck, StringComparison comp) => source?.IndexOf(toCheck, comp) >= 0;
    public static string CsvEscapeQuotes(this string s) => string.IsNullOrEmpty(s) ? s : s.Replace("\"", "\"\"");
    public static bool Equals(this string s1, string s2, bool useOrdinalIgnoreCase) => useOrdinalIgnoreCase ? s1.Equals(s2, StringComparison.OrdinalIgnoreCase) : s1.Equals(s2);
    public static bool EqualsIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
    public static bool EqualsCurrentCultureIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.CurrentCultureIgnoreCase);
    public static bool EqualsInvariantCultureIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase);
    public static bool EqualsOrdinalIgnoreCase(this string s1, string s2) => s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
    public static string FixedLengthCenter(this string s, int length, char paddingChar = ' ') => FixedLengthLeft(FixedLengthRight(s, (s.Length + length) / 2, paddingChar), length, paddingChar);
    public static string FixedLengthLeft(this string s, int length, char paddingChar = ' ') => s.Length > length ? s.Substring(0, length) : s.PadRight(length, paddingChar);
    public static string FixedLengthRight(this string s, int length, char paddingChar = ' ') => s.Length > length ? s.Substring(s.Length - length, length) : s.PadLeft(length, paddingChar);
    public static string IfNullOrEmpty(this string s, string defaultValue) => string.IsNullOrEmpty(s) ? defaultValue : s;
    public static string IfNullOrWhiteSpace(this string s, string defaultValue) => string.IsNullOrWhiteSpace(s) ? defaultValue : s;

    public static bool Is<TValue>(this string value) {
      var converter = TypeDescriptor.GetConverter(typeof(TValue));
      if (converter != null) {
        try {
          if (value == null || converter.CanConvertFrom(null, value.GetType())) {
            converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
            return true;
          }
        } catch {
        }
      }
      return false;
    }

    /// <summary>
    /// This C# implementation of SQL Like operator is based on the following SO post https://stackoverflow.com/a/8583383/10577116
    /// It covers almost all of the scenarios, and it's faster than regex based implementations.
    /// It may fail/throw in some very specific and edge cases, hence, wrap it in try/catch.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <exception cref="ArgumentException"></exception>
    public static bool IsLikeSql(this string input, string pattern) {
      try {
        var isMatch = true;
        var isWildCardOn = false;
        var isCharWildCardOn = false;
        var isCharSetOn = false;
        var isNotCharSetOn = false;
        var endOfPattern = false;
        var lastWildCard = -1;
        var patternIndex = 0;
        var set = new List<char>();
        var p = '\0';
        for (var i = 0; i < input.Length; i++) {
          var c = input[i];
          endOfPattern = (patternIndex >= pattern.Length);
          if (!endOfPattern) {
            p = pattern[patternIndex];
            if (!isWildCardOn && p == '%') {
              lastWildCard = patternIndex;
              isWildCardOn = true;
              while (patternIndex < pattern.Length &&
                  pattern[patternIndex] == '%') {
                patternIndex++;
              }
              if (patternIndex >= pattern.Length) p = '\0';
              else p = pattern[patternIndex];
            } else if (p == '_') {
              isCharWildCardOn = true;
              patternIndex++;
            } else if (p == '[') {
              if (pattern[++patternIndex] == '^') {
                isNotCharSetOn = true;
                patternIndex++;
              } else isCharSetOn = true;
              set.Clear();
              if (pattern[patternIndex + 1] == '-' && pattern[patternIndex + 3] == ']') {
                var start = char.ToUpper(pattern[patternIndex]);
                patternIndex += 2;
                var end = char.ToUpper(pattern[patternIndex]);
                if (start <= end) {
                  for (var ci = start; ci <= end; ci++) {
                    set.Add(ci);
                  }
                }
                patternIndex++;
              }
              while (patternIndex < pattern.Length &&
        pattern[patternIndex] != ']') {
                set.Add(pattern[patternIndex]);
                patternIndex++;
              }
              patternIndex++;
            }
          }

          if (isWildCardOn) {
            if (char.ToUpper(c) == char.ToUpper(p)) {
              isWildCardOn = false;
              patternIndex++;
            }
          } else if (isCharWildCardOn) {
            isCharWildCardOn = false;
          } else if (isCharSetOn || isNotCharSetOn) {
            var charMatch = (set.Contains(char.ToUpper(c)));
            if ((isNotCharSetOn && charMatch) || (isCharSetOn && !charMatch)) {
              if (lastWildCard >= 0) patternIndex = lastWildCard;
              else {
                isMatch = false;
                break;
              }
            }
            isNotCharSetOn = isCharSetOn = false;
          } else {
            if (char.ToUpper(c) == char.ToUpper(p)) {
              patternIndex++;
            } else {
              if (lastWildCard >= 0) patternIndex = lastWildCard;
              else {
                isMatch = false;
                break;
              }
            }
          }
        }
        endOfPattern = patternIndex >= pattern.Length;
        if (isMatch && !endOfPattern) {
          var isOnlyWildCards = true;
          for (var i = patternIndex; i < pattern.Length; i++) {
            if (pattern[i] != '%') {
              isOnlyWildCards = false;
              break;
            }
          }
          if (isOnlyWildCards) endOfPattern = true;
        }
        return isMatch && endOfPattern;
      } catch (Exception) {
        throw new ArgumentException(pattern);
      }
    }



    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);


    //public static bool IsBool(this string value) => bool.TryParse(value, out var result);
    //public static bool IsDateTime(this string value) => DateTime.TryParse(value, out var result);
    //public static bool IsDecimal(this string value) => decimal.TryParse(value, out var result);
    //public static bool IsInt(this string value) => int.TryParse(value, out var result);
    //public static bool IsFloat(this string value) => float.TryParse(value, out var result);

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
      var result = target;
      while (result.StartsWith(trimString)) {
        result = result.Substring(trimString.Length);
      }
      return result;
    }

    public static string TrimEnd(this string target, string trimString) {
      if (string.IsNullOrEmpty(trimString)) return target;
      var result = target;
      while (result.EndsWith(trimString)) {
        result = result.Substring(0, result.Length - trimString.Length);
      }
      return result;
    }

    public static string WrapIfNotNullOrEmpty(this string s, string prefix = "", string suffix = "", string defaultIfNullOrEmpty = "") => string.IsNullOrEmpty(s) ? defaultIfNullOrEmpty : prefix + s + suffix;
    public static string WrapIfNotNullOrWhiteSpace(this string s, string prefix = "", string suffix = "", string defaultIfNullOrWhiteSpace = "") => string.IsNullOrWhiteSpace(s) ? defaultIfNullOrWhiteSpace : prefix + s + suffix;

    public static T ToEnum<T>(this string s, T defaultValue, bool ignoreCase = true) where T : struct => Enum.IsDefined(typeof(T), s) ? (T)Enum.Parse(typeof(T), s, ignoreCase) : defaultValue;
    //public static T? ToEnum<T>(this string value) where T : struct {
    //  var isParsed = Enum.TryParse(value, true, out T parsedValue);
    //  return isParsed ? parsedValue : null;
    //}

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
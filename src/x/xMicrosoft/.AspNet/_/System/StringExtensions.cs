using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System {
  public static class StringExtensions {

    public static bool IsDate(this string input) {
      DateTime temp;
      return DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out temp) &&
             temp.Hour == 0 && temp.Minute == 0 && temp.Second == 0 && temp.Millisecond == 0 && temp > DateTime.MinValue;
    }

    public static bool IsNumeric(this string input) => int.TryParse(input, out _);


    public static string Format(this IEnumerable<KeyValuePair<string, string>> replacements, string template) {
      if (string.IsNullOrWhiteSpace(template)) {
        return template;
      }
      var stringBuilder = new StringBuilder(template);
      if (replacements != null) {
        foreach (var replacement in replacements) {
          stringBuilder.Replace(replacement.Key, replacement.Value);
        }
      }
      return stringBuilder.ToString();
    }

    public static string FormatWith(this string format, params object[] args) {
      if (format == null) {
        throw new ArgumentNullException("format is null");
      }
      return string.Format(format, args);
    }

    public static string FromCssCaseToPascalCase(this string s) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant().Replace("-", " "));

    public static string FromPascalCase(this string pascalText, string stringBetweenWords = " ") {
      if (pascalText == null) {
        return "";
      }
      var sb = new StringBuilder();
      checked {
        var num = pascalText.Length - 1;
        for (var i = 0; i <= num; i++) {
          var a = pascalText[i];
          if (char.IsUpper(a) && i + 1 < pascalText.Length && !char.IsUpper(pascalText[i + 1])) {
            if (sb.Length > 0) {
              sb.Append(stringBetweenWords);
            }
            sb.Append(a);
          } else {
            sb.Append(a);
          }
        }
        return sb.ToString();
      }
    }

    public static string FromPascalCaseToCssCase(this string s) => s.FromPascalCase().Replace(" ", "-").ToLower();

    public static string IfNothing(this string value, string valueIfNothing) => (value == null) ? valueIfNothing : value;

    public static bool IsValidEmailAddress(this string email) => new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").IsMatch(email);

    public static bool IsValidUrl(this string text) => new Regex("http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?").IsMatch(text);



    public static IEnumerable<int> SplitAsInt(this string @this, string separator = ",") => (@this ?? string.Empty).Split(Convert.ToChar(separator))
      .Where(x => x.IsNumeric()).Select(x => Convert.ToInt32(x));

    public static List<DateTime> SplitToListOfDate(this string expr, string delimeter) {
      var list = new List<DateTime>();
      if (expr == null) { return list; }
      foreach (var s in expr.Split(delimeter)) {
        if (DateTime.TryParse(s, out var result)) {
          list.Add(result);
        }
      }
      return list;
    }

    public static List<decimal> SplitToListOfDecimal(this string expr, string delimeter) {
      var list = new List<decimal>();
      if (expr == null) { return list; }
      foreach (var s in expr.Split(delimeter)) {
        if (decimal.TryParse(s, out var result)) {
          list.Add(result);
        }
      }
      return list;
    }

    public static List<int> SplitToListOfInt(this string expr, string delimeter) {
      var list = new List<int>();
      if (expr == null) { return list; }
      foreach (var s in expr.Split(delimeter)) {
        if (int.TryParse(s, out var result)) {
          list.Add(result);
        }
      }
      return list;
    }

    public static List<string> SplitToListOfString(this string expr, string delimeter) {
      var list = new List<string>();
      if (expr == null) { return list; }
      foreach (var text in expr.Split(new char[] { Convert.ToChar(delimeter) })) {
        if (!string.IsNullOrWhiteSpace(text)) {
          list.Add(text);
        }
      }
      return list;
    }

    public static string ToProperCase(this string the_string) {
      var result = Regex.Replace(the_string, "(?<=\\w)(?=[A-Z])", " ", RegexOptions.None);
      return result.Substring(0, 1).ToUpper() + result.Substring(1);
    }

    public static string? ToSeparatedWords(this string value) => value != null ? Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim() : null;

    public static string Truncate(this string text, int maxLength, string suffix = "...") {
      var truncatedString = text;
      if (maxLength <= 0) {
        return truncatedString;
      }
      var strLength = checked(maxLength - suffix.Length);
      if (strLength <= 0) {
        return truncatedString;
      }
      if (text == null || text.Length <= maxLength) {
        return truncatedString;
      }
      truncatedString = text.Substring(0, strLength);
      truncatedString = truncatedString.TrimEnd();
      return truncatedString + suffix;
    }

  }
}
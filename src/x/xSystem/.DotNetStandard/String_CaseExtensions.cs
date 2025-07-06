using System.Text.RegularExpressions;

namespace System {
  public static class String_CaseExtensions {

    public static string ToCamelCase(this string input) {
      string text = input.ToPascalCase();
      return text.Length <= 0 ? text : text.Substring(0, 1).ToLower() + text.Substring(1);
    }

    public static string ToPascalCase(this string input)  => Regex.Replace(input, "(?:^|_| +)(.)", match => match.Groups[1].Value.ToUpper());

    //public static string? ToCamelCase(this string s) {
    //  if (s == null) return null;
    //  if (s.Length == 1) return s.ToLowerInvariant();
    //  var properCase = s.Replace("_", " ").ToProperCase().Replace(" ", string.Empty);
    //  return char.ToLowerInvariant(properCase[0]) + properCase.Substring(1);
    //}

    public static string ToProperCase(this string s) => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s);

    public static string ToSnakeCase(this string s) => ToSeparatedCase(s, '_');

    public static string ToKebabCase(this string s) => ToSeparatedCase(s, '-');

    private enum SeparatedCaseState {
      Start,
      Lower,
      Upper,
      NewWord
    }

    private static string ToSeparatedCase(string s, char separator) {
      if (string.IsNullOrEmpty(s)) {
        return s;
      }
      var sb = new StringBuilder();
      var state = SeparatedCaseState.Start;
      for (var i = 0; i < s.Length; i++) {
        if (s[i] == ' ') {
          if (state != SeparatedCaseState.Start) {
            state = SeparatedCaseState.NewWord;
          }
        } else if (char.IsUpper(s[i])) {
          switch (state) {
            case SeparatedCaseState.Upper:
              var hasNext = i + 1 < s.Length;
              if (i > 0 && hasNext) {
                var nextChar = s[i + 1];
                if (!char.IsUpper(nextChar) && nextChar != separator) {
                  sb.Append(separator);
                }
              }
              break;
            case SeparatedCaseState.Lower:
            case SeparatedCaseState.NewWord:
              sb.Append(separator);
              break;
          }
          char c;
#if HAVE_CHAR_TO_LOWER_WITH_CULTURE
                              c = char.ToLower(s[i], CultureInfo.InvariantCulture);
#else
          c = char.ToLowerInvariant(s[i]);
#endif
          sb.Append(c);
          state = SeparatedCaseState.Upper;
        } else if (s[i] == separator) {
          sb.Append(separator);
          state = SeparatedCaseState.Start;
        } else {
          if (state == SeparatedCaseState.NewWord) {
            sb.Append(separator);
          }
          sb.Append(s[i]);
          state = SeparatedCaseState.Lower;
        }
      }
      return sb.ToString();
    }

    //public static string ToTitleCase(this string value) => value.Titleize();
    //public static string ToTrainCase(this string value) => value.Dasherize();


  }
}

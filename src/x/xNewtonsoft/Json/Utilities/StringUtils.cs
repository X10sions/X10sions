using System.Text;

namespace Newtonsoft.Json.Utilities {
  public static class StringUtils {

    #region https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs

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
      StringBuilder sb = new StringBuilder();
      SeparatedCaseState state = SeparatedCaseState.Start;
      for (int i = 0; i < s.Length; i++) {
        if (s[i] == ' ') {
          if (state != SeparatedCaseState.Start) {
            state = SeparatedCaseState.NewWord;
          }
        } else if (char.IsUpper(s[i])) {
          switch (state) {
            case SeparatedCaseState.Upper:
              bool hasNext = (i + 1 < s.Length);
              if (i > 0 && hasNext) {
                char nextChar = s[i + 1];
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

    #endregion

  }
}

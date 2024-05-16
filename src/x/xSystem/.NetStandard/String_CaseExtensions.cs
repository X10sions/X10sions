namespace System {
  public static class String_CaseExtensions {
    #region https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs

    //public static string ToCamelCase(this string s) => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(s);
    //public static string ToCamelCase(this string s) => string.IsNullOrWhiteSpace(s)? s: char.ToLowerInvariant(s[0]) + s.Substring(1);
    //public static string? ToCamelCase(this string? s) => s is null ? null : ((string)s).ToCamelCase();
    public static string? ToCamelCase(this string s) {
      var words = s.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
      //var leadWord = words[0].ToLower();
      var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)", m => {
        return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
      });
      //var tailWords = words.Skip(1).Select(word => char.ToUpper(word[0]) + word.Substring(1)).ToArray();
      var tailWords = words.Skip(1).Select(word => word.ToProperCase());
      return $"{leadWord}{string.Join(string.Empty, tailWords)}";
    }

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

    #endregion

  }
}

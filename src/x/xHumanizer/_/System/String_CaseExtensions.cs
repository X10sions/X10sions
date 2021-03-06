﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace System {

  //  [Obsolete("Use Humanizer")]
  //  public enum ToStringCase {
  //    /// <summary>
  //    /// Words are linked without spaces. Each word begins with a capital letter with the exception of the first.
  //    /// </summary>
  //    /// <example>theQuickBrownFox</example>
  //    [Obsolete("Use Humanizer: .Camelize()")] CamelCase,
  //    /// <summary>
  //    /// kebab-case or spinal words are in lower case and are linked by hyphens (-)
  //    /// </summary>
  //    /// <example>the-quick-brown-fox</example>
  //    [Obsolete("Use Humanizer: .Kebaberize()")] KebabCase,
  //    /// <summary>
  //    /// Words are linked without spaces. Each word begins with a capital letter.
  //    /// </summary>
  //    /// <example>TheQuickBrownFox</example>
  //    [Obsolete("Use Humanizer: .Pascalize()")] PascalCase,
  //    /// <summary>
  //    /// snake_case or underscore words are in lower case and are linked by undescores (_).
  //    /// </summary>
  //    /// <example>the_quick_brown_fox</example>
  //    [Obsolete("Use Humanizer: .Underscore()")] SnakeCase,
  //    /// <summary>
  //    /// </summary>
  //    /// <example>The Quick Brown Fox</example>
  //    [Obsolete("Use Humanizer: .Titleize()")] TitleCase,
  //    /// <summary>
  //    /// </summary>
  //    /// <example>The-Quick-Brown-Fox</example>
  //    [Obsolete("Use Humanizer: .Dasherize() or .Hyphenate()")] TrainCase
  //  }

  [Obsolete("Use Humanizer")]
  public static class String_CaseExtensions {

    //    #region "LetterCasing"

    //    public static string ToCamelCase2(this string value) {
    //      if (value == null || value.Length < 2) return value;
    //      string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
    //      string result = words[0].ToLower();
    //      for (int i = 1; i < words.Length; i++) {
    //        result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
    //      }
    //      return result;
    //    }

    //    public static string ToCamelCase(this string value) => value.ToPascalCase().Substring(0, 1).ToLower() + value.Substring(1);

    //    public static string ToPascalCase(this string value) => string.Join(string.Empty, Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value).Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries));
    //    public static string ToSnakeCase(this string input) => string.IsNullOrEmpty(input) ? input : Regex.Match(input, "^_+") + Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2");

    //    public static string ToTitleCase(this string str) => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    //    public static string ToTitleCase(this string str, string cultureInfoName) => new CultureInfo(cultureInfoName).TextInfo.ToTitleCase(str.ToLower());
    //    public static string ToTitleCase(this string str, CultureInfo cultureInfo) => cultureInfo.TextInfo.ToTitleCase(str.ToLower());

    //    #endregion 

 
    #region https://github.com/vad3x/case-extensions

        private static readonly char[] CaseDelimeters = { ' ', '-', '_' };

      public static string ToCamelCase_Vad3x(this string source) => source.SymbolsPipe('\0', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { char.ToUpperInvariant(s) });
      public static string ToKebabCase_Vad3x(this string source) => source.SymbolsPipe('-', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { '-',  char.ToLowerInvariant(s) });
      public static string ToPascalCase_Vad3x(this string source) => source.SymbolsPipe('\0', (s, disableFrontDelimeter) => new char[] { char.ToUpperInvariant(s) });
      public static string ToSnakeCase_Vad3x(this string source) => source.SymbolsPipe('_', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { '_', char.ToLowerInvariant(s) });
      public static string ToTrainCase_Vad3x(this string source) => source.SymbolsPipe('-', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToUpperInvariant(s) } : new char[] { '-', char.ToUpperInvariant(s) });

    private static string SymbolsPipe(this string source, char mainDelimeter, Func<char, bool, char[]> newWordSymbolHandler) {
      if (source == null) throw new ArgumentNullException(nameof(source));
      var builder = new StringBuilder();
      var nextSymbolStartsNewWord = true;
      var disableFrontDelimeter = true;
      for (var i = 0; i < source.Length; i++) {
        var symbol = source[i];
        if (CaseDelimeters.Contains(symbol)) {
          if (symbol == mainDelimeter) {
            builder.Append(symbol);
            disableFrontDelimeter = true;
          }
          nextSymbolStartsNewWord = true;
        } else if (!char.IsLetterOrDigit(symbol)) {
          builder.Append(symbol);
          disableFrontDelimeter = true;
          nextSymbolStartsNewWord = true;
        } else if (nextSymbolStartsNewWord || char.IsUpper(symbol)) {
          builder.Append(newWordSymbolHandler(symbol, disableFrontDelimeter));
          disableFrontDelimeter = false;
          nextSymbolStartsNewWord = false;
        } else {
          builder.Append(symbol);
        }
      }
      return builder.ToString();
    }

    #endregion
  }
}
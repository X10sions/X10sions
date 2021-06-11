using System.Linq;
using System.Text;

namespace System {
  public static class String_CaseExtensions {
    #region https://github.com/vad3x/case-extensions

    private static readonly char[] CaseDelimeters = { ' ', '-', '_' };

    public static string xToCamelCase(this string source) => source.SymbolsPipe('\0', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { char.ToUpperInvariant(s) });
    public static string xToKebabCase(this string source) => source.SymbolsPipe('-', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { '-', char.ToLowerInvariant(s) });
    public static string xToPascalCase(this string source) => source.SymbolsPipe('\0', (s, disableFrontDelimeter) => new char[] { char.ToUpperInvariant(s) });
    public static string xToSnakeCase(this string source) => source.SymbolsPipe('_', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToLowerInvariant(s) } : new char[] { '_', char.ToLowerInvariant(s) });
    public static string xToTrainCase(this string source) => source.SymbolsPipe('-', (s, disableFrontDelimeter) => disableFrontDelimeter ? new char[] { char.ToUpperInvariant(s) } : new char[] { '-', char.ToUpperInvariant(s) });

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
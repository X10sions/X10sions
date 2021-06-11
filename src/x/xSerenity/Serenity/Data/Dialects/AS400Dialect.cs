using System;

namespace Serenity.Data.Dialects {
  public class AS400Dialect : ISqlDialect {

    public static readonly AS400Dialect Instance = new AS400Dialect();

    public bool CanUseOffsetFetch { get; } = false;
    public bool CanUseRowNumber { get; } = true;
    public bool CanUseSkipKeyword { get; } = false;
    public char CloseQuote => Convert.ToChar("\"");
    public string ConcatOperator { get; } = "||";
    public string DateFormat { get; } = "yyyy-MM-dd";
    public string DateTimeFormat => DateFormat + " " + TimeFormat + ".ffffff";
    public bool IsLikeCaseSensitive { get; } = true;
    public bool MultipleResultsets { get; } = false;
    public bool NeedsExecuteBlockStatement { get; } = false;
    public bool NeedsBoolWorkaround { get; } = false;
    public string OffsetFormat { get; } = $" {nameof(OffsetFormat)}";
    public string OffsetFetchFormat { get; } = $" {nameof(OffsetFetchFormat)}";
    public char OpenQuote => Convert.ToChar("\"");

    public string QuoteColumnAlias(string s) {
      return QuoteIdentifier(s);
    }

    public string QuoteIdentifier(string s) {
      if (string.IsNullOrEmpty(s)) {
        return s;
      }
      if (s.StartsWith(OpenQuote.ToString(),StringComparison.OrdinalIgnoreCase) && s.EndsWith(CloseQuote.ToString(), StringComparison.OrdinalIgnoreCase)) {
        return s;
      }
      return s;
    }
    public string QuoteUnicodeString(string s) {
      if (s.IndexOf('\'') >= 0)
        return $"N{OpenQuote}{s.Replace("'", "''")}{CloseQuote}";
      return $"N{OpenQuote}{s}{CloseQuote}";
    }
    public string ScopeIdentityExpression { get; } = "IDENTITY_VAL_LOCAL()";
    public string ServerType { get; } = "AS400";
    public string SkipKeyword { get; } = $" {nameof(SkipKeyword)}";
    public string TakeKeyword { get; } = $" Fetch {nameof(TakeKeyword)}";
    public string TimeFormat { get; } = "HH:mm:ss";
    public bool UseDateTime2 { get; } = false;
    public bool UseReturningIdentity { get; } = false;
    public bool UseReturningIntoVar { get; } = false;
    public bool UseScopeIdentity { get; } = true;
    public bool UseTakeAtEnd { get; } = false;
    public bool UseRowNum { get; } = false;
    public char ParameterPrefix { get; } = Convert.ToChar("@");

    public string UnionKeyword(SqlUnionType unionType) {
      switch (unionType) {
        case SqlUnionType.Union: return "UNION";
        case SqlUnionType.UnionAll: return "UNION ALL";
        case SqlUnionType.Intersect: return "INTERSECT";
        case SqlUnionType.Except: return "EXCEPT";
        default: throw new NotImplementedException();
      }
    }

  }
}
using RepoDb.Extensions;
using RepoDb.Interfaces;
using System.Linq;

namespace RepoDb {
  public static class StringExtensions {

    public static string AsJoinQualifier(this string value, string leftAlias, string rightAlias, IDbSetting dbSetting) => string.Concat(leftAlias, dbSetting.SchemaSeparator, value.AsQuoted(true, true, dbSetting), " = ", rightAlias, dbSetting.SchemaSeparator, value.AsQuoted(true, true, dbSetting));

    public static string AsQuoted(this string value, bool trim, bool ignoreSchema, IDbSetting dbSetting) {
      if (dbSetting == null) {
        return value;
      }
      if (ignoreSchema || value.IndexOf(dbSetting.SchemaSeparator) < 0) {
        return value.AsQuotedInternal(trim, dbSetting);
      } else {
        var splitted = value.Split(dbSetting?.SchemaSeparator.ToCharArray());
        return splitted.Select(s => s.AsQuotedInternal(trim, dbSetting)).Join(dbSetting.SchemaSeparator);
      }
    }

    public static string AsQuotedInternal(this string value, bool trim, IDbSetting dbSetting) {
      if (dbSetting == null) {
        return value;
      }
      if (trim) {
        value = value.Trim();
      }
      if (!value.StartsWith(dbSetting.OpeningQuote)) {
        value = string.Concat(dbSetting.OpeningQuote, value);
      }
      if (!value.EndsWith(dbSetting.ClosingQuote)) {
        value = string.Concat(value, dbSetting.ClosingQuote);
      }
      return value;
    }


  }
}

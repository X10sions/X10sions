using Common.Enums;

namespace Common.Sql {
  public static class SqlHelper {

    public const string DateFormat = "yyyy-MM-dd";
    public const string TimeFormat = "HH:mm:ss";
    public const string TimestampFormat = DateFormat + " " + TimeFormat;
    public const string NULL = "Null";
    public const string QualifierDateDefault = "'";
    public const string QualifierDateMsAccess = "#";
    public const string QualifierStringDefault = "'";

    public static string GetComparisonOperatorKeyWord(string op) {
      switch (op.Trim().ToLower()) {
        case "gt": return ">";
        case "ge": return ">=";
        case "eq": return "=";
        case "ne": return "<>";
        case "le": return "<=";
        case "lt": return "<";
        case "list": return "in";
        case "range": return "between";
        case "contains": return "like";
        case "beg": return "likestart";
        case "end": return "likeend";
        case "null": return "null";
      }
      return op;
    }

    // http://www.mikesdotnetting.com/article/227/migrating-classic-asp-to-asp-net-razor-web-pages-part-three-include-files-and-reusable-code

    public static string GetComparisonOperatorExpression(string keyword) {
      switch (keyword.Trim().ToLower()) {
        case "in": return "In({0})";
        case "between": return "Between {0} And {1}";
        case "like": return "Like '%{0}%'";
        case "likestart": return "Like '{0}%'";
        case "likeend": return "Like '%{0}'";
        case "null": return "Is Null";
        default:
          // > >= <> <= < =
          return keyword + " {0}";
      }
    }

    [Obsolete("Try remove")]
    public static string ParseSqlComparison(string prefix, string expr, string qry, string suffix, string qualifier) {
      if (qry.Length < 1) return string.Empty;
      var sql = "";
      var qrySplit = qry.Replace(":", ":::").Split(":::");
      //Enum.TryParse(qrySplit[0], out DB2WorkQuerySqlComparison db2Comparison);
      //var op = db2Comparison.GetSqlComparison().SqlFormat();
      var op = SqlComparisonExtensions.GetFromCode(qrySplit[0]).SqlFormat();
      var hasNot = op.IndexOf("not", StringComparison.OrdinalIgnoreCase) > 0;
      var sqlNot = "";
      if (hasNot) {
        op = op.Replace("not", "");
        sqlNot = "Not ";
      }
      var hasNull = op.IndexOf("null", StringComparison.OrdinalIgnoreCase) > 0;
      if (hasNull) {
        op = op.Replace("null", "");
        sql = sqlNot + expr + " Is Null";
      }
      // op = GetComparisonOperatorExpression(GetComparisonOperatorKeyWord(op))
      if (op.Length > 0) {
        var val = (qrySplit.Count() > 0 ? qrySplit[1] : string.Empty).Split(",");
        if (op.IndexOf("{1}", StringComparison.Ordinal) > 0) {
          if (val.Count() > 0)
            op = op.Replace("{0} And {1}", qualifier + val[0] + qualifier + " And " + qualifier + val[1] + qualifier);
        } else {
          if (op.IndexOf("'", StringComparison.Ordinal) > 0)
            qualifier = "";
          op = op.Replace("{0}", qualifier + string.Join(qualifier + ", " + qualifier, val) + qualifier);
        }
        op = sqlNot + expr + " " + op;
        if (sql.Length > 0)
          sql = "(" + sql + " Or " + op + ")";
        else
          sql = op;
      }
      return prefix + sql + suffix;
    }

    public static string QuoteCsv(string v) => "'" + v.Replace(", ", ",").Replace(",", "', '") + "'";
  }
}

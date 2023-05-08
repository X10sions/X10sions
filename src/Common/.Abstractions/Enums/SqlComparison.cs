using Common.Attributes;
using System.Text;

namespace Common.Enums {
  public enum SqlComparison {
    [SqlComparison("null", "Is Null", 0)] IsNull,
    [SqlComparison("notnull", "Is Not Null", 0)] NotNull,
    [SqlComparison("in", "In({0})")] InList,
    [SqlComparison("notin", "Not In({0})")] NotInList,
    [SqlComparison("<", "<{0}", 1)] LessThan,
    [SqlComparison("<=", "<={0}", 1)] LessOrEqualTo,
    [SqlComparison("=", "={0}", 1)] EqualTo,
    [SqlComparison(">=", ">={0}", 1)] MoreOrEqualTo,
    [SqlComparison(">", ">{0}", 1)] MoreThan,
    [SqlComparison("likestart", "Like '{0}%'", 1)] BeginsWith,
    [SqlComparison("like", "Like '%{0}%'", 1)] IsLike,
    [SqlComparison("notlike", "Not Like '%{0}%'", 1)] NotLike,
    [SqlComparison("likeend", "Like '%{0}'", 1)] EndsWith,
    [SqlComparison("between", "Between {0} And {1}", 2)] Between,
    [SqlComparison("<>", "<>{0}", 1)] NotEqualTo
  }

  public static class SqlComparisonExtensions {

    public static IEnumerable<SqlComparison> GetValues() => EnumExtensions.GetValuesOfType<SqlComparison>();

    public static SqlComparisonAttribute Attributes(this SqlComparison sqlComparison) => sqlComparison.GetAttribute<SqlComparisonAttribute>();
    public static string SqlFormat(this SqlComparison sqlComparison) => sqlComparison.Attributes().SqlFormat;
    //public static string ISeriesCode(this SqlComparison sqlComparison) => sqlComparison.Attributes().ISeriesCode;
    //public static SqlComparison GetFromISeriesCode(this string iSeriesCode) => (from x in GetValues() where x.ISeriesCode() == iSeriesCode select x).FirstOrDefault();


    public static string ToSqlString<T>(this SqlComparison sqlComparison,
      string sqlExpression,
      IEnumerable<T> selectedValues,
      string sqlValuePrefix = "",
      string sqlValueSuffix = "",
      bool isNot = false,
      SqlLogicOperator sqlAndOr = SqlLogicOperator.And
      ) {
      var hasSelectedValues = selectedValues.Count() > 0;
      if (!hasSelectedValues && sqlComparison != SqlComparison.IsNull) {
        return string.Empty;
      }
      var sql = new StringBuilder(" "
        + (sqlAndOr == SqlLogicOperator.Blank ? string.Empty : sqlAndOr.ToString()) + " "
        + (isNot ? " Not" : string.Empty)
        + sqlExpression);
      switch (sqlComparison) {
        case SqlComparison.IsNull: sql.Append($" Is Null"); break;
        case SqlComparison.Between: sql.Append($" Between {selectedValues.SqlLiteralBetween(sqlValuePrefix, sqlValueSuffix)}"); break;
        case SqlComparison.InList: sql.Append($" In({selectedValues.SqlLiteralIn(sqlValuePrefix, sqlValueSuffix)})"); break;
        case SqlComparison.MoreOrEqualTo: sql.Append($" >= {selectedValues.SqlLiteralMin(sqlValuePrefix, sqlValueSuffix)}"); break;
        case SqlComparison.MoreThan: sql.Append($" > {selectedValues.SqlLiteralMin(sqlValuePrefix, sqlValueSuffix)}"); break;
        case SqlComparison.LessOrEqualTo: sql.Append($" <= {selectedValues.SqlLiteralMax(sqlValuePrefix, sqlValueSuffix)}"); break;
        case SqlComparison.LessThan: sql.Append($" < {selectedValues.SqlLiteralMax(sqlValuePrefix, sqlValueSuffix)}"); break;
        default: throw new NotImplementedException($"Unknown sqlComparison: {sqlComparison}.");
      }
      return sql.ToString();
    }


    //public static List<SqlReportCompareType> SqlReportCompareTypeList() => new List<SqlReportCompareType> {
    //    new SqlReportCompareType(SqlComparison.InList, "InList", "In({0})", "in list", "list", -1),
    //    new SqlReportCompareType(SqlComparison.EqualTo, "Equals", "= {0}", "equal to", "eq", 1),
    //    new SqlReportCompareType(SqlComparison.Between, "Between", "Between {0} and {1}", "between", "range", 2),
    //    new SqlReportCompareType(SqlComparison.IsLike, "Like", "Like '%{0}%'", "containing text", "like", 1),
    //    new SqlReportCompareType(SqlComparison.BeginsWith, "BeginsWith", "Like '{0}%'", "begining with", "beg", 1),
    //    new SqlReportCompareType(SqlComparison.EndsWith, "EndsWith", "Like '%{0}'", "ending with", "end", 1),
    //    new SqlReportCompareType(SqlComparison.LessThan, "LessThan", "< {0}", "less than", "lt", 1),
    //    new SqlReportCompareType(SqlComparison.LessOrEqualTo, "LessOrEquals", "<= {0}", "less than or equal to", "le", 1),
    //    new SqlReportCompareType(SqlComparison.MoreOrEqualTo, "GreaterOrEquals", ">= {0}", "more than or equal to", "ge", 1),
    //    new SqlReportCompareType(SqlComparison.MoreThan, "GreaterThan", "> {0}", "more than", "gt", 1),
    //    new SqlReportCompareType(SqlComparison.IsNull, "Null", "Null", "empty", "null", 0)
    //  };

    public static string SqlFilter<T>(this SqlComparison compareOperator, string expression, bool isNot, params T[] values) {
      var sb = new StringBuilder(expression + " ");
      if (isNot) {
        sb.Append("Not ");
      }
      sb.Append(compareOperator.SqlFormat());
      switch (compareOperator) {
        case SqlComparison.IsNull:
        case SqlComparison.NotNull:
          return sb.ToString();
        case SqlComparison.Between:
          return string.Format(sb.ToString(), values.Min(), values.Max());
        case SqlComparison.NotInList:
        case SqlComparison.InList:
          return string.Format(sb.ToString(), string.Join(", ", values));
        default: return string.Format(sb.ToString(), values[0]);
      }
    }

    public static string Code(this SqlComparison @this) => Attributes(@this).Code;


    public static SqlComparison GetFromCode(string iSeriesCode) => (from x in Enum.GetValues(typeof(SqlComparison)).Cast<object>().Select(x => (SqlComparison)Convert.ToInt32(x))
                                                                    where Code(x).Equals(iSeriesCode, StringComparison.OrdinalIgnoreCase)
                                                                    select x).FirstOrDefault();

  }
}
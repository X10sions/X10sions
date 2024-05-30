using Common.Attributes;
using System.Text;

namespace Common.Enums;
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

  public static SqlComparisonAttribute Attributes(this SqlComparison sqlComparison) => sqlComparison.GetCustomAttribute<SqlComparisonAttribute>();
  public static string SqlFormat(this SqlComparison sqlComparison) => sqlComparison.Attributes().SqlFormat;

  public static string ToSqlString<T>(this SqlComparison sqlComparison,
    string sqlExpression,
    IEnumerable<T> selectedValues,
    string sqlValuePrefix = "",
    string sqlValueSuffix = "",
    bool isNot = false,
    SqlLogicOperator sqlAndOr = SqlLogicOperator.And
    ) {
    var hasSelectedValues = selectedValues.Any();
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
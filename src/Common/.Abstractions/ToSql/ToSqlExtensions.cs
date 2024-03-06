using System.Linq.Expressions;
using System.Text;

namespace Common.ToSql;

public static class ToSqlExtensions {

  public static string AddWhereToSelectCommand<T>(this Expression<Func<T, bool>> predicate, string _tableName, int maxCount = 0)
  => string.Format("{0} where {1}", CreateSelectCommand(_tableName, maxCount), CreateWhereClause(predicate));

  public static string CreateSelectCommand(string _tableName, int maxCount = 0) {
    var selectMax = maxCount > 0 ? "TOP " + maxCount.ToString() + " * " : "*";
    return string.Format("Select {0} from {1}", selectMax, _tableName);
  }

  public static string CreateWhereClause<T>(this Expression<Func<T, bool>> predicate) {
    var p = new StringBuilder(predicate.Body.ToString());
    var pName = predicate.Parameters.First();
    p.Replace(pName.Name + ".", "");
    p.Replace("==", "=");
    p.Replace("AndAlso", "and");
    p.Replace("OrElse", "or");
    p.Replace("\"", "\'");
    return p.ToString();
  }

  public static Expression StripQuotes(this Expression e) {
    while (e.NodeType == ExpressionType.Quote) {
      e = ((UnaryExpression)e).Operand;
    }
    return e;
  }

  public static string GetSqlOperator(this ExpressionType expressionType, bool isNullConstant) => expressionType switch {
    ExpressionType.And => " AND ",
    ExpressionType.AndAlso => " AND ",
    ExpressionType.Or => " OR ",
    ExpressionType.OrElse => " OR ",
    ExpressionType.Equal => isNullConstant ? " IS " : " = ",
    ExpressionType.NotEqual => isNullConstant ? " IS NOT " : " <> ",
    ExpressionType.LessThan => " < ",
    ExpressionType.LessThanOrEqual => " <= ",
    ExpressionType.GreaterThan => " > ",
    ExpressionType.GreaterThanOrEqual => " >= ",
    _ => throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", expressionType))
  };

}
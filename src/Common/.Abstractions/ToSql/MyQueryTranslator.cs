using System.Linq.Expressions;
using System.Text;

namespace Common.ToSql;

/// <summary>
/// https://stackoverflow.com/questions/7731905/how-to-convert-an-expression-tree-to-a-partial-sql-query
/// </summary>
/// <example>
/// var translator = new MyQueryTranslator();
/// string whereClause = translator.Translate(expression);
/// </example>
public class MyQueryTranslator : ExpressionVisitor {
  private StringBuilder sb;

  public int? Skip { get; private set; }
  public int? Take { get; private set; }
  public string OrderBy { get; private set; } = string.Empty;
  public string WhereClause { get; private set; } = string.Empty;

  public string Translate(Expression expression) {
    this.sb = new StringBuilder();
    this.Visit(expression);
    WhereClause = this.sb.ToString();
    return WhereClause;
  }

  protected override Expression VisitMethodCall(MethodCallExpression m) {
    if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where") {
      this.Visit(m.Arguments[0]);
      var lambda = (LambdaExpression)m.Arguments[1].StripQuotes();
      this.Visit(lambda.Body);
      return m;
    } else if (m.Method.Name == "Take") {
      if (this.ParseTakeExpression(m)) {
        var nextExpression = m.Arguments[0];
        return this.Visit(nextExpression);
      }
    } else if (m.Method.Name == "Skip") {
      if (this.ParseSkipExpression(m)) {
        var nextExpression = m.Arguments[0];
        return this.Visit(nextExpression);
      }
    } else if (m.Method.Name == "OrderBy") {
      if (this.ParseOrderByExpression(m, "ASC")) {
        var nextExpression = m.Arguments[0];
        return this.Visit(nextExpression);
      }
    } else if (m.Method.Name == "OrderByDescending") {
      if (this.ParseOrderByExpression(m, "DESC")) {
        var nextExpression = m.Arguments[0];
        return this.Visit(nextExpression);
      }
    }
    throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
  }

  protected override Expression VisitUnary(UnaryExpression u) {
    switch (u.NodeType) {
      case ExpressionType.Not:
        sb.Append(" NOT ");
        this.Visit(u.Operand);
        break;
      case ExpressionType.Convert:
        this.Visit(u.Operand);
        break;
      default:
        throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
    }
    return u;
  }

  protected override Expression VisitBinary(BinaryExpression b) {
    sb.Append("(");
    Visit(b.Left);
    sb.Append(b.NodeType.GetSqlOperator(IsNullConstant(b.Right)));
    Visit(b.Right);
    sb.Append(")");
    return b;
  }

  protected override Expression VisitConstant(ConstantExpression c) {
    var q = c.Value as IQueryable;
    if (q == null && c.Value == null) {
      sb.Append("NULL");
    } else if (q == null) {
      switch (Type.GetTypeCode(c.Value.GetType())) {
        case TypeCode.Boolean:
          sb.Append(((bool)c.Value) ? 1 : 0);
          break;
        case TypeCode.String:
          sb.Append("'");
          sb.Append(c.Value);
          sb.Append("'");
          break;
        case TypeCode.DateTime:
          sb.Append("'");
          sb.Append(c.Value);
          sb.Append("'");
          break;
        case TypeCode.Object:
          throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
        default:
          sb.Append(c.Value);
          break;
      }
    }
    return c;
  }

  protected override Expression VisitMember(MemberExpression m) {
    if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter) {
      sb.Append(m.Member.Name);
      return m;
    }
    throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
  }

  protected bool IsNullConstant(Expression exp) => (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);

  private bool ParseOrderByExpression(MethodCallExpression expression, string order) {
    var unary = (UnaryExpression)expression.Arguments[1];
    var lambdaExpression = (LambdaExpression)unary.Operand;
    lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);
    var body = lambdaExpression.Body as MemberExpression;
    if (body != null) {
      if (string.IsNullOrEmpty(OrderBy)) {
        OrderBy = string.Format("{0} {1}", body.Member.Name, order);
      } else {
        OrderBy = string.Format("{0}, {1} {2}", OrderBy, body.Member.Name, order);
      }
      return true;
    }
    return false;
  }

  private bool ParseTakeExpression(MethodCallExpression expression) {
    var sizeExpression = (ConstantExpression)expression.Arguments[1];
    int size;
    if (int.TryParse(sizeExpression.Value.ToString(), out size)) {
      Take = size;
      return true;
    }
    return false;
  }

  private bool ParseSkipExpression(MethodCallExpression expression) {
    var sizeExpression = (ConstantExpression)expression.Arguments[1];
    int size;
    if (int.TryParse(sizeExpression.Value.ToString(), out size)) {
      Skip = size;
      return true;
    }
    return false;
  }
}

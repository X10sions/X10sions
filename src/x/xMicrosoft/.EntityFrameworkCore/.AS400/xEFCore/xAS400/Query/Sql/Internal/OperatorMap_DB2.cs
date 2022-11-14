using System.Collections.Generic;
using System.Linq.Expressions;

namespace xEFCore.xAS400.Query.Sql.Internal {
  public class OperatorMap_DB2 : Dictionary<ExpressionType, string> {

    public OperatorMap_DB2() {
      this.Add(ExpressionType.Add, " + ");
      this.Add(ExpressionType.And, " AND ");
      this.Add(ExpressionType.AndAlso, " AND ");
      this.Add(ExpressionType.Divide, " / ");
      this.Add(ExpressionType.Equal, " = ");
      this.Add(ExpressionType.GreaterThan, " > ");
      this.Add(ExpressionType.GreaterThanOrEqual, " >= ");
      this.Add(ExpressionType.LessThan, " < ");
      this.Add(ExpressionType.LessThanOrEqual, " <= ");
      this.Add(ExpressionType.Modulo, " % ");
      this.Add(ExpressionType.Multiply, " * ");
      this.Add(ExpressionType.NotEqual, " <> ");
      this.Add(ExpressionType.Or, " OR ");
      this.Add(ExpressionType.OrElse, " OR ");
      this.Add(ExpressionType.Subtract, " - ");
    }

  }
}
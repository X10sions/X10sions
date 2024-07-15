using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace System.Data.Linq.SqlClient {
  /*
   * Simple CASE function:
   * CASE inputExpression 
   * WHEN whenExpression THEN resultExpression 
   * [ ...n ] 
   * [ 
   * ELSE elseResultExpression 
   * ] 
   * END 
   */
  internal class SqlSimpleCase : SqlExpression {
    private SqlExpression expression;
    private List<SqlWhen> whens = new List<SqlWhen>();

    internal SqlSimpleCase(Type clrType, SqlExpression expr, IEnumerable<SqlWhen> whens, Expression sourceExpression)
        : base(SqlNodeType.SimpleCase, clrType, sourceExpression) {
      Expression = expr;
      if (whens == null)
        throw Error.ArgumentNull("whens");
      this.whens.AddRange(whens);
      if (this.whens.Count == 0)
        throw Error.ArgumentOutOfRange("whens");
    }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (expression != null && expression.ClrType != value.ClrType)
          throw Error.ArgumentWrongType("value", expression.ClrType, value.ClrType);
        expression = value;
      }
    }

    internal List<SqlWhen> Whens => whens;

    internal override ProviderType SqlType => whens[0].Value.SqlType;
  }

}

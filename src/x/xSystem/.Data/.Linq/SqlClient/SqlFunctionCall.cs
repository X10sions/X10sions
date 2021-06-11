using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlFunctionCall : SqlSimpleTypeExpression {
    internal string Name { get; }
    internal List<SqlExpression> Arguments { get; }
    internal SqlFunctionCall(Type clrType, ProviderType sqlType, string name, IEnumerable<SqlExpression> args, Expression source)
      : this(SqlNodeType.FunctionCall, clrType, sqlType, name, args, source) {
    }
    internal SqlFunctionCall(SqlNodeType nodeType, Type clrType, ProviderType sqlType, string name, IEnumerable<SqlExpression> args, Expression source)
      : base(nodeType, clrType, sqlType, source) {
      Name = name;
      Arguments = new List<SqlExpression>(args);
    }
  }

}

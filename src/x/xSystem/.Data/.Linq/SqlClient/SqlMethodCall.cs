using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlMethodCall : SqlSimpleTypeExpression {
    private MethodInfo method;
    private SqlExpression obj;
    private List<SqlExpression> arguments;

    internal SqlMethodCall(Type clrType, ProviderType sqlType, MethodInfo method, SqlExpression obj, IEnumerable<SqlExpression> args, Expression sourceExpression)
        : base(SqlNodeType.MethodCall, clrType, sqlType, sourceExpression) {
      if (method == null)
        throw Error.ArgumentNull("method");
      this.method = method;
      Object = obj;
      arguments = new List<SqlExpression>();
      if (args != null)
        arguments.AddRange(args);
    }

    internal MethodInfo Method => method;

    internal SqlExpression Object {
      get => obj;
      set {
        if (value == null && !method.IsStatic)
          throw Error.ArgumentNull("value");
        if (value != null && !method.DeclaringType.IsAssignableFrom(value.ClrType))
          throw Error.ArgumentWrongType("value", method.DeclaringType, value.ClrType);
        obj = value;
      }
    }

    internal List<SqlExpression> Arguments => arguments;
  }

}

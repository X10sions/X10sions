using System.Data.Linq.SqlClient.Common;
using System.Reflection;

namespace System.Data.Linq.SqlClient {
  internal class SqlMemberAssign : SqlNode {
    private MemberInfo member;
    private SqlExpression expression;

    internal SqlMemberAssign(MemberInfo member, SqlExpression expr)
        : base(SqlNodeType.MemberAssign, expr.SourceExpression) {
      if (member == null)
        throw Error.ArgumentNull("member");
      this.member = member;
      Expression = expr;
    }

    internal MemberInfo Member => member;

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        expression = value;
      }
    }
  }

}

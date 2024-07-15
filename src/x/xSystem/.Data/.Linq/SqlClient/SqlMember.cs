using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Reflection;

namespace System.Data.Linq.SqlClient {
  internal class SqlMember : SqlSimpleTypeExpression {
    private SqlExpression expression;
    private MemberInfo member;

    internal SqlMember(Type clrType, ProviderType sqlType, SqlExpression expr, MemberInfo member)
        : base(SqlNodeType.Member, clrType, sqlType, expr.SourceExpression) {
      this.member = member;
      Expression = expr;
    }

    internal MemberInfo Member => member;

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (!member.ReflectedType.IsAssignableFrom(value.ClrType) &&
            !value.ClrType.IsAssignableFrom(member.ReflectedType))
          throw Error.MemberAccessIllegal(member, member.ReflectedType, value.ClrType);
        expression = value;
      }
    }
  }

}

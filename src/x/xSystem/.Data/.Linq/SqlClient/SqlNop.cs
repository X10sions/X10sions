using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlNop : SqlSimpleTypeExpression {
    internal SqlNop(Type clrType, ProviderType sqlType, Expression sourceExpression)
        : base(SqlNodeType.Nop, clrType, sqlType, sourceExpression) {
    }
  }

}

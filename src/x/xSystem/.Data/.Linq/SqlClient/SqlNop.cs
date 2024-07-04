using System.Linq.Expressions;
using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlNop : SqlSimpleTypeExpression {
    internal SqlNop(Type clrType, ProviderType sqlType, Expression sourceExpression)
        : base(SqlNodeType.Nop, clrType, sqlType, sourceExpression) {
    }
  }

}

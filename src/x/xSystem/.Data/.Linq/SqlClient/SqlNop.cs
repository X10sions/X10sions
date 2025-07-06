using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace System.Data.Linq.SqlClient {
  internal class SqlNop : SqlSimpleTypeExpression {
    internal SqlNop(Type clrType, ProviderType sqlType, Expression sourceExpression)
        : base(SqlNodeType.Nop, clrType, sqlType, sourceExpression) {
    }
  }

}

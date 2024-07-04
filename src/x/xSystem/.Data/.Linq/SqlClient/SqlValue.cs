using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlValue : SqlSimpleTypeExpression {
    private object value;
    private bool isClient;

    internal SqlValue(Type clrType, ProviderType sqlType, object value, bool isClientSpecified, Expression sourceExpression)
        : base(SqlNodeType.Value, clrType, sqlType, sourceExpression) {
      this.value = value;
      isClient = isClientSpecified;
    }

    internal object Value => value;

    internal bool IsClientSpecified => isClient;
  }

}

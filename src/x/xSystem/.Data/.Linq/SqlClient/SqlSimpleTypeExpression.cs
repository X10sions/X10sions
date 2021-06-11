using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal abstract class SqlSimpleTypeExpression : SqlExpression {
    private ProviderType sqlType;

    internal override ProviderType SqlType => sqlType;

    internal SqlSimpleTypeExpression(SqlNodeType nodeType, Type clrType, ProviderType sqlType, Expression sourceExpression)
      : base(nodeType, clrType, sourceExpression) {
      this.sqlType = sqlType;
    }

    internal void SetSqlType(ProviderType type) => sqlType = type;
  }

}

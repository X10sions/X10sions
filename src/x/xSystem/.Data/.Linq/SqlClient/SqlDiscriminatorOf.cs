using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlDiscriminatorOf : SqlSimpleTypeExpression {
    internal SqlExpression Object { get; set; }

    internal SqlDiscriminatorOf(SqlExpression obj, Type clrType, ProviderType sqlType, Expression sourceExpression)
      : base(SqlNodeType.DiscriminatorOf, clrType, sqlType, sourceExpression) {
      this.Object = obj;
    }
  }



}

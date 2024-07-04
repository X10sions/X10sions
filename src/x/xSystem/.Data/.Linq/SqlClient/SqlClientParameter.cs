using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlClientParameter : SqlSimpleTypeExpression {
    // Expression<Func<object[], T>>
    LambdaExpression accessor;
    internal SqlClientParameter(Type clrType, ProviderType sqlType, LambdaExpression accessor, Expression sourceExpression) :
        base(SqlNodeType.ClientParameter, clrType, sqlType, sourceExpression) {
      this.accessor = accessor;
    }
    internal LambdaExpression Accessor => accessor;
  }

}

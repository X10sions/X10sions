using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlLift : SqlExpression {
    internal SqlExpression liftedExpression;

    internal SqlLift(Type type, SqlExpression liftedExpression, Expression sourceExpression)
        : base(SqlNodeType.Lift, type, sourceExpression) {
      if (liftedExpression == null)
        throw Error.ArgumentNull("liftedExpression");
      this.liftedExpression = liftedExpression;
    }

    internal SqlExpression Expression {
      get => liftedExpression;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        liftedExpression = value;
      }
    }

    internal override ProviderType SqlType => liftedExpression.SqlType;
  }

}

using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlJoin : SqlSource {
    private SqlJoinType joinType;
    private SqlSource left;
    private SqlSource right;
    private SqlExpression condition;

    internal SqlJoin(SqlJoinType type, SqlSource left, SqlSource right, SqlExpression cond, Expression sourceExpression)
        : base(SqlNodeType.Join, sourceExpression) {
      JoinType = type;
      Left = left;
      Right = right;
      Condition = cond;
    }

    internal SqlJoinType JoinType {
      get => joinType;
      set => joinType = value;
    }

    internal SqlSource Left {
      get => left;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        left = value;
      }
    }

    internal SqlSource Right {
      get => right;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        right = value;
      }
    }

    internal SqlExpression Condition {
      get => condition;
      set => condition = value;
    }
  }

}

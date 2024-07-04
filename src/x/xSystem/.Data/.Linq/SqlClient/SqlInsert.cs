using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlInsert : SqlStatement {
    private SqlTable table;
    private SqlRow row;
    private SqlExpression expression;
    private SqlColumn outputKey;
    private bool outputToLocal;

    internal SqlInsert(SqlTable table, SqlExpression expr, Expression sourceExpression)
        : base(SqlNodeType.Insert, sourceExpression) {
      Table = table;
      Expression = expr;
      Row = new SqlRow(sourceExpression);
    }

    internal SqlTable Table {
      get => table;
      set {
        if (value == null)
          throw Error.ArgumentNull("null");
        table = value;
      }
    }

    internal SqlRow Row {
      get => row;
      set => row = value;
    }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value == null)
          throw Error.ArgumentNull("null");
        if (!table.RowType.Type.IsAssignableFrom(value.ClrType))
          throw Error.ArgumentWrongType("value", table.RowType, value.ClrType);
        expression = value;
      }
    }

    internal SqlColumn OutputKey {
      get => outputKey;
      set => outputKey = value;
    }

    internal bool OutputToLocal {
      get => outputToLocal;
      set => outputToLocal = value;
    }
  }

}

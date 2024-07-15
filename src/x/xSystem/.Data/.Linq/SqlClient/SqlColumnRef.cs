namespace System.Data.Linq.SqlClient {
  internal class SqlColumnRef : SqlExpression {
    internal SqlColumn Column { get; }

    internal override ProviderType SqlType => Column.SqlType;

    internal SqlColumnRef(SqlColumn col)
      : base(SqlNodeType.ColumnRef, col.ClrType, col.SourceExpression) {
      Column = col;
    }

    public override bool Equals(object obj) {
      var sqlColumnRef = obj as SqlColumnRef;
      if (sqlColumnRef != null) {
        return sqlColumnRef.Column == Column;
      }
      return false;
    }

    public override int GetHashCode() {
      return Column.GetHashCode();
    }

    internal SqlColumn GetRootColumn() {
      var sqlColumn = Column;
      while (sqlColumn.Expression != null && sqlColumn.Expression.NodeType == SqlNodeType.ColumnRef) {
        sqlColumn = ((SqlColumnRef)sqlColumn.Expression).Column;
      }
      return sqlColumn;
    }
  }

}

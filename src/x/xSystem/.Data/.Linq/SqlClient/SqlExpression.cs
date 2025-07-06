using System.Linq.Expressions;
using System.Data.Linq.SqlClient.Common;

namespace System.Data.Linq.SqlClient {
  internal abstract class SqlExpression : SqlNode {
    internal Type ClrType { get; private set; }

    internal abstract ProviderType SqlType { get; }

    internal bool IsConstantColumn {
      get {
        if (base.NodeType == SqlNodeType.Column) {
          var sqlColumn = (SqlColumn)this;
          if (sqlColumn.Expression != null) {
            return sqlColumn.Expression.IsConstantColumn;
          }
        } else {
          if (base.NodeType == SqlNodeType.ColumnRef) {
            return ((SqlColumnRef)this).Column.IsConstantColumn;
          }
          if (base.NodeType == SqlNodeType.OptionalValue) {
            return ((SqlOptionalValue)this).Value.IsConstantColumn;
          }
          if (base.NodeType == SqlNodeType.Value || base.NodeType == SqlNodeType.Parameter) {
            return true;
          }
        }
        return false;
      }
    }

    internal SqlExpression(SqlNodeType nodeType, Type clrType, Expression sourceExpression)
      : base(nodeType, sourceExpression) {
      ClrType = clrType;
    }

    internal void SetClrType(Type type) => ClrType = type;
  }

}

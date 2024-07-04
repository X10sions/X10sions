using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlUserRow : SqlSimpleTypeExpression {
    private SqlUserQuery query;
    private MetaType rowType;

    internal SqlUserRow(MetaType rowType, ProviderType sqlType, SqlUserQuery query, Expression source)
        : base(SqlNodeType.UserRow, rowType.Type, sqlType, source) {
      Query = query;
      this.rowType = rowType;
    }

    internal MetaType RowType => rowType;

    internal SqlUserQuery Query {
      get => query;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (value.Projection != null && value.Projection.ClrType != ClrType)
          throw Error.ArgumentWrongType("value", ClrType, value.Projection.ClrType);
        query = value;
      }
    }
  }

}

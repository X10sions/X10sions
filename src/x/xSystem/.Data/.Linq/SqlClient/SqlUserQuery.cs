using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlUserQuery : SqlNode {
    private string queryText;
    private SqlExpression projection;
    private List<SqlExpression> args;
    private List<SqlUserColumn> columns;

    internal SqlUserQuery(SqlNodeType nt, SqlExpression projection, IEnumerable<SqlExpression> args, Expression source)
        : base(nt, source) {
      Projection = projection;
      this.args = (args != null) ? new List<SqlExpression>(args) : new List<SqlExpression>();
      columns = new List<SqlUserColumn>();
    }

    internal SqlUserQuery(string queryText, SqlExpression projection, IEnumerable<SqlExpression> args, Expression source)
        : base(SqlNodeType.UserQuery, source) {
      this.queryText = queryText;
      Projection = projection;
      this.args = (args != null) ? new List<SqlExpression>(args) : new List<SqlExpression>();
      columns = new List<SqlUserColumn>();
    }

    internal string QueryText => queryText;

    internal SqlExpression Projection {
      get => projection;
      set {
        if (projection != null && projection.ClrType != value.ClrType)
          throw Error.ArgumentWrongType("value", projection.ClrType, value.ClrType);
        projection = value;
      }
    }

    internal List<SqlExpression> Arguments => args;

    internal List<SqlUserColumn> Columns => columns;

    internal SqlUserColumn Find(string name) {
      foreach (var c in Columns) {
        if (c.Name == name)
          return c;
      }
      return null;
    }
  }

}

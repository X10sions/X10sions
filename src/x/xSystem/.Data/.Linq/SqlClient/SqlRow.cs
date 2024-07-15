using System.Collections.Generic;
using System.Data.Linq.SqlClient.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlRow : SqlNode {
    internal List<SqlColumn> Columns { get; }

    internal SqlRow(Expression sourceExpression)
      : base(SqlNodeType.Row, sourceExpression) {
      Columns = new List<SqlColumn>();
    }

    internal SqlColumn Find(string name) {
      foreach (SqlColumn column in Columns) {
        if (name == column.Name) {
          return column;
        }
      }
      return null;
    }
  }

}

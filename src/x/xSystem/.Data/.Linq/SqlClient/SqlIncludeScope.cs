using System.Linq.Expressions;
using System.Data.Linq.SqlClient.Common;

namespace System.Data.Linq.SqlClient {
  internal class SqlIncludeScope : SqlNode {
    internal SqlNode Child { get; set; }

    internal SqlIncludeScope(SqlNode child, Expression sourceExpression)
      : base(SqlNodeType.IncludeScope, sourceExpression) {
      this.Child = child;
    }
  }


}

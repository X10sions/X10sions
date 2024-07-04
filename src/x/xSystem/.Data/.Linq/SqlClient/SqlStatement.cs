using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal abstract class SqlStatement : SqlNode {
    internal SqlStatement(SqlNodeType nodeType, Expression sourceExpression)
      : base(nodeType, sourceExpression) {
    }
  }

}

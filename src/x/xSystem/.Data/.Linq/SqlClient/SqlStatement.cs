using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Common;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal abstract class SqlStatement : SqlNode {
    internal SqlStatement(SqlNodeType nodeType, Expression sourceExpression)
      : base(nodeType, sourceExpression) {
    }
  }

}

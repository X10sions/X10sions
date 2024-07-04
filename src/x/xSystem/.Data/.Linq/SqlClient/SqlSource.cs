using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal abstract class SqlSource : SqlNode {
    internal SqlSource(SqlNodeType nt, Expression sourceExpression)
      : base(nt, sourceExpression) {
    }
  }

}

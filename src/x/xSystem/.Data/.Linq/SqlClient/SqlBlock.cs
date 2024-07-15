using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlBlock : SqlStatement {
    internal SqlBlock(Expression sourceExpression)
        : base(SqlNodeType.Block, sourceExpression) {
      Statements = new List<SqlStatement>();
    }

    internal List<SqlStatement> Statements { get; }
  }

}

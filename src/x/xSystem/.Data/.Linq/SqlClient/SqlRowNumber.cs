using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlRowNumber : SqlSimpleTypeExpression {
    private List<SqlOrderExpression> orderBy;

    internal List<SqlOrderExpression> OrderBy => orderBy;

    internal SqlRowNumber(Type clrType, ProviderType sqlType, List<SqlOrderExpression> orderByList, Expression sourceExpression)
        : base(SqlNodeType.RowNumber, clrType, sqlType, sourceExpression) {
      if (orderByList == null) {
        throw Error.ArgumentNull("orderByList");
      }

      orderBy = orderByList;
    }
  }

}

using System.Collections.Generic;

namespace System.Data.Linq.SqlClient {
  internal class SqlClientQuery : SqlSimpleTypeExpression {
    private SqlSubSelect query;
    private List<SqlExpression> arguments;
    private List<SqlParameter> parameters;
    int ordinal;

    internal SqlClientQuery(SqlSubSelect subquery)
        : base(SqlNodeType.ClientQuery, subquery.ClrType, subquery.SqlType, subquery.SourceExpression) {
      query = subquery;
      arguments = new List<SqlExpression>();
      parameters = new List<SqlParameter>();
    }

    internal SqlSubSelect Query {
      get => query;
      set {
        if (value == null || (query != null && query.ClrType != value.ClrType))
          throw Error.ArgumentWrongType(value, query.ClrType, value.ClrType);
        query = value;
      }
    }

    internal List<SqlExpression> Arguments => arguments;

    internal List<SqlParameter> Parameters => parameters;

    internal int Ordinal {
      get => ordinal;
      set => ordinal = value;
    }
  }

}

using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlDelete : SqlStatement {
    private SqlSelect select;

    internal SqlSelect Select {
      get => select;
      set => select = value ?? throw Error.ArgumentNull("value");
    }

    internal SqlDelete(SqlSelect select, Expression sourceExpression)
      : base(SqlNodeType.Delete, sourceExpression) {
      Select = select;
    }
  }



}

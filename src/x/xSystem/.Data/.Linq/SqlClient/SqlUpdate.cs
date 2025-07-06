using System.Data.Linq;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlUpdate : SqlStatement {
    private SqlSelect select;
    private List<SqlAssign> assignments;

    internal SqlUpdate(SqlSelect select, IEnumerable<SqlAssign> assignments, Expression sourceExpression)
        : base(SqlNodeType.Update, sourceExpression) {
      Select = select;
      this.assignments = new List<SqlAssign>(assignments);
    }

    internal SqlSelect Select {
      get => select;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        select = value;
      }
    }

    internal List<SqlAssign> Assignments => assignments;
  }

}

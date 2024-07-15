using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlUserColumn : SqlSimpleTypeExpression {
    private SqlUserQuery query;
    private string name;
    private bool isRequired;

    internal SqlUserColumn(Type clrType, ProviderType sqlType, SqlUserQuery query, string name, bool isRequired, Expression source)
        : base(SqlNodeType.UserColumn, clrType, sqlType, source) {
      Query = query;
      this.name = name;
      this.isRequired = isRequired;
    }

    internal SqlUserQuery Query {
      get => query;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (query != null && query != value)
          throw Error.ArgumentWrongValue("value");
        query = value;
      }
    }

    internal string Name => name;

    internal bool IsRequired => isRequired;
  }

}

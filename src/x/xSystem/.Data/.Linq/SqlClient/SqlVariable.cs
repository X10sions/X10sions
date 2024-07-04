using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlVariable : SqlSimpleTypeExpression {
    private string name;

    internal SqlVariable(Type clrType, ProviderType sqlType, string name, Expression sourceExpression)
        : base(SqlNodeType.Variable, clrType, sqlType, sourceExpression) {
      if (name == null)
        throw Error.ArgumentNull("name");
      this.name = name;
    }

    internal string Name => name;
  }

}

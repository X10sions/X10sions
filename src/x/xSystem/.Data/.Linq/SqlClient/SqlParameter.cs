using System.Data;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlParameter : SqlSimpleTypeExpression {
    internal SqlParameter(Type clrType, ProviderType sqlType, string name, Expression sourceExpression) : base(SqlNodeType.Parameter, clrType, sqlType, sourceExpression) {
      if (name == null)
        throw Error.ArgumentNull("name");
      if (typeof(Type).IsAssignableFrom(clrType))
        throw Error.ArgumentWrongValue("clrType");
      Name = name;
      Direction = ParameterDirection.Input;
    }

    internal string Name { get; }

    internal ParameterDirection Direction { get; set; }
  }

}

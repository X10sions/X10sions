using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlParameter : SqlSimpleTypeExpression {
    internal SqlParameter(Type clrType, ProviderType sqlType, string name, Expression sourceExpression) : base(SqlNodeType.Parameter, clrType, sqlType, sourceExpression) {
      if (name == null)
        throw Error.ArgumentNull("name");
      if (typeof(Type).IsAssignableFrom(clrType))
        throw Error.ArgumentWrongValue("clrType");
      Name = name;
      Direction = System.Data.ParameterDirection.Input;
    }

    internal string Name { get; }

    internal System.Data.ParameterDirection Direction { get; set; }
  }

}

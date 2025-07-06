using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace System.Data.Linq.Mapping;
internal class SqlParameter : SqlSimpleTypeExpression {
  private string name;

  internal SqlParameter(Type clrType, ProviderType sqlType, string name, Expression sourceExpression)
      : base(SqlNodeType.Parameter, clrType, sqlType, sourceExpression) {
    if (name == null)
      throw Error.ArgumentNull("name");
    if (typeof(Type).IsAssignableFrom(clrType))
      throw Error.ArgumentWrongValue("clrType");
    this.name = name;
    this.Direction = System.Data.ParameterDirection.Input;
  }

  internal string Name => this.name;

  internal System.Data.ParameterDirection Direction { get; set; }
}

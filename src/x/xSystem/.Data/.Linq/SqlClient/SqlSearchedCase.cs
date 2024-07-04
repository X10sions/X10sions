using System.Collections.Generic;
using System.Linq.Expressions;
using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  /*
   * Searched CASE function:
   * CASE
   * WHEN BooleanExpression THEN resultExpression 
   * [ ...n ] 
   * [ 
   * ELSE elseResultExpression 
   * ] 
   * END
   */
  internal class SqlSearchedCase : SqlExpression {
    private List<SqlWhen> whens;
    private SqlExpression @else;

    internal SqlSearchedCase(Type clrType, IEnumerable<SqlWhen> whens, SqlExpression @else, Expression sourceExpression)
        : base(SqlNodeType.SearchedCase, clrType, sourceExpression) {
      if (whens == null)
        throw Error.ArgumentNull("whens");
      this.whens = new List<SqlWhen>(whens);
      if (this.whens.Count == 0)
        throw Error.ArgumentOutOfRange("whens");
      Else = @else;
    }

    internal List<SqlWhen> Whens => whens;

    internal SqlExpression Else {
      get => @else;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (@else != null && !@else.ClrType.IsAssignableFrom(value.ClrType))
          throw Error.ArgumentWrongType("value", @else.ClrType, value.ClrType);
        @else = value;
      }
    }

    internal override ProviderType SqlType => whens[0].Value.SqlType;
  }

}

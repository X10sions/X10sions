using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlColumn : SqlExpression {
    private SqlExpression expression;
    private ProviderType sqlType;
    internal SqlAlias Alias { get; set; }
    internal string Name { get; set; }
    internal int Ordinal { get; set; }
    internal MetaDataMember MetaMember { get; }

    internal SqlExpression Expression {
      get => expression;
      set {
        if (value != null) {
          if (!base.ClrType.IsAssignableFrom(value.ClrType)) {
            throw System.Data.Linq.SqlClient.Error.ArgumentWrongType("value", base.ClrType, value.ClrType);
          }
          var sqlColumnRef = value as SqlColumnRef;
          if (sqlColumnRef != null && sqlColumnRef.Column == this) {
            throw System.Data.Linq.SqlClient.Error.ColumnCannotReferToItself();
          }
        }
        expression = value;
      }
    }

    internal override ProviderType SqlType {
      get {
        if (expression != null) {
          return expression.SqlType;
        }
        return sqlType;
      }
    }

    internal SqlColumn(Type clrType, ProviderType sqlType, string name, MetaDataMember member, SqlExpression expr, Expression sourceExpression)
      : base(SqlNodeType.Column, clrType, sourceExpression) {
      if (typeof(Type).IsAssignableFrom(clrType)) {
        throw System.Data.Linq.SqlClient.Error.ArgumentWrongValue("clrType");
      }
      Name = name;
      MetaMember = member;
      Expression = expr;
      Ordinal = -1;
      if (sqlType == null) {
        throw System.Data.Linq.SqlClient.Error.ArgumentNull("sqlType");
      }
      this.sqlType = sqlType;
    }

    internal SqlColumn(string name, SqlExpression expr)
      : this(expr.ClrType, expr.SqlType, name, null, expr, expr.SourceExpression) {
    }
  }

}

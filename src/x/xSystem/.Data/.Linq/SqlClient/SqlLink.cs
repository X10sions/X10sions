using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlLink : SqlSimpleTypeExpression {
    private MetaType rowType;
    private SqlExpression expression;
    private MetaDataMember member;
    private List<SqlExpression> keyExpressions;
    private SqlExpression expansion;
    private object id;

    internal SqlLink(object id, MetaType rowType, Type clrType, ProviderType sqlType, SqlExpression expression, MetaDataMember member, IEnumerable<SqlExpression> keyExpressions, SqlExpression expansion, Expression sourceExpression)
        : base(SqlNodeType.Link, clrType, sqlType, sourceExpression) {
      this.id = id;
      this.rowType = rowType;
      this.expansion = expansion;
      this.expression = expression;
      this.member = member;
      this.keyExpressions = new List<SqlExpression>();
      if (keyExpressions != null)
        this.keyExpressions.AddRange(keyExpressions);
    }

    internal MetaType RowType => rowType;

    internal SqlExpression Expansion {
      get => expansion;
      set => expansion = value;
    }


    internal SqlExpression Expression {
      get => expression;
      set => expression = value;
    }

    internal MetaDataMember Member => member;

    internal List<SqlExpression> KeyExpressions => keyExpressions;

    internal object Id => id;
  }

}

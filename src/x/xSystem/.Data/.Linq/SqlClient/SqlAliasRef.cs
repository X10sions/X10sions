using System.Data.Linq.SqlClient.Common;

namespace System.Data.Linq.SqlClient {
  internal class SqlAliasRef : SqlExpression {
    private SqlAlias alias;

    internal SqlAliasRef(SqlAlias alias)
        : base(SqlNodeType.AliasRef, GetClrType(alias.Node), alias.SourceExpression) {
      if (alias == null)
        throw Error.ArgumentNull("alias");
      this.alias = alias;
    }

    internal SqlAlias Alias => alias;

    internal override ProviderType SqlType => GetSqlType(alias.Node);

    private static Type GetClrType(SqlNode node) {
      var tvf = node as SqlTableValuedFunctionCall;
      if (tvf != null)
        return tvf.RowType.Type;
      var exp = node as SqlExpression;
      if (exp != null) {
        if (TypeSystem.IsSequenceType(exp.ClrType))
          return TypeSystem.GetElementType(exp.ClrType);
        return exp.ClrType;
      }
      var sel = node as SqlSelect;
      if (sel != null)
        return sel.Selection.ClrType;
      var tab = node as SqlTable;
      if (tab != null)
        return tab.RowType.Type;
      var su = node as SqlUnion;
      if (su != null)
        return su.GetClrType();
      throw Error.UnexpectedNode(node.NodeType);
    }

    private static ProviderType GetSqlType(SqlNode node) {
      var exp = node as SqlExpression;
      if (exp != null)
        return exp.SqlType;
      var sel = node as SqlSelect;
      if (sel != null)
        return sel.Selection.SqlType;
      var tab = node as SqlTable;
      if (tab != null)
        return tab.SqlRowType;
      var su = node as SqlUnion;
      if (su != null)
        return su.GetSqlType();
      throw Error.UnexpectedNode(node.NodeType);
    }
  }

}

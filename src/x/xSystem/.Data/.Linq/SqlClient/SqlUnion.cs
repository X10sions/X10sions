using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlUnion : SqlNode {
    private SqlNode left;
    private SqlNode right;
    private bool all;

    internal SqlUnion(SqlNode left, SqlNode right, bool all)
        : base(SqlNodeType.Union, right.SourceExpression) {
      Left = left;
      Right = right;
      All = all;
    }

    internal SqlNode Left {
      get => left;
      set {
        Validate(value);
        left = value;
      }
    }

    internal SqlNode Right {
      get => right;
      set {
        Validate(value);
        right = value;
      }
    }

    internal bool All {
      get => all;
      set => all = value;
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
    private void Validate(SqlNode node) {
      if (node == null)
        throw Error.ArgumentNull("node");
      if (!(node is SqlExpression || node is SqlSelect || node is SqlUnion))
        throw Error.UnexpectedNode(node.NodeType);
    }

    internal Type GetClrType() {
      var exp = Left as SqlExpression;
      if (exp != null)
        return exp.ClrType;
      var sel = Left as SqlSelect;
      if (sel != null)
        return sel.Selection.ClrType;
      throw Error.CouldNotGetClrType();
    }

    internal ProviderType GetSqlType() {
      var exp = Left as SqlExpression;
      if (exp != null)
        return exp.SqlType;
      var sel = Left as SqlSelect;
      if (sel != null)
        return sel.Selection.SqlType;
      throw Error.CouldNotGetSqlType();
    }
  }

}

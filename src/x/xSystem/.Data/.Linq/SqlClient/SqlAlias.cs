namespace System.Data.Linq.SqlClient {
  internal class SqlAlias : SqlSource {
    private SqlNode node;

    internal string Name { get; set; }

    internal SqlNode Node {
      get => node;
      set {
        if (value == null) {
          throw Error.ArgumentNull("value");
        }
        if (!(value is SqlExpression) && !(value is SqlSelect) && !(value is SqlTable) && !(value is SqlUnion)) {
          throw Error.UnexpectedNode(value.NodeType);
        }
        node = value;
      }
    }

    internal SqlAlias(SqlNode node)
      : base(SqlNodeType.Alias, node.SourceExpression) {
      Node = node;
    }
  }

}

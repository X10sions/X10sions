using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient.Common;

// SQL Client extensions to ExpressionType
internal enum InternalExpressionType {
  Known = 2000,
  LinkedTable = 2001
}

abstract internal class InternalExpression : Expression {
#pragma warning disable 618 // Disable the 'obsolete' warning.
  internal InternalExpression(InternalExpressionType nt, Type type) : base((ExpressionType)nt, type) {
  }
#pragma warning restore 618
  internal static KnownExpression Known(SqlExpression expr) {
    return new KnownExpression(expr, expr.ClrType);
  }
  internal static KnownExpression Known(SqlNode node, Type type) {
    return new KnownExpression(node, type);
  }
}

internal sealed class KnownExpression : InternalExpression {
  SqlNode node;
  internal KnownExpression(SqlNode node, Type type)      : base(InternalExpressionType.Known, type) {
    this.node = node;
  }
  internal SqlNode Node => this.node;
}

internal sealed class LinkedTableExpression : InternalExpression {
  private SqlLink link;
  private ITable table;
  internal LinkedTableExpression(SqlLink link, ITable table, Type type)
      : base(InternalExpressionType.LinkedTable, type) {
    this.link = link;
    this.table = table;
  }
  internal SqlLink Link => this.link;
  internal ITable Table => this.table;
}
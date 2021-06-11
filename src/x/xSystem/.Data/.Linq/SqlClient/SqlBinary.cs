using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Data.Linq.SqlClient {
  internal class SqlBinary : SqlSimpleTypeExpression {
    private SqlExpression left;
    private SqlExpression right;
    private MethodInfo method;

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
    internal SqlBinary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression left, SqlExpression right)
        : this(nt, clrType, sqlType, left, right, null) {
    }

    internal SqlBinary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression left, SqlExpression right, MethodInfo method)
        : base(nt, clrType, sqlType, right.SourceExpression) {
      switch (nt) {
        case SqlNodeType.Add:
        case SqlNodeType.Sub:
        case SqlNodeType.Mul:
        case SqlNodeType.Div:
        case SqlNodeType.Mod:
        case SqlNodeType.BitAnd:
        case SqlNodeType.BitOr:
        case SqlNodeType.BitXor:
        case SqlNodeType.And:
        case SqlNodeType.Or:
        case SqlNodeType.GE:
        case SqlNodeType.GT:
        case SqlNodeType.LE:
        case SqlNodeType.LT:
        case SqlNodeType.EQ:
        case SqlNodeType.NE:
        case SqlNodeType.EQ2V:
        case SqlNodeType.NE2V:
        case SqlNodeType.Concat:
        case SqlNodeType.Coalesce:
          break;
        default:
          throw Error.UnexpectedNode(nt);
      }
      Left = left;
      Right = right;
      this.method = method;
    }

    internal SqlExpression Left {
      get => left;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        left = value;
      }
    }

    internal SqlExpression Right {
      get => right;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        right = value;
      }
    }

    internal MethodInfo Method => method;
  }

}

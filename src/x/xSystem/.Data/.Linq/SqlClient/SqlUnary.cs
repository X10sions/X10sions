using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq.SqlClient {
  internal class SqlUnary : SqlSimpleTypeExpression {
    private SqlExpression operand;

    internal SqlExpression Operand {
      get => operand;
      set {
        if (value == null && NodeType != SqlNodeType.Count && NodeType != SqlNodeType.LongCount) {
          throw Error.ArgumentNull("value");
        }
        operand = value;
      }
    }

    internal MethodInfo Method { get; }

    internal SqlUnary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression expr, Expression sourceExpression)
      : this(nt, clrType, sqlType, expr, null, sourceExpression) {
    }

    internal SqlUnary(SqlNodeType nt, Type clrType, ProviderType sqlType, SqlExpression expr, MethodInfo method, Expression sourceExpression)
      : base(nt, clrType, sqlType, sourceExpression) {
      if (nt > SqlNodeType.Max) {
        switch (nt) {
          case SqlNodeType.New:
          case SqlNodeType.Not:
          case SqlNodeType.Not2V:
          case SqlNodeType.Nop:
          case SqlNodeType.Or:
          case SqlNodeType.OptionalValue:
          case SqlNodeType.OuterJoinedValue:
            if ((uint)(nt - 62) > 1u && nt != SqlNodeType.OuterJoinedValue) {
              break;
            }
            goto IL_0090;
          case SqlNodeType.Min:
          case SqlNodeType.Negate:
          case SqlNodeType.Stddev:
          case SqlNodeType.Sum:
          case SqlNodeType.Treat:
          case SqlNodeType.ValueOf:
            goto IL_0090;
        }
        goto IL_0084;
      }
      if (nt <= SqlNodeType.ClrLength) {
        if (nt != SqlNodeType.Avg && nt != SqlNodeType.BitNot && nt != SqlNodeType.ClrLength) {
          goto IL_0084;
        }
      } else if (nt <= SqlNodeType.IsNull) {
        if ((uint)(nt - 21) > 1u && (uint)(nt - 36) > 1u) {
          goto IL_0084;
        }
      } else if (nt != SqlNodeType.LongCount && nt != SqlNodeType.Max) {
        goto IL_0084;
      }
      goto IL_0090;
IL_0090:
      Operand = expr;
      Method = method;
      return;
IL_0084:
      throw Error.UnexpectedNode(nt);
    }
  }

}

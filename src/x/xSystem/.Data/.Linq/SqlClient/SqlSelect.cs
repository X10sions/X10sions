using System.Collections.Generic;
using System.Linq.Expressions;
using xSystem.Data.Linq.SqlClient;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlSelect : SqlStatement {
    private SqlExpression selection;
    private SqlRow row;
    private SqlExpression where;
    private SqlExpression having;
    internal SqlExpression Top { get; set; }
    internal bool IsPercent { get; set; }
    internal bool IsDistinct { get; set; }
    internal SqlExpression Selection { get => selection; set => selection = value ?? throw Error.ArgumentNull("value"); }
    internal SqlRow Row { get => row; set => row = value ?? throw Error.ArgumentNull("value"); }
    internal SqlSource From { get; set; }

    internal SqlExpression Where {
      get => where;
      set {
        if (value != null && TypeSystem.GetNonNullableType(value.ClrType) != typeof(bool)) {
          throw Error.ArgumentWrongType("value", "bool", value.ClrType);
        }
        where = value;
      }
    }

    internal List<SqlExpression> GroupBy { get; }

    internal SqlExpression Having {
      get => having;
      set {
        if (value != null && TypeSystem.GetNonNullableType(value.ClrType) != typeof(bool)) {
          throw Error.ArgumentWrongType("value", "bool", value.ClrType);
        }
        having = value;
      }
    }

    internal List<SqlOrderExpression> OrderBy { get; }

    internal SqlOrderingType OrderingType { get; set; }

    internal bool DoNotOutput { get; set; }

    internal SqlSelect(SqlExpression selection, SqlSource from, Expression sourceExpression)
      : base(SqlNodeType.Select, sourceExpression) {
      Row = new SqlRow(sourceExpression);
      Selection = selection;
      From = from;
      GroupBy = new List<SqlExpression>();
      OrderBy = new List<SqlOrderExpression>();
      OrderingType = SqlOrderingType.Default;
    }
  }

}

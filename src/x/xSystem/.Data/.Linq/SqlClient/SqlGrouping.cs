using System.Linq.Expressions;

namespace xSystem.Data.Linq.SqlClient {
  internal class SqlGrouping : SqlSimpleTypeExpression {
    private SqlExpression key;
    private SqlExpression group;

    internal SqlGrouping(Type clrType, ProviderType sqlType, SqlExpression key, SqlExpression group, Expression sourceExpression)
        : base(SqlNodeType.Grouping, clrType, sqlType, sourceExpression) {
      if (key == null) throw Error.ArgumentNull("key");
      if (group == null) throw Error.ArgumentNull("group");
      this.key = key;
      this.group = group;
    }

    internal SqlExpression Key {
      get => key;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (!key.ClrType.IsAssignableFrom(value.ClrType)
            && !value.ClrType.IsAssignableFrom(key.ClrType))
          throw Error.ArgumentWrongType("value", key.ClrType, value.ClrType);
        key = value;
      }
    }

    internal SqlExpression Group {
      get => group;
      set {
        if (value == null)
          throw Error.ArgumentNull("value");
        if (value.ClrType != group.ClrType)
          throw Error.ArgumentWrongType("value", group.ClrType, value.ClrType);
        group = value;
      }
    }
  }

}

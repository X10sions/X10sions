namespace System.Data.Linq.SqlClient;
/// <summary>
/// Represents one choice of object instantiation type in a type case.
/// When 'match' is the same as type case Discriminator then the corresponding
/// type binding is the one used for instantiation.
/// </summary>
internal class SqlTypeCaseWhen {
  private SqlExpression match;

  internal SqlExpression Match {
    get => match;
    set {
      if (match != null && value != null && match.ClrType != value.ClrType) {
        throw Error.ArgumentWrongType("value", match.ClrType, value.ClrType);
      }
      match = value;
    }
  }
  internal SqlExpression TypeBinding { get; set; }

  internal SqlTypeCaseWhen(SqlExpression match, SqlExpression typeBinding) {
    Match = match;
    TypeBinding = typeBinding;
  }

}

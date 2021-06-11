using Microsoft.Data.SqlClient;

namespace System.Data {
  public static class SqlDbTypeExtensions {

    public static SqlParameter ToNewSqlParameter(this SqlDbType @this) => new SqlParameter { SqlDbType = @this };
    public static DbType AsDbType(this SqlDbType @this) => @this.ToNewSqlParameter().DbType;

    public static Type AsType(this SqlDbType @this) {
      switch (@this) {
        case SqlDbType.VarBinary:
          return typeof(byte[]);
        case SqlDbType.UniqueIdentifier:
          return typeof(Guid?);
        case SqlDbType.DateTimeOffset:
          return typeof(DateTimeOffset);
        case SqlDbType.Time:
          return typeof(TimeSpan);
        default:
          throw new NotImplementedException($"Not Mapped yet: {@this.ToString()}");
      }
    }

    public static TypeCode AsTypeCode(this SqlDbType @this) {
      switch (@this) {
        case SqlDbType.Bit:
          return TypeCode.Boolean;
        case SqlDbType.TinyInt:
          return TypeCode.SByte;
        case SqlDbType.SmallInt:
          return TypeCode.Int16;
        case SqlDbType.Int:
          return TypeCode.Int32;
        case SqlDbType.BigInt:
          return TypeCode.Int64;
        case SqlDbType.Float:
          return TypeCode.Single;
        case SqlDbType.NVarChar:
          return TypeCode.String;
        case SqlDbType.NChar:
          return TypeCode.Char;
        case SqlDbType.DateTime:
          return TypeCode.DateTime;
        case SqlDbType.Decimal:
          return TypeCode.Decimal;
        default:
          throw new NotImplementedException($"Not Mapped yet: {@this.ToString()}");
      }
    }
  }
}

using System.Data.OleDb;

namespace System.Data {
  public static class SqlDbTypeExtensions {
    public static DbType AsDbType(this SqlDbType @this) {
      switch (@this) {
        case SqlDbType.BigInt:
          return DbType.Int64;
        case SqlDbType.Binary:
          return DbType.Binary;
        case SqlDbType.Bit:
          return DbType.Boolean;
        case SqlDbType.Char:
          return DbType.AnsiStringFixedLength;
        case SqlDbType.Date:
          return DbType.Date;
        case SqlDbType.DateTime:
          return DbType.DateTime;
        case SqlDbType.SmallDateTime:
          return DbType.DateTime;
        case SqlDbType.DateTime2:
          return DbType.DateTime2;
        case SqlDbType.DateTimeOffset:
          return DbType.DateTimeOffset;
        case SqlDbType.Decimal:
          return DbType.Decimal;
        case SqlDbType.Float:
          return DbType.Double;
        case SqlDbType.Real:
          return DbType.Double;
        case SqlDbType.Image:
          return DbType.Binary;
        case SqlDbType.Int:
          return DbType.Int32;
        case SqlDbType.Money:
          return DbType.Currency;
        case SqlDbType.SmallMoney:
          return DbType.Currency;
        case SqlDbType.NChar:
          return DbType.StringFixedLength;
        case SqlDbType.NText:
          return DbType.String;
        case SqlDbType.NVarChar:
          return DbType.String;
        case SqlDbType.SmallInt:
          return DbType.Int16;
        case SqlDbType.Text:
          return DbType.AnsiString;
        case SqlDbType.Time:
          return DbType.Time;
        case SqlDbType.Timestamp:
          return DbType.Binary;
        case SqlDbType.TinyInt:
          return DbType.SByte;
        case SqlDbType.Udt:
          return DbType.Object;
        case SqlDbType.UniqueIdentifier:
          return DbType.Guid;
        case SqlDbType.VarBinary:
          return DbType.Binary;
        case SqlDbType.VarChar:
          return DbType.AnsiString;
        case SqlDbType.Variant:
          return DbType.Object;
        case SqlDbType.Xml:
          return DbType.String;
        default:
          throw new NotImplementedException($"Not Mapped yet: {@this.ToString()}");
      }
    }

    public static OleDbType AsOleDbType(this SqlDbType @this) {
      switch (@this) {
        case SqlDbType.BigInt:
          return OleDbType.BigInt;
        case SqlDbType.Binary:
          return OleDbType.Binary;
        case SqlDbType.Bit:
          return OleDbType.Boolean;
        case SqlDbType.Char:
          return OleDbType.Char;
        case SqlDbType.Date:
          return OleDbType.Date;
        case SqlDbType.DateTime:
          return OleDbType.DBTimeStamp;
        case SqlDbType.DateTime2:
          return OleDbType.DBTimeStamp;
        case SqlDbType.DateTimeOffset:
          return OleDbType.DBTimeStamp;
        case SqlDbType.SmallDateTime:
          return OleDbType.DBTimeStamp;
        case SqlDbType.Decimal:
          return OleDbType.Decimal;
        case SqlDbType.Float:
          return OleDbType.Double;
        case SqlDbType.Real:
          return OleDbType.Double;
        case SqlDbType.Image:
          return OleDbType.LongVarBinary;
        case SqlDbType.Int:
          return OleDbType.Integer;
        case SqlDbType.Money:
          return OleDbType.Currency;
        case SqlDbType.SmallMoney:
          return OleDbType.Currency;
        case SqlDbType.NChar:
          return OleDbType.WChar;
        case SqlDbType.NText:
          return OleDbType.LongVarChar;
        case SqlDbType.NVarChar:
          return OleDbType.VarWChar;
        case SqlDbType.SmallInt:
          return OleDbType.SmallInt;
        case SqlDbType.Text:
          return OleDbType.LongVarChar;
        case SqlDbType.Time:
          return OleDbType.DBTime;
        case SqlDbType.Timestamp:
          return OleDbType.Binary;
        case SqlDbType.TinyInt:
          return OleDbType.TinyInt;
        case SqlDbType.Udt:
          return OleDbType.Variant;
        case SqlDbType.UniqueIdentifier:
          return OleDbType.Guid;
        case SqlDbType.VarBinary:
          return OleDbType.VarBinary;
        case SqlDbType.VarChar:
          return OleDbType.VarChar;
        case SqlDbType.Variant:
          return OleDbType.Variant;
        case SqlDbType.Xml:
          return OleDbType.VarWChar;
        default:
          throw new NotImplementedException($"Not Mapped yet: {@this.ToString()}");
      }
    }

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

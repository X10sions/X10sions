using System;
using System.Data;

namespace IQToolkit.Data.Ado {
  public static class DbTypeHelper {
    public static DbType ToDbType(this SqlDbType sqlDbType) {
      switch (sqlDbType) {
        case SqlDbType.BigInt: return DbType.Int64;
        case SqlDbType.Binary: return DbType.Binary;
        case SqlDbType.Bit: return DbType.Boolean;
        case SqlDbType.Char: return DbType.AnsiStringFixedLength;
        case SqlDbType.Date: return DbType.Date;
        case SqlDbType.DateTime:
        case SqlDbType.SmallDateTime: return DbType.DateTime;
        case SqlDbType.DateTime2: return DbType.DateTime2;
        case SqlDbType.DateTimeOffset: return DbType.DateTimeOffset;
        case SqlDbType.Decimal: return DbType.Decimal;
        case SqlDbType.Float:
        case SqlDbType.Real: return DbType.Double;
        case SqlDbType.Image: return DbType.Binary;
        case SqlDbType.Int: return DbType.Int32;
        case SqlDbType.Money:
        case SqlDbType.SmallMoney: return DbType.Currency;
        case SqlDbType.NChar: return DbType.StringFixedLength;
        case SqlDbType.NText:
        case SqlDbType.NVarChar: return DbType.String;
        case SqlDbType.SmallInt: return DbType.Int16;
        case SqlDbType.Text: return DbType.AnsiString;
        case SqlDbType.Time: return DbType.Time;
        case SqlDbType.Timestamp: return DbType.Binary;
        case SqlDbType.TinyInt: return DbType.SByte;
        case SqlDbType.Udt: return DbType.Object;
        case SqlDbType.UniqueIdentifier: return DbType.Guid;
        case SqlDbType.VarBinary: return DbType.Binary;
        case SqlDbType.VarChar: return DbType.AnsiString;
        case SqlDbType.Variant: return DbType.Object;
        case SqlDbType.Xml: return DbType.String;
        default:
          throw new InvalidOperationException(string.Format("Unhandled sqlDbType: {0}", sqlDbType));
      }
    }
  }
}
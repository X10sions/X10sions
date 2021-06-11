using System;
using System.Data;
using System.Data.Odbc;

namespace IQToolkit.Data.Odbc {
  //public class TypeMap {
  //  public SqlDbType SqlDbType { get; }
  //  public DbType DbType { get; }
  //  public OdbcType OdbcType { get; }
  //}

  public static class _Extensions {

    public static OdbcType ToOdbcType(this SqlDbType dbType) {
      switch (dbType) {
        case SqlDbType.BigInt: return OdbcType.BigInt;
        case SqlDbType.Binary: return OdbcType.Binary;
        case SqlDbType.Bit: return OdbcType.Bit;
        case SqlDbType.Char: return OdbcType.Char;
        case SqlDbType.Date: return OdbcType.Date;
        case SqlDbType.DateTime: return OdbcType.DateTime;
        case SqlDbType.DateTime2: return OdbcType.DateTime;
        case SqlDbType.DateTimeOffset: return OdbcType.DateTime;
        case SqlDbType.Decimal: return OdbcType.Decimal;
        case SqlDbType.Float: return OdbcType.Double;
        case SqlDbType.Image: return OdbcType.Image;
        case SqlDbType.Int: return OdbcType.Int;
        case SqlDbType.Money:
        //case SqlDbType.SmallMoney: return OdbcType.Currency;
        case SqlDbType.NChar: return OdbcType.NChar;
        case SqlDbType.NText: return OdbcType.NText;
        case SqlDbType.NVarChar: return OdbcType.NVarChar;
        case SqlDbType.Real: return OdbcType.Real;
        case SqlDbType.SmallDateTime: return OdbcType.DateTime;
        case SqlDbType.SmallInt: return OdbcType.SmallInt;
        case SqlDbType.Text: return OdbcType.Text;
        case SqlDbType.Time: return OdbcType.Time;
        case SqlDbType.Timestamp: return OdbcType.Timestamp;
        case SqlDbType.TinyInt: return OdbcType.TinyInt;
        case SqlDbType.Udt: return OdbcType.NVarChar;
        case SqlDbType.UniqueIdentifier: return OdbcType.UniqueIdentifier;
        case SqlDbType.VarBinary: return OdbcType.VarBinary;
        case SqlDbType.VarChar: return OdbcType.VarChar;
        case SqlDbType.Variant: return OdbcType.NVarChar;
        case SqlDbType.Xml: return OdbcType.NVarChar;
        default:
          throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", dbType));
      }

    }

    public static OdbcType ToOdbcType(this DbType type) {
      switch (type) {
        case DbType.AnsiString: return OdbcType.VarChar;
        case DbType.AnsiStringFixedLength: return OdbcType.Char;
        case DbType.Binary: return OdbcType.Binary;
        case DbType.Boolean: return OdbcType.Bit;
        case DbType.Byte: return OdbcType.TinyInt;
        //case DbType.Currency: return OdbcType.Currency;
        case DbType.Date: return OdbcType.Date;
        case DbType.DateTime:
        case DbType.DateTime2:
        case DbType.DateTimeOffset: return OdbcType.DateTime;
        case DbType.Decimal: return OdbcType.Decimal;
        case DbType.Double: return OdbcType.Double;
        case DbType.Guid: return OdbcType.UniqueIdentifier;
        case DbType.Int16: return OdbcType.SmallInt;
        case DbType.Int32: return OdbcType.Int;
        case DbType.Int64: return OdbcType.BigInt;
        case DbType.Object: return OdbcType.NVarChar;
        case DbType.SByte: return OdbcType.TinyInt;
        case DbType.Single: return OdbcType.Real;
        case DbType.String: return OdbcType.NVarChar;
        case DbType.StringFixedLength: return OdbcType.NChar;
        case DbType.Time: return OdbcType.Time;
        case DbType.UInt16: return OdbcType.Int;
        case DbType.UInt32: return OdbcType.BigInt;
        case DbType.UInt64: return OdbcType.Numeric;
        case DbType.VarNumeric: return OdbcType.Numeric;
        case DbType.Xml: return OdbcType.NVarChar;
        default:
          throw new InvalidOperationException(string.Format("Unhandled db type '{0}'.", type));
      }
    }

  }
}
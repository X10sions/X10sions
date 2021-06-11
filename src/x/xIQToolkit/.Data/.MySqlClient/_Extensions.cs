extern alias MySqlConnector;
extern alias MySqlData;
using MySqlData::MySql.Data.MySqlClient;
using System;
using System.Data;

namespace IQToolkit.Data.MySqlClient {
  public static class _Extensions {

    public static MySqlDbType ToMySqlDbType(this SqlDbType dbType) {
      switch (dbType) {
        case SqlDbType.BigInt: return MySqlDbType.Int64;
        case SqlDbType.Binary: return MySqlDbType.Binary;
        case SqlDbType.Bit: return MySqlDbType.Bit;
        case SqlDbType.NChar:
        case SqlDbType.Char: return MySqlDbType.Text;
        case SqlDbType.Date: return MySqlDbType.Date;
        case SqlDbType.DateTime:
        case SqlDbType.SmallDateTime: return MySqlDbType.DateTime;
        case SqlDbType.Decimal: return MySqlDbType.Decimal;
        case SqlDbType.Float: return MySqlDbType.Float;
        case SqlDbType.Image: return MySqlDbType.LongBlob;
        case SqlDbType.Int: return MySqlDbType.Int32;
        case SqlDbType.Money:
        case SqlDbType.SmallMoney: return MySqlDbType.Decimal;
        case SqlDbType.NVarChar:
        case SqlDbType.VarChar: return MySqlDbType.VarChar;
        case SqlDbType.SmallInt: return MySqlDbType.Int16;
        case SqlDbType.NText:
        case SqlDbType.Text: return MySqlDbType.LongText;
        case SqlDbType.Time: return MySqlDbType.Time;
        case SqlDbType.Timestamp: return MySqlDbType.Timestamp;
        case SqlDbType.TinyInt: return MySqlDbType.Byte;
        case SqlDbType.UniqueIdentifier: return MySqlDbType.Guid;
        case SqlDbType.VarBinary: return MySqlDbType.VarBinary;
        case SqlDbType.Xml: return MySqlDbType.Text;
        default: throw new NotSupportedException(string.Format("The SQL type '{0}' is not supported", dbType));
      }
    }

  }
}
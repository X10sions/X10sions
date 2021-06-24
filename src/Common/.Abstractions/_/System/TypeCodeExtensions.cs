using Common.Constants;
using System.Data;
using System.Linq;

namespace System {
  public static class TypeCodeExtensions {

    public static bool IsInteger(this TypeCode typeCode) => TypeCodeConstants.Integer.Contains(typeCode);
    public static bool IsFloat(this TypeCode typeCode) => TypeCodeConstants.Float.Contains(typeCode);
    public static bool IsNumeric(this TypeCode typeCode) => TypeCodeConstants.Numeric.Contains(typeCode);
    public static bool IsText(this TypeCode typeCode) => TypeCodeConstants.Text.Contains(typeCode);

    public static SqlDbType AsSqlDbType(this TypeCode typeCode) => typeCode switch {
      TypeCode.Boolean => SqlDbType.Bit,
      TypeCode.SByte => SqlDbType.TinyInt,
      TypeCode.Byte => SqlDbType.TinyInt,
      TypeCode.Int16 => SqlDbType.SmallInt,
      TypeCode.UInt16 => SqlDbType.SmallInt,
      TypeCode.Int32 => SqlDbType.Int,
      TypeCode.UInt32 => SqlDbType.Int,
      TypeCode.Int64 => SqlDbType.BigInt,
      TypeCode.UInt64 => SqlDbType.BigInt,
      TypeCode.Single => SqlDbType.Float,
      TypeCode.Double => SqlDbType.Float,
      TypeCode.String => SqlDbType.NVarChar,
      TypeCode.Char => SqlDbType.NChar,
      TypeCode.DateTime => SqlDbType.DateTime,
      TypeCode.Decimal => SqlDbType.Decimal,
      _ => throw new NotImplementedException($"Not Mapped yet: {typeCode}")
    };

  }
}
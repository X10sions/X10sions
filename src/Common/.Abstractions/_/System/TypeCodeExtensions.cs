using Common.Constants;
using System.Linq;

namespace System {
  public static class TypeCodeExtensions {

    public static bool IsInteger(this TypeCode typeCode) => TypeCodeConstants.Integer.Contains(typeCode);
    public static bool IsFloat(this TypeCode typeCode) => TypeCodeConstants.Float.Contains(typeCode);
    public static bool IsNumeric(this TypeCode typeCode) => TypeCodeConstants.Numeric.Contains(typeCode);
    public static bool IsText(this TypeCode typeCode) => TypeCodeConstants.Text.Contains(typeCode);

    [Obsolete("VB replace with: ")]
    public static bool IsOldNumeric(this TypeCode typeCode) => TypeCodeConstants.NumericVB.Contains(typeCode);


    //    <Extension>
    //Public Function AsSqlDbType(this As TypeCode) As SqlDbType
    //  Select Case this
    //    Case TypeCode.Boolean : Return SqlDbType.Bit
    //    Case TypeCode.SByte : Return SqlDbType.TinyInt
    //    Case TypeCode.Byte : Return SqlDbType.TinyInt
    //    Case TypeCode.Int16 : Return SqlDbType.SmallInt
    //    Case TypeCode.UInt16 : Return SqlDbType.SmallInt
    //    Case TypeCode.Int32 : Return SqlDbType.Int
    //    Case TypeCode.UInt32 : Return SqlDbType.Int
    //    Case TypeCode.Int64 : Return SqlDbType.BigInt
    //    Case TypeCode.UInt64 : Return SqlDbType.BigInt
    //    Case TypeCode.Single : Return SqlDbType.Float
    //    Case TypeCode.Double : Return SqlDbType.Float
    //    Case TypeCode.String : Return SqlDbType.NVarChar
    //    Case TypeCode.Char : Return SqlDbType.NChar
    //    Case TypeCode.DateTime : Return SqlDbType.DateTime
    //    Case TypeCode.Decimal : Return SqlDbType.Decimal
    //    Case Else : Throw New NotImplementedException($"Not Mapped yet: {this.ToString}")
    //  End Select
    //End Function


  }
}
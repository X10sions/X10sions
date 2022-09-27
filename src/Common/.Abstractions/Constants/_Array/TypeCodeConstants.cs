namespace Common.Constants {
  public static class TypeCodeConstants {

    public static readonly TypeCode[] Float =  {
      TypeCode.Double,
      TypeCode.Decimal,
      TypeCode.Single
    };

    public static readonly TypeCode[] Integer = {
      TypeCode.Byte,
      TypeCode.Int16,
      TypeCode.Int32,
      TypeCode.Int64,
      TypeCode.SByte,
      TypeCode.UInt16,
      TypeCode.UInt32,
      TypeCode.UInt64
    };

    public static readonly TypeCode[] Numeric = Integer.Union(Float).ToArray();

    public static readonly TypeCode[] Text = {
      TypeCode.Char,
      TypeCode.String
    };

  }
}
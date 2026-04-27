namespace Common.Constants;

public static class TypeConstants {
  public static readonly Type[] Short = [typeof(byte), typeof(char), typeof(short)];
  public static readonly Type[] UShort = [.. Short, typeof(ushort)];
  public static readonly Type[] Int = [.. UShort, typeof(int), typeof(sbyte)];
  public static readonly Type[] UInt = [.. Int, typeof(uint)];
  public static readonly Type[] Long = [.. UInt, typeof(long)];
  public static readonly Type[] ULong = [.. Long, typeof(ulong)];
  public static readonly Type[] Decimal = [.. ULong, typeof(decimal)];
  public static readonly Type[] Double = [.. ULong, typeof(double), typeof(float)];
  public static readonly Type[] Float = Double;
}

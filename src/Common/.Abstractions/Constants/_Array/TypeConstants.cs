using System;
using System.Linq;

namespace Common.Constants {     
  public static class TypeConstants {

    public static readonly Type[] Short = { typeof(byte), typeof(char), typeof(short) };
    public static readonly Type[] UShort = Short.Union(new[] { typeof(ushort) }).ToArray();
    public static readonly Type[] Int = UShort.Union(new[] { typeof(int), typeof(sbyte) }).ToArray();
    public static readonly Type[] UInt = Int.Union(new[] { typeof(uint) }).ToArray();
    public static readonly Type[] Long = UInt.Union(new[] { typeof(long), }).ToArray();
    public static readonly Type[] ULong = Long.Union(new[] { typeof(ulong) }).ToArray();

    public static readonly Type[] Decimal = ULong.Union(new[] { typeof(decimal) }).ToArray();
    public static readonly Type[] Double = ULong.Union(new[] { typeof(double), typeof(float) }).ToArray();
    public static readonly Type[] Float = Double;

  }
}
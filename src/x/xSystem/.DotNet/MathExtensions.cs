namespace System;
public static class MathExtensions {

  public static byte Clamp(this byte value, byte min, byte max) => Math.Clamp(value, min, max);
  public static decimal Clamp(this decimal value, decimal min, decimal max) => Math.Clamp(value, min, max);
  public static double Clamp(this double value, double min, double max) => Math.Clamp(value, min, max);
  public static float Clamp(this float value, float min, float max) => Math.Clamp(value, min, max);
  public static int Clamp(this int value, int min, int max) => Math.Clamp(value, min, max);
  public static long Clamp(this long value, long min, long max) => Math.Clamp(value, min, max);
  public static sbyte Clamp(this sbyte value, sbyte min, sbyte max) => Math.Clamp(value, min, max);
  public static short Clamp(this short value, short min, short max) => Math.Clamp(value, min, max);
  public static uint Clamp(this uint value, uint min, uint max) => Math.Clamp(value, min, max);
  public static ulong Clamp(this ulong value, ulong min, ulong max) => Math.Clamp(value, min, max);
  public static ushort Clamp(this ushort value, ushort min, ushort max) => Math.Clamp(value, min, max);

}

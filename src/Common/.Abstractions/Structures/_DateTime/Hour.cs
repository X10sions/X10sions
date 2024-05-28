using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct Hour(int Value) : IValueObject<int>, IFormattable {
  public Hour(DateTime d) : this(d.Hour) { }
  public Hour(decimal value) : this((int)value) { }
  public Hour(IntHHMM hhmm) : this(hhmm.Value / 100) { }
  public Hour(string value) : this(int.Parse(value)) { }
  public Hour(IntHHMMSS hhmmss) : this(hhmmss.Value / 10000) { }

  public const int MinValue = 0;
  public const int MaxValue = 23;
  public const string Format = "hh";

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);

  public static readonly Hour Min = new(MinValue);
  public static readonly Hour Max = new(MaxValue);

  public static implicit operator Hour(decimal value) => new(value);
  public static implicit operator Hour(int value) => new(value);
  public static implicit operator Hour(DateTime value) => new(value);
}
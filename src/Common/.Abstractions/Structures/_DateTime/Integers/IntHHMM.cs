using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct IntHHMM(int Value) : IValueObject<int>, IFormattable {
  public IntHHMM(Hour hh, Minute mm) : this((hh.Value * 100) + mm.Value) { }
  public IntHHMM(DateTime d) : this((d.Hour * 100) + d.Minute) { }
  public IntHHMM(string hh, string mm) : this(new Hour(hh), new Minute(mm)) { }
  public IntHHMM(IntHHMMSS hhmmss) : this(hhmmss.Value / 100) { }

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);

  public const int MinValue = 0;
  public const int MaxValue = 2359;
  public const string Format = "hhmm";

  public static readonly IntHHMM Min = new(MinValue);
  public static readonly IntHHMM Max = new(MaxValue);

  public Hour HH => new(this);
  public Month MM => new(this);

  #region IFormattable
  public override string ToString() => Value.ToString("0000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  #endregion

  public DateTime GetDate(int ss, int millisecond = 0) => new DateTime(1, 1, 1, HH.Value, MM.Value, ss, millisecond, DateTimeKind.Unspecified);

  public static implicit operator IntHHMM(decimal value) => new IntHHMM((int)value);
  public static implicit operator IntHHMM(int value) => new IntHHMM(value);
  public static implicit operator IntHHMM(DateTime value) => new IntHHMM(value);
}
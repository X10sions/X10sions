using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct IntHHMM(int Value) : IValueObject<int>, IFormattable {
  public IntHHMM(Hour hh, Minute mm) : this((hh.Value * 100) + mm.Value) { }
  public IntHHMM(DateTime d) : this((d.Hour * 100) + d.Minute) { }
  public IntHHMM(string hh, string mm) : this(new Hour(hh), new Minute(mm)) { }
  public IntHHMM(IntHHMMSS hhmmss) : this(hhmmss.Value / 100) { }

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);

  public const int MinValue = 0;
  public const int MaxValue = 2359;
  public const string Format = "HHmm";

  public static readonly IntHHMM Min = new(MinValue);
  public static readonly IntHHMM Max = new(MaxValue);

  public int HH => Value / 100;
  public Hour Hour => new(this);
  public int MM => Value % 100;
  public Minute Minute => new(this);

  public DateTime DateTime(int second = Second.MinValue, int millisecond = Millisecond.MinValue)
    => new(Year.MinValue, Month.MinValue, 1, Hour.Value, Minute.Value, second, millisecond, DateTimeKind.Unspecified);
  public TimeOnly TimeOnly(int seconds = Second.MinValue) => new(Hour.Value, Minute.Value, seconds);

  #region IFormattable
  public override string ToString() => Value.ToString("0000");
  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);
  #endregion


  public static implicit operator IntHHMM(decimal value) => new IntHHMM((int)value);
  public static implicit operator IntHHMM(int value) => new IntHHMM(value);
  public static implicit operator IntHHMM(DateTime value) => new IntHHMM(value);
}
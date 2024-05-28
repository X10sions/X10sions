using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHHMMSS(int Value) : IValueObject<int>, IFormattable {
  public IntHHMMSS() : this(DateTime.Now) { }
  public IntHHMMSS(Hour hh, Minute mm, Second ss) : this((hh.Value * 10000) + (mm.Value * 100) + ss.Value) { }
  public IntHHMMSS(IntCYYMMDD_HHMMSS cyymmdd_hmmss) : this(cyymmdd_hmmss.Value % 1 * 1000000) { }
  public IntHHMMSS(decimal cyymmdd_hhmmss) : this((int)(cyymmdd_hhmmss % 1 * 1000000)) { }


  public IntHHMMSS(DateTime d) : this(new Hour(d.Hour), new Minute(d.Minute), new Second(d.Second)) {
    Millisecond = new(d.Millisecond);
  }
  public IntHHMMSS(string hhmmss) : this(int.Parse(hhmmss)) { }

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);

  public const string Format = "hhmmss";
  public const int MinValue = 0;
  public const int MaxValue = 235959;

  public static readonly IntHHMMSS Min = new(MinValue);// 00:00:00
  public static readonly IntHHMMSS Max = new(MaxValue); // 23:59:00

  public Hour HH => new(this);
  public Minute MM => new(this);
  public Second SS => new(this);
  public Millisecond Millisecond { get; init; }
  public DateTime Date => new DateTime(1, 1, 1, HH.Value, MM.Value, SS.Value, Millisecond.Value, DateTimeKind.Unspecified);
  public TimeSpan ToTimeSpan() => new TimeSpan(0, HH.Value, MM.Value, SS.Value, Millisecond.Value);

  public override string ToString() => Value.ToString("000000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);

  public static implicit operator IntHHMMSS(decimal value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(int value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(DateTime value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(double value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(string value) => new IntHHMMSS(value.As(0));

  public static implicit operator DateTime(IntHHMMSS value) => value.Date;
  public static implicit operator decimal(IntHHMMSS value) => value.Value;
  public static implicit operator double(IntHHMMSS value) => value.Value;
  public static implicit operator int(IntHHMMSS value) => value.Value;
  public static implicit operator string(IntHHMMSS value) => value.ToString();

}

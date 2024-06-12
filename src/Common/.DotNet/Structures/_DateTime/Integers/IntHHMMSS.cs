using Common.ValueObjects;
using System;

namespace Common.Structures;

public readonly record struct IntHHMMSS : IValueObject<int>, IFormattable {
  public static TimeOnly GetTimeOnly(int hh, int mm, int ss) => new(hh.Clamp(Hour.MinValue, Hour.MaxValue), mm.Clamp(Minute.MinValue, Minute.MaxValue), ss.Clamp(Second.MinValue, Second.MaxValue));

  IntHHMMSS(int hh, int mm, int ss) {
    IntHHMM = new(hh * 100 + mm);
    IntSS = new(ss);
    Value = (HH * 10000) + (MM * 100) + SS;
    TimeOnly = IntHHMM.TimeOnly(ss);
  }
  public IntHHMMSS() : this(System.DateTime.Now) { }
  public IntHHMMSS(int hhmmss) : this(new IntHHMM(hhmmss / 10000), hhmmss % 100) { }
  public IntHHMMSS(IntHHMM hhmm, int ss) : this(hhmm.HH, hhmm.MM, ss) { }
  public IntHHMMSS(Hour hh, Minute mm, Second ss) : this((hh.Value * 10000) + (mm.Value * 100) + ss.Value) { }
  public IntHHMMSS(DecimalCYYMMDD_HHMMSS cyymmdd_hmmss) : this(cyymmdd_hmmss.Value % 1 * 1000000) { }
  public IntHHMMSS(decimal cyymmdd_hhmmss) : this((int)(cyymmdd_hhmmss % 1 * 1000000)) { }
  public IntHHMMSS(DateTime d) : this(TimeOnly.FromDateTime(d)) { }
  public IntHHMMSS(TimeOnly t) : this(new Hour(t.Hour), new Minute(t.Minute), new Second(t.Second)) { }
  public IntHHMMSS(string hhmmss) : this(int.Parse(hhmmss)) { }

  public int Value { get;  }  

  public const string Format = "HHmmss";
  public const int MinValue = 0;
  public const int MaxValue = 999999;

  public static readonly IntHHMMSS Min = new(MinValue);// 00:00:00
  public static readonly IntHHMMSS Max = new(MaxValue); // 23:59:00

  public int HH => IntHHMM.HH;
  public Hour Hour => new(this);
  public int MM => IntHHMM.MM;
  public Minute Minute => new(this);
  public IntSS IntSS { get; }
  public int SS => Value % 100;
  public Second Second => new(this);
  public IntHHMM IntHHMM { get; }
  public DateTime GetDateTime(int second = Second.MinValue, int millisecond = Millisecond.MinValue) => new(Year.MinValue, Month.MinValue, 1, Hour.Value, Minute.Value, second, millisecond, DateTimeKind.Unspecified);
  public TimeOnly TimeOnly { get; }// => new(Hour.Value, Minute.Value, Second.Value);
  public TimeSpan TimeSpan => TimeOnly.ToTimeSpan(  );

  public override string ToString() => Value.ToString("000000");
  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);

  public static implicit operator IntHHMMSS(decimal value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(int value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(DateTime value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(double value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(string value) => new IntHHMMSS(value.As(0));

  public static implicit operator TimeOnly(IntHHMMSS value) => value.TimeOnly;
  public static implicit operator DateTime(IntHHMMSS value) => value.GetDateTime();
  public static implicit operator decimal(IntHHMMSS value) => value.Value;
  public static implicit operator double(IntHHMMSS value) => value.Value;
  public static implicit operator int(IntHHMMSS value) => value.Value;
  public static implicit operator string(IntHHMMSS value) => value.ToString();

}

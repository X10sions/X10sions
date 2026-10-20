using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHHMMSS(int Value) : IValueObject<int> {
  public IntHHMMSS() : this(TimeProvider.System.GetLocalNow().DateTime) { }
  public IntHHMMSS(int hh, int mm, int ss) : this(
    10000 * hh.Clamp(MinHH, MaxHH) +
    100 * mm.Clamp(MinMM, MaxMM) +
    ss.Clamp(MinSS, MaxSS)
    ) { }
  public IntHHMMSS(DateTime dt) : this(dt.Hour, dt.Minute, dt.Second) { }
  public IntHHMMSS(TimeOnly t) : this(t.Hour, t.Minute, t.Second) { }

  //public IntHHMMSS(int hhmmss) : this(new IntHHMM(hhmmss / 10000), hhmmss % 100) { }
  //public IntHHMMSS(IntHHMM hhmm, int ss) : this(hhmm.HH, hhmm.MM, ss) { }
  //public IntHHMMSS(Hour hh, Minute mm, Second ss) : this((hh.Value * 10000) + (mm.Value * 100) + ss.Value) { }
  //public IntHHMMSS(DecimalCYYMMDD_HHMMSS cyymmdd_hmmss) : this(cyymmdd_hmmss.Value % 1 * 1000000) { }
  public IntHHMMSS(decimal cyymmdd_hhmmss) : this(decimal.ToInt32(decimal.Round(Math.Abs(cyymmdd_hhmmss % 1m) * 1_000_000m, 0, MidpointRounding.AwayFromZero))) { }
  public IntHHMMSS(string hhmmss) : this(int.Parse(hhmmss)) { }

  public static TimeOnly GetTimeOnly(int hh, int mm, int ss) => new(hh.Clamp(Hour.MinValue, Hour.MaxValue), mm.Clamp(Minute.MinValue, Minute.MaxValue), ss.Clamp(Second.MinValue, Second.MaxValue));

  //IntHHMMSS(int hh, int mm, int ss) {
  //  IntHHMM = new(hh * 100 + mm);
  //  IntSS = new(ss);
  //  Value = (HH * 10000) + (MM * 100) + SS;
  //  TimeOnly = IntHHMM.TimeOnly(ss);
  //}

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public int HH => HHMM / 100;
  public int HHMM => Value / 100;
  public int MM => HHMM % 100;
  public int SS => Value % 100;

  public const string DefaultFormat = "000000";
  public const int MinValue = 0;
  public const int MaxValue = 999999;
  public const int MinHH = 0;
  public const int MaxHH = 99;
  public const int MinMM = 0;
  public const int MaxMM = 99;
  public const int MinSS = 0;
  public const int MaxSS = 99;


  /// <summary>00:00:00</summary>
  public static readonly IntHHMMSS Min = new(MinValue);
  /// <summary>23:59:59</summary>
  public static readonly IntHHMMSS Max = new(MaxValue);

  //public IntHHMM IntHHMM => new(HHMM);
  // public int HHMMSS => (HH * 10000) + (MM * 100) + SS;

  public Hour Hour => new(HH);
  public Minute Minute => new(MM);
  public Second Second => new(SS);

  public TimeOnly TimeOnly => new(Hour.Value, Minute.Value, Second.Value);
  public TimeOnly GetTimeOnly(int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue) => new(Hour.Value, Minute.Value, Second.Value, millisecond, microsecond);
  public TimeSpan GetTimeSpan(int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue) => new(0, Hour.Value, Minute.Value, Second.Value, millisecond);

  public override string ToString() => Value.ToString(DefaultFormat);
  public string TimeString => $"{HH:00}{MM:00}{SS:00}";

  public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(string.IsNullOrWhiteSpace(format) ? DefaultFormat : format, formatProvider);

  public static implicit operator IntHHMMSS(decimal value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(int value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(DateTime value) => new IntHHMMSS(value);
  public static implicit operator IntHHMMSS(double value) => new IntHHMMSS((int)value);
  public static implicit operator IntHHMMSS(string value) => new IntHHMMSS(value.As(0));

  //public static implicit operator TimeOnly(IntHHMMSS value) => value.TimeOnly;
  //public static implicit operator DateTime(IntHHMMSS value) => value.GetDateTime();
  //public static implicit operator decimal(IntHHMMSS value) => value.Value;
  //public static implicit operator double(IntHHMMSS value) => value.Value;
  //public static implicit operator int(IntHHMMSS value) => value.Value;
  //public static implicit operator string(IntHHMMSS value) => value.ToString();

}

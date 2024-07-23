using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHHMMSS : IValueObject<int> {
  public IntHHMMSS() : this(TimeProvider.System.GetLocalNow().DateTime) { }
  public IntHHMMSS(int value) {
    HH = (value / 10000).Clamp(MinHH, MaxHH);
    MM = (value / 100 % 100).Clamp(MinMM, MaxMM);
    SS = (value % 100).Clamp(MinSS, MaxSS);
    Value = value.Clamp(MinValue, MaxValue);
  }
  public IntHHMMSS(int hh, int mm, int ss) {
    HH = hh.Clamp(MinHH, MaxHH);
    MM = mm.Clamp(MinMM, MaxMM);
    SS = ss.Clamp(MinSS, MaxSS);
    Value = (HH * 10000 + MM * 100 + SS).Clamp(MinValue, MaxValue);
  }
  public IntHHMMSS(DateTime dt) : this(dt.ToTimeOnly()) { }
  public IntHHMMSS(TimeOnly t) {
    HH = t.Hour;
    MM = t.Minute;
    SS = t.Second;
    Value = HH * 10000 + MM * 100 + SS;
    TimeOnly = t;
  }

  //public IntHHMMSS(int hhmmss) : this(new IntHHMM(hhmmss / 10000), hhmmss % 100) { }
  //public IntHHMMSS(IntHHMM hhmm, int ss) : this(hhmm.HH, hhmm.MM, ss) { }
  //public IntHHMMSS(Hour hh, Minute mm, Second ss) : this((hh.Value * 10000) + (mm.Value * 100) + ss.Value) { }
  //public IntHHMMSS(DecimalCYYMMDD_HHMMSS cyymmdd_hmmss) : this(cyymmdd_hmmss.Value % 1 * 1000000) { }
  public IntHHMMSS(decimal cyymmdd_hhmmss) : this((int)(cyymmdd_hhmmss % 1 * 1000000)) { }
  public IntHHMMSS(string hhmmss) : this(int.Parse(hhmmss)) { }


  public static TimeOnly GetTimeOnly(int hh, int mm, int ss) => new(hh.Clamp(Hour.MinValue, Hour.MaxValue), mm.Clamp(Minute.MinValue, Minute.MaxValue), ss.Clamp(Second.MinValue, Second.MaxValue));

  //IntHHMMSS(int hh, int mm, int ss) {
  //  IntHHMM = new(hh * 100 + mm);
  //  IntSS = new(ss);
  //  Value = (HH * 10000) + (MM * 100) + SS;
  //  TimeOnly = IntHHMM.TimeOnly(ss);
  //}

  public int Value { get; }
  public int HH { get; }
  public int MM { get; }
  public int SS { get; }

  public const string TimeFormat = "HHmmss";
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

  public IntHHMM IntHHMM { get; }
  public int HHMMSS => (HH * 10000) + (MM * 100) + SS;


  public Hour Hour => new(HH);
  public Minute Minute => new(MM);
  public Second Second => new(SS);

  public TimeOnly TimeOnly { get; }
  public TimeOnly GetTimeOnly(int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue) => new(Hour.Value, Minute.Value, Second.Value, millisecond, microsecond);
  public TimeSpan GetTimeSpan(int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue) => new(Hour.Value, Minute.Value, Second.Value, millisecond, microsecond);

  public override string ToString() => Value.ToString("000000");
  public string TimeString => Value.ToString(TimeFormat);

  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);

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

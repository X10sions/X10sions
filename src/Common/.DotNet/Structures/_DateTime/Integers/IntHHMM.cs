using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHHMM(int Value) : IValueObject<int> {
//  public IntHHMM(DateTime dt) : this(dt.Hour) { }
//  public IntHHMM(TimeOnly t) : this(t.Hour) { }

//  //public static int GetHH(int hh) => hh.Clamp(IntHH.MinValue, IntHH.MaxValue);
//  //public static int GetMM(int mm) => mm.Clamp(IntMM.MinValue, IntMM.MaxValue);
//  //IntHHMM(int hh, int mm) {
//  //  IntHH = new(hh);
//  //  IntMM = new(mm);
//  //  Value = HH * 100 + MM;
//  //}
//  //public IntHHMM(int hhmm) : this(hhmm / 100, hhmm % 100) { }
//  //public IntHHMM(Hour hh, Minute mm) : this(hh.Value, mm.Value) { }
//  //public IntHHMM(DateTime d) : this(d.Hour, d.Minute) { }
//  //public IntHHMM(TimeOnly t) : this(t.Hour, t.Minute) { }
//  //public IntHHMM(string hh, string mm) : this(new Hour(hh), new Minute(mm)) { }
//  //public IntHHMM(IntHHMMSS hhmmss) : this(hhmmss.Value / 100) { }
//  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);

//  #region Min & Max
//  public const string TimeFormat = "HHmm";
  public const int MinValue = 0;
  public const int MaxValue = 9999;

  //  public static readonly IntHHMM Min = new(MinValue);
  //  public static readonly IntHHMM Max = new(MaxValue);
  //  #endregion

  //  public IntHH IntHH => new(HH);
  //  public int HH => IntHH.Value;
  //  public int MM => Value % 100;
  //  public Hour Hour => IntHH.Hour;
  //  public Minute Minute => new(MM);

  //  public DateTime DateTime(int second = Second.MinValue, int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue, DateTimeKind dateTimeKind = DateTimeKind.Unspecified)
  //    => new(Year.MinValue, Month.MinValue, 1, Hour.Value, Minute.Value, second, millisecond, microsecond, dateTimeKind);
  //  public TimeOnly TimeOnly(int second = Second.MinValue, int millisecond = Millisecond.MinValue, int microsecond = Microsecond.MinValue) => new(Hour.Value, Minute.Value, second, millisecond, microsecond);

  //  #region IFormattable
  //  public override string ToString() => Value.ToString("0000");
  //  #endregion

  //  public static implicit operator IntHHMM(decimal value) => new IntHHMM((int)value);
  //  public static implicit operator IntHHMM(int value) => new IntHHMM(value);
  //  public static implicit operator IntHHMM(DateTime value) => new IntHHMM(value);
}
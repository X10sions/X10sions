using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct IntCYYMM(int Value) : IValueObject<int>, IFormattable {
  public IntCYYMM() : this(System.DateTime.Now) { }
  //public IntCYYMM(IntC c, IntYY yy, Month mm) : this(c.Value * 10000 + yy.Value * 100 + mm.Value) { }
  public IntCYYMM(DateTime d) : this(new IntCYY(d), new Month(d.Month)) { }
  //public IntCYYMM(string c, string yy, string mm) : this(new IntC(c), new IntYY(yy), new Month(mm)) { }
  public IntCYYMM(IntCYY cyy, Month mm) : this(cyy.Value * 100 + mm.Value) { }
  public IntCYYMM(IntCYYMMDD cyymmdd) : this(cyymmdd.Value / 100) { }

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public int C => IntCYY.C;
  public int CYY => IntCYY.Value;
  public int YY => IntCYY.YY;
  public int MM => Value % 100;
  public int YYYY => IntCYY.YYYY;
  public int DaysInMonth => System.DateTime.DaysInMonth(Year.Value, Month.Value);
  public IntC IntC => new(this);
  public IntCYY IntCYY => new(this);
  public IntCYYMMDD CYYMM00 => new(Value * 100);
  public IntCYYMMDD CYYMM01 => new(CYYMM00.Value + 1);
  public IntCYYMMDD CYYMM99 => new(CYYMM00.Value + 99);
  public Month Month => new(this);
  public Year Year => new(this);


  #region Min & Max Values
  public const int MinValue = 0;
  public const int MaxValue = 99999;
  public const int MaxValidValue = 99912;

  public static readonly IntCYYMM Min = new(MinValue);
  public static readonly IntCYYMM Max = new(MaxValue);
  public static readonly IntCYYMM MaxValid = new(MaxValidValue);
  #endregion

  public DateOnly DateOnly(int day) => new (YYYY, Month.Value, day);
  public DateTime DateTime(int day, int hour = Hour.MinValue, int minute = Minute.MinValue, int second = Second.MinValue, int millisecond = Millisecond.MinValue) => new  (YYYY, MM, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);

  #region IFormattable
  public override string ToString() => Value.ToString("00000");
  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);
  #endregion

  public static implicit operator IntCYYMM(decimal value) => new IntCYYMM((int)value);
  public static implicit operator IntCYYMM(int value) => new IntCYYMM(value);
  public static implicit operator IntCYYMM(DateTime value) => new IntCYYMM(value);

}

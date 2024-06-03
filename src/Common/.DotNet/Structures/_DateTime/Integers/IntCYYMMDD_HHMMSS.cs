using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct IntCYYMMDD_HHMMSS(decimal Value) : IValueObject<decimal>, IFormattable {
  public IntCYYMMDD_HHMMSS() : this(new IntCYYMMDD(), new IntHHMMSS()) { }
  public IntCYYMMDD_HHMMSS(IntCYYMMDD cyymmdd, IntHHMMSS hhmmss) : this(cyymmdd.Value + (hhmmss.Value / 1000000)) { }
  public IntCYYMMDD_HHMMSS(DateTime d) : this(new IntCYYMMDD(d), new IntHHMMSS(d)) { }
  public IntCYYMMDD_HHMMSS(int cyymmdd, int hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
  public IntCYYMMDD_HHMMSS(int year, int month, int day, int hour, int minute, int second, int millisecond) : this(new DateTime(year, month, day, hour, minute, second, millisecond)) { }
  public IntCYYMMDD_HHMMSS(string cyymmdd, string hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
  public IntCYYMMDD_HHMMSS(string c, string yymmdd, string hhmmss) : this(new IntCYYMMDD(c, yymmdd), new IntHHMMSS(hhmmss)) { }

  public const decimal MinValue = 0;
  public const decimal MinValidValue = 10101;
  public const decimal MaxValidValue = IntCYYMMDD.MaxValidCYYMMDD + IntHHMMSS.MaxValue;
  public const decimal MaxValue = IntCYYMMDD.MaxValue + IntHHMMSS.MaxValue;

  public decimal Value { get; init; } = Value.Clamp(MinValue, MaxValue);

  public static readonly IntCYYMMDD_HHMMSS Min = new(MinValue); // 0000-00-00 00:00:00
  public static readonly IntCYYMMDD_HHMMSS MinValid = new(MinValidValue); // 1901-01-01 00:00:00
  public static readonly IntCYYMMDD_HHMMSS Max = new(MaxValue);
  public static readonly IntCYYMMDD_HHMMSS MaxValid = new(MaxValidValue); // 9999-12-31 23:59:59

  public DateTime Date => new DateTime(IntCYYMMDD.YYYY.Value, IntCYYMMDD.MM.Value, IntCYYMMDD.DD.Value, IntHHMMSS.HH.Value, IntHHMMSS.MM.Value, IntHHMMSS.SS.Value, IntHHMMSS.Millisecond.Value, DateTimeKind.Unspecified);

  //public decimal CYYMMDD_HHMMSS {
  //  get => new decimal(IntCYYMMDD.CYYMMDD + (double)IntHHMMSS.HHMMSS / 1000000);
  //}

  public IntCYYMMDD IntCYYMMDD => new(this);
  public IntC C => IntCYYMMDD.C;
  public IntYYMMDD YYMMDD => IntCYYMMDD.YYMMDD;
  public IntHHMMSS IntHHMMSS => new(this);

  public static implicit operator IntCYYMMDD_HHMMSS(decimal value) => new(value);
  public static implicit operator IntCYYMMDD_HHMMSS(int value) => new(value);
  public static implicit operator IntCYYMMDD_HHMMSS(DateTime value) => new(value);

  #region IFormattable
  public override string ToString() => Value.ToString("0000000.000000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  #endregion

}
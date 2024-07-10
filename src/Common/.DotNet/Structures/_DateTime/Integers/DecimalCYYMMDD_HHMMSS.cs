using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct DecimalCYYMMDD_HHMMSS(decimal Value) : IValueObject<decimal>, IFormattable {
  public DecimalCYYMMDD_HHMMSS() : this(new IntCYYMMDD(), new IntHHMMSS()) { }
  public DecimalCYYMMDD_HHMMSS(IntCYYMMDD cyymmdd, IntHHMMSS hhmmss) : this(cyymmdd.Value + (hhmmss.Value / 1000000)) { }
  public DecimalCYYMMDD_HHMMSS(DateOnly d) : this(new IntCYYMMDD(d), IntHHMMSS.Min) { }
  public DecimalCYYMMDD_HHMMSS(DateTime d) : this(new IntCYYMMDD(d), new IntHHMMSS(d)) { }
  //public DecimalCYYMMDD_HHMMSS(TimeOnly t) : this(new IntCYYMMDD(),  new IntHHMMSS(t)) { }
  public DecimalCYYMMDD_HHMMSS(int cyymmdd, int hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
  public DecimalCYYMMDD_HHMMSS(int year, int month, int day, int hour, int minute, int second, int millisecond) : this(new DateTime(year, month, day, hour, minute, second, millisecond)) { }
  //DecimalCYYMMDD_HHMMSS(string cyymmdd, string hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
  public DecimalCYYMMDD_HHMMSS(string c, string yymmdd, string hhmmss) : this(new IntCYYMMDD(c, yymmdd), new IntHHMMSS(hhmmss)) { }

  public const decimal MinValue = 0;
  public const decimal MinValidValue = 10101;
  public const decimal MaxValidValue = IntCYYMMDD.MaxValidCYYMMDD + IntHHMMSS.MaxValue;
  public const decimal MaxValue = IntCYYMMDD.MaxValue + IntHHMMSS.MaxValue;

  public decimal Value { get; init; } = Value.Clamp(MinValue, MaxValue);

  public static readonly DecimalCYYMMDD_HHMMSS Min = new(MinValue); // 0000-00-00 00:00:00
  public static readonly DecimalCYYMMDD_HHMMSS MinValid = new(MinValidValue); // 1901-01-01 00:00:00
  public static readonly DecimalCYYMMDD_HHMMSS Max = new(MaxValue);
  public static readonly DecimalCYYMMDD_HHMMSS MaxValid = new(MaxValidValue); // 9999-12-31 23:59:59

  public DateOnly DateOnly => IntCYYMMDD.DateOnly;
  public DateTime DateTime => new(DateOnly, TimeOnly, DateTimeKind.Unspecified);
  public TimeOnly TimeOnly => IntHHMMSS.TimeOnly;

  //public decimal CYYMMDD_HHMMSS {
  //  get => new decimal(IntCYYMMDD.CYYMMDD + (double)IntHHMMSS.HHMMSS / 1000000);
  //}

  public IntCYYMMDD IntCYYMMDD => new(DateOnly);
  //public IntC C => IntCYYMMDD.C;
  //public IntYYMMDD YYMMDD => IntCYYMMDD.YYMMDD;
  public IntHHMMSS IntHHMMSS => new(TimeOnly);

  public static implicit operator DecimalCYYMMDD_HHMMSS(decimal value) => new(value);
  public static implicit operator DecimalCYYMMDD_HHMMSS(int value) => new(value);
  public static implicit operator DecimalCYYMMDD_HHMMSS(DateTime value) => new(value);

  public override string ToString() => Value.ToString("0000000.000000");
  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);

}

public static class DecimalCYYMMDD_HHMMSSExtensions {
  //public static Year ToYear(this int yyyy) => new(yyyy);
  //public static IntCYYMM ToIntCYYMM(this int cyymm) => new IntCYYMM(cyymm);
  //public static IntCYYMMDD ToIntCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd);
  //public static DecimalCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss = 0) => new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss);
  //public static DecimalCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0) => new DecimalCYYMMDD_HHMMSS(year, month, day, hour, minute, second, millisecond);
  //public static IntHHMMSS ToIntHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss);
  //public static IntHHMMSS ToIntHHMMSS(this int hour, int minute, int second) => new IntHHMMSS(new(hour), new(minute), new(second));
  //public static DateTime? ToDateFromCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd);
  //public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) => new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss).DateTime;
  //public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int c, int yymmdd, int hhmmss) => (c * 1000000 + yymmdd).ToDateFromCYYMMDD_HHMMSS(hhmmss);
  //public static DateTime? ToDateFromHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss).GetDateTime();
}
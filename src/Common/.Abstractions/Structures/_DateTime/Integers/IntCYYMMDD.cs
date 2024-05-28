using Common.ValueObjects;
using System.Linq.Expressions;

namespace Common.Structures;
public readonly record struct IntCYYMMDD(int Value) : IValueObject<int>,
  IComparable,
  IComparable<IntCYYMMDD>,
  IComparable<DateTime>,
  IComparable<decimal>,
  IComparable<double>,
  IComparable<int>,
  IComparable<string>,
  IConvertible,
  IEquatable<IntCYYMMDD>,
  IEquatable<DateTime>,
  IEquatable<decimal>,
  IEquatable<double>,
  IEquatable<int>,
  IEquatable<string> {

  public IntCYYMMDD() : this(DateTime.Now) { }
  public IntCYYMMDD(Year yyyy, Month mm, Day dd) : this(new DateTime(yyyy.Value, mm.Value, dd.Value)) { }
  public IntCYYMMDD(IntC c, IntYY yy, Month mm, Day dd) : this(c.Value * 1000000 + yy.Value * 10000 + mm.Value * 100 + dd.Value) { }
  public IntCYYMMDD(DateTime d) : this(new IntCYYMM(d), new Day(d.Day)) { }
  public IntCYYMMDD(string c, string yy, string mm, string dd) : this(new IntC(c), new IntYY(yy), new Month(mm), new Day(dd)) { }
  public IntCYYMMDD(string cyymmdd) : this(cyymmdd.As(0)) { }
  public IntCYYMMDD(string c, string yymmdd) : this((c + yymmdd).As(0)) { }
  public IntCYYMMDD(IntCYYMM cyymm, Day dd) : this(cyymm.Value * 100 + dd.Value) { }
  public IntCYYMMDD(IntC c, IntYYMMDD yymmdd) : this(c.Value * 1000000 + yymmdd.Value) { }
  public IntCYYMMDD(IntCYYMMDD_HHMMSS cyymmdd_hmmss) : this(cyymmdd_hmmss.Value ) { }
  public IntCYYMMDD(decimal cyymmdd_hhmmss) : this((int)cyymmdd_hhmmss) { }

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);

  #region Min & Max Values


  public const int MinValue = 0;
  public const int MaxValue = 9999999;

  public const int MinValidCYYMMDD = 101;
  public const int MaxValidCYYMMDD = 9991231;

  public static readonly IntCYYMMDD Min = new(0);
  public static readonly IntCYYMMDD Max = new(9999999);
  /// <summary>1901-01-01</summary>
  public static readonly IntCYYMMDD MinValid = new(101);
  /// <summary>9999-12-31</summary>
  public static readonly IntCYYMMDD MaxValid = new(9991231);
  #endregion

  public Day DD => new(this);
  public Month MM => new(this);
  public IntC C => new(this);
  public IntCYY CYY => new(this);
  public IntCYYMM CYYMM => new(this);
  public Year YYYY => new(this);

  public IntYYMMDD YYMMDD => new(this);
  public bool IsValid => Value.IsBetween(MinValidCYYMMDD, MaxValidCYYMMDD);

  public DateTime? Date =>   new(YYYY.Value, MM.Value, DD.Value, 0, 0, 0, 0, DateTimeKind.Unspecified);
  public DateTime? DateWithTime(IntHHMMSS hhmmss) => Date + hhmmss.ToTimeSpan();

  public string YYYY_MM_DD(string separator = "-") => $"{YYYY:0000}{separator}{MM:00}{separator}{DD:00}";
  public string DD_MM_YYYY(string separator = "-") => $"{DD:00}{separator}{MM:00}{separator}{YYYY:0000}";


  public static Expression<Func<DateTime, int>> CYYMMDD_Expression() => (DateTime d) => new IntCYYMMDD(d).Value;

  public static implicit operator IntCYYMMDD(decimal value) => new IntCYYMMDD((int)value);
  public static implicit operator IntCYYMMDD(DateTime value) => new IntCYYMMDD(value);
  public static implicit operator IntCYYMMDD(double value) => new IntCYYMMDD((int)value);
  public static implicit operator IntCYYMMDD(int value) => new IntCYYMMDD(value);
  public static implicit operator IntCYYMMDD(string value) => new IntCYYMMDD(Convert.ToInt32(value));

  public static implicit operator DateTime?(IntCYYMMDD value) => value.Date;
  public static implicit operator decimal(IntCYYMMDD value) => value.Value;
  public static implicit operator double(IntCYYMMDD value) => value.Value;
  public static implicit operator int(IntCYYMMDD value) => value.Value;
  public static implicit operator string(IntCYYMMDD value) => value.ToString();

  #region IComparable
  public int CompareTo(object? obj) {
    //if(obj == null) {        return 1;      }
    switch (obj) {
      case null:
        return 1;
      case IntCYYMMDD icymd:
        return icymd.CompareTo(obj);
      case int i:
        return i.CompareTo(obj);
      case DateTime dt:
        return dt.CompareTo(obj);
      case decimal dec:
        return dec.CompareTo(obj);
      case double dbl:
        return dbl.CompareTo(obj);
      case string s:
        return s.CompareTo(obj);
    }
    throw new NotImplementedException();
  }
  public int CompareTo(IntCYYMMDD other) => other.Value.CompareTo(Value);
  public int CompareTo(DateTime other) => other.CompareTo(Date);
  public int CompareTo(decimal other) => other.CompareTo(Value);
  public int CompareTo(double other) => other.CompareTo(Value);
  public int CompareTo(int other) => other.CompareTo(Value);
  public int CompareTo(string other) => string.Compare(other, ToString(), StringComparison.OrdinalIgnoreCase);
  #endregion

  #region IConvertible
  public bool ToBoolean(IFormatProvider provider) => throw new NotImplementedException();
  public byte ToByte(IFormatProvider provider) => Convert.ToByte(Value, provider);
  public char ToChar(IFormatProvider provider) => throw new NotImplementedException();
  public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Date, provider);
  public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Value, provider);
  public double ToDouble(IFormatProvider provider) => Convert.ToDouble(Value, provider);
  public float ToSingle(IFormatProvider provider) => Convert.ToSingle(Value, provider);
  public int ToInt32(IFormatProvider provider) => Convert.ToInt32(Value, provider);
  public long ToInt64(IFormatProvider provider) => Convert.ToInt64(Value, provider);
  public object ToType(Type conversionType, IFormatProvider provider) {
    switch (conversionType) {
      case Type _ when conversionType == typeof(DateTime):
      case Type _ when conversionType == typeof(TimeSpan):
        return Convert.ChangeType(Date, conversionType, provider);
      default:
        return Convert.ChangeType(Value, conversionType, provider);
    }
  }
  public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(Value, provider);
  public short ToInt16(IFormatProvider provider) => Convert.ToInt16(Value, provider);
  public string ToString(IFormatProvider provider) => Convert.ToString(ToString(), provider);
  public TypeCode GetTypeCode() => TypeCode.Int32;
  public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Value, provider);
  public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Value, provider);
  public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Value, provider);
  #endregion

  #region IEquatable
  public bool Equals(DateTime other) => other.Equals(Date);
  public bool Equals(decimal other) => other.Equals(Value);
  public bool Equals(double other) => other.Equals(Value);
  public bool Equals(int other) => other.Equals(Value);
  public bool Equals(string other) => other.Equals(ToString());
  #endregion

  #region IFormattable
  public override string ToString() => Value.ToString("0000000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  #endregion
}

public static class IntCYYMMDDExtensions {

  //public static int ToCYYMMDD(this DateTime d) => ((d.Year - 1900) * 10000) + (d.Month * 100) + d.Day;

  //[Computed] public static int? ToCYYMMDD(this DateTime? d, int? defaultIfNull = null) => d.HasValue ? d.Value.ToCYYMMDD() : defaultIfNull;
  //[Computed] public static int ToCYY(DateTime d) => ToCYY(d.Year);
  //[Computed] public static int ToCYY(int year) => (year - 1900);
  //[Computed] public static int ToCYYMM(DateTime d) => ToCYYMM(d.Year, d.Month);
  //[Computed] public static int ToCYYMM(int year, int month) => ((ToCYY(year) * 100) + month);
  //[Computed] public static int ToCYYMM00(DateTime d) => (ToCYYMM(d) * 100);
  //[Computed] public static int ToCYYMM01(DateTime d) => ((ToCYYMM(d) * 100) + 1);
  //[Computed] public static int ToCYYMM99(DateTime d) => ((ToCYYMM(d) * 100) + 99);
  //[Computed] public static int ToCYYMMDD(DateTime d) => ToCYYMMDD(d.Year, d.Month, d.Day);
  //[Computed] public static int ToCYYMMDD(int year, int month, int day) => (ToCYYMM(year, month) * 100) + day;

  //public static int IntCYYMMDD_GetCYYMM(this int cyymmdd) => (cyymmdd / 100).GetValueBetween(0, 99999);
  //public static int IntCYYMMDD_GetCYY(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYYMM() / 100;
  //public static int IntCYYMMDD_GetC(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYY() / 100;
  //public static int IntCYYMMDD_GetYY(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYY() % 100;
  //public static int IntCYYMMDD_GetMM(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYYMM() % 100;
  //public static int IntCYYMMDD_GetDD(this int cyymmdd) => cyymmdd % 100.GetValueBetween(0, 99);
}
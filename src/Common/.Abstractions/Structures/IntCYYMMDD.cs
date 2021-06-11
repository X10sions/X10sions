using System;
using System.Linq.Expressions;

namespace Common.Structures {
  public class IntCYYMMDD : IntCYYMM,
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

    public IntCYYMMDD() { }
    public IntCYYMMDD(DateTime d) : this(d.Year, d.Month, d.Day) { }
    public IntCYYMMDD(int cyymmdd) {
      CYYMMDD = cyymmdd;
    }
    public IntCYYMMDD(int c, int yy, int mm, int dd) {
      C = c;
      YY = yy;
      MM = mm;
      DD = dd;
    }
    public IntCYYMMDD(int year, int month, int day) {
      SetYearMonthDay(year, month, day);
    }

    #region Min & Max Values
    public const int _MinCYYMMDD = 0;
    public const int _MaxCYYMMDD = 9999999;

    public const int _MinDD = 0;
    public const int _MaxDD = 99;

    public static readonly IntCYYMMDD _MaxIntCYYMMDD = new IntCYYMMDD(_MaxCYYMMDD); // 9999-12-31
    public static readonly IntCYYMMDD _MinIntCYYMMDD = new IntCYYMMDD(_MinCYYMMDD); // 1901-01-01

    //public static readonly DateTime _MaxDate = new DateTime(9999 - 1900, 12, 31).Date;
    //public static readonly DateTime _MinDate = new DateTime(0 + 1900, 1, 1).Date;
    #endregion

    int dd;
    public int DD { get => dd; set => dd = value.GetValueBetween(_MinDD, _MaxDD); }


    public int Day { get => GetDay(DD); set => SetDay(value); }
    public void SetDay(int day) => DD = GetDay(day);
    public int GetDay(int day) => day.GetValueBetween(1, DateTime.DaysInMonth(Year, Month));

    public DateTime Date {
      get => new DateTime(Year, Month, Day).Date;
      set => SetYearMonthDay(value.Year, value.Month, value.Day);
    }

    public int CYYMMDD {
      get => CYYMM * 100 + DD;
      set {
        value = value.GetValueBetween(_MinCYYMMDD, _MaxCYYMMDD);
        CYYMM = value / 100;
        DD = value % 100;
      }
    }

    public override void SetMonth(int month) => SetYearMonthDay(Year, month, Day);
    public override void SetYear(int year) => SetYearMonthDay(year, Month, Day);

    public void SetYearMonthDay(int year, int month, int day) {
      base.SetYear(year);
      base.SetMonth(month);
      SetDay(day);
    }

    public static Expression<Func<DateTime, int>> CYYMMDD_Expression() => (DateTime d) => new IntCYYMMDD(d).CYYMMDD;

    public static implicit operator IntCYYMMDD(decimal value) => new IntCYYMMDD((int)value);
    public static implicit operator IntCYYMMDD(DateTime value) => new IntCYYMMDD(value);
    public static implicit operator IntCYYMMDD(double value) => new IntCYYMMDD((int)value);
    public static implicit operator IntCYYMMDD(int value) => new IntCYYMMDD(value);
    public static implicit operator IntCYYMMDD(string value) => new IntCYYMMDD(Convert.ToInt32(value));

    public static implicit operator DateTime(IntCYYMMDD value) => value.Date;
    public static implicit operator decimal(IntCYYMMDD value) => value.CYYMMDD;
    public static implicit operator double(IntCYYMMDD value) => value.CYYMMDD;
    public static implicit operator int(IntCYYMMDD value) => value.CYYMMDD;
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
    public int CompareTo(IntCYYMMDD other) => other.CYYMMDD.CompareTo(CYYMMDD);
    public int CompareTo(DateTime other) => other.CompareTo(Date);
    public int CompareTo(decimal other) => other.CompareTo(CYYMMDD);
    public int CompareTo(double other) => other.CompareTo(CYYMMDD);
    public int CompareTo(int other) => other.CompareTo(CYYMMDD);
    public int CompareTo(string other) => string.Compare(other, ToString(), StringComparison.OrdinalIgnoreCase);
    #endregion

    #region IConvertible
    public bool ToBoolean(IFormatProvider provider) => throw new NotImplementedException();
    public byte ToByte(IFormatProvider provider) => Convert.ToByte(CYYMMDD, provider);
    public char ToChar(IFormatProvider provider) => throw new NotImplementedException();
    public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Date, provider);
    public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(CYYMMDD, provider);
    public double ToDouble(IFormatProvider provider) => Convert.ToDouble(CYYMMDD, provider);
    public float ToSingle(IFormatProvider provider) => Convert.ToSingle(CYYMMDD, provider);
    public int ToInt32(IFormatProvider provider) => Convert.ToInt32(CYYMMDD, provider);
    public long ToInt64(IFormatProvider provider) => Convert.ToInt64(CYYMMDD, provider);
    public object ToType(Type conversionType, IFormatProvider provider) {
      switch (conversionType) {
        case Type _ when conversionType == typeof(DateTime):
        case Type _ when conversionType == typeof(TimeSpan):
          return Convert.ChangeType(Date, conversionType, provider);
        default:
          return Convert.ChangeType(CYYMMDD, conversionType, provider);
      }
    }
    public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(CYYMMDD, provider);
    public short ToInt16(IFormatProvider provider) => Convert.ToInt16(CYYMMDD, provider);
    public string ToString(IFormatProvider provider) => Convert.ToString(ToString(), provider);
    public TypeCode GetTypeCode() => TypeCode.Int32;
    public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(CYYMMDD, provider);
    public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(CYYMMDD, provider);
    public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(CYYMMDD, provider);
    #endregion

    #region IEquatable
    public bool Equals(IntCYYMMDD other) => other.CYYMMDD.Equals(CYYMMDD);
    public bool Equals(DateTime other) => other.Equals(Date);
    public bool Equals(decimal other) => other.Equals(CYYMMDD);
    public bool Equals(double other) => other.Equals(CYYMMDD);
    public bool Equals(int other) => other.Equals(CYYMMDD);
    public override bool Equals(object obj) {
      switch (obj) {
        case IntCYYMMDD other:
          return other.CYYMMDD.Equals(CYYMMDD);
        case DateTime other:
          return other.Equals(Date);
        case decimal other:
          return other.Equals(CYYMMDD);
        case double other:
          return other.Equals(CYYMMDD);
        case int other:
          return other.Equals(CYYMMDD);
        case string other:
          return other.Equals(ToString());
      }
      throw new NotImplementedException();
    }
    public bool Equals(string other) => other.Equals(ToString());
    #endregion

    #region IFormattable
    public override string ToString() => CYYMMDD.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    //[Computed] public int CYYMMDD => ToCYYMMDD(Year, Month, Day);
    //[Computed] public bool IsMaxCYYMMDD() => CYYMMDD == _MaxCYYMMDD;

    //public string SqlValue() => Date.ToSqlDate();
    //public string SqlExpression() => Date.ToSqlDateExpression();

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

}

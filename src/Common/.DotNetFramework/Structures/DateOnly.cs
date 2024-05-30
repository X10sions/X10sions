using System;
using System.Globalization;

namespace Common.Structures {
  public struct DateOnly :
   IComparable,
   IComparable<DateTime>,
   IComparable<string>,
   IConvertible,
   IEquatable<DateTime>,
   IEquatable<string>,
   IFormattable {

    public DateOnly(DateTime d) : this(d.Year, d.Month, d.Day) {
    }

    public DateOnly(int year, int month = 1, int day = 1) {
      Year = year;
      Month = month;
      Day = day;
    }

    public static readonly DateOnly _Max = new DateOnly(9999, 12, 31); // 9999-12-31
    public static readonly DateOnly _Min = new DateOnly(1, 1, 1); // 1901-01-01

    public DateTime Date {
      get => new DateTime(Year, Month, Day);
      set {
        Year = value.Year;
        Month = value.Month;
        Day = value.Day;
      }
    }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }

    public static implicit operator DateOnly(DateTime value) => new DateOnly(value);

    public static implicit operator DateTime(DateOnly value) => value.Date;
    public static implicit operator string(DateOnly value) => value.ToString();

    #region IComparable
    public int CompareTo(object obj) {
      switch (obj) {
        //case null: return null;
        case int i: return i.CompareTo(obj);
        case DateTime dt: return dt.CompareTo(obj);
        case decimal dec: return dec.CompareTo(obj);
        case double dbl: return dbl.CompareTo(obj);
        case string s: return s.CompareTo(obj);
      }
      throw new NotImplementedException();
    }
    public int CompareTo(DateTime other) => other.CompareTo(Date);
    public int CompareTo(string other) => string.Compare(other, ToString(), StringComparison.OrdinalIgnoreCase);
    #endregion

    #region IConvertible 
    public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Date, provider);
    public byte ToByte(IFormatProvider provider) => Convert.ToByte(Date, provider);
    public char ToChar(IFormatProvider provider) => Convert.ToChar(Date, provider);
    public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Date, provider);
    public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Date, provider);
    public double ToDouble(IFormatProvider provider) => Convert.ToDouble(Date, provider);
    public float ToSingle(IFormatProvider provider) => Convert.ToSingle(Date, provider);
    public int ToInt32(IFormatProvider provider) => Convert.ToInt32(Date, provider);
    public long ToInt64(IFormatProvider provider) => Convert.ToInt64(Date, provider);
    public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Date, conversionType, provider);
    public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(Date, provider);
    public short ToInt16(IFormatProvider provider) => Convert.ToInt16(Date, provider);
    public string ToString(IFormatProvider provider) => Convert.ToString(ToString(), provider);
    public TypeCode GetTypeCode() => TypeCode.Int32;
    public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Date, provider);
    public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Date, provider);
    public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Date, provider);
    #endregion

    #region IEquatable
    public bool Equals(DateTime other) => other.Equals(Date);
    public bool Equals(string other) => other.Equals(ToString());
    #endregion

    #region IFormattable
    public override string ToString() => Date.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
    public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

  }
}

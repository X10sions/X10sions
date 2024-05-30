using System;
using System.Globalization;

namespace Common.Structures {
  public struct TimeOnly :
   IComparable,
   IComparable<DateTime>,
   IComparable<TimeSpan>,
   IComparable<string>,
   IConvertible,
   IEquatable<DateTime>,
   IEquatable<TimeSpan>,
   IEquatable<string>,
   IFormattable {

    public TimeOnly(DateTime d) : this(new TimeSpan(d.Ticks)) {
      Date = d;
    }

    public TimeOnly(TimeSpan ts) : this() {
      TimeSpan = ts;
    }

    public TimeOnly(int hour, int minute = 0, int second = 0, int millisecond = 0) {
      Hour = hour;
      Minute = minute;
      Second = second;
      Millisecond = millisecond;
    }

    public static readonly TimeOnly _Max = new TimeOnly(23, 59, 59, 999); // 23:59:59.999
    public static readonly TimeOnly _Min = new TimeOnly(0); // 00:00:00.000


    public DateTime Date {
      get => new DateTime(1, 1, 1, Hour, Minute, Second);
      set {
        Hour = value.Hour;
        Minute = value.Minute;
        Second = value.Second;
        Millisecond = value.Millisecond;
      }
    }

    public TimeSpan TimeSpan {
      get => new TimeSpan(0, Hour, Minute, Second, Millisecond);
      set {
        Hour = value.Hours;
        Minute = value.Minutes;
        Second = value.Seconds;
        Millisecond = value.Milliseconds;
      }
    }

    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
    public int Millisecond { get; set; }

    public static implicit operator TimeOnly(DateTime value) => new TimeOnly(value);
    public static implicit operator TimeOnly(TimeSpan value) => new TimeOnly(value);

    public static implicit operator DateTime(TimeOnly value) => value.Date;
    public static implicit operator string(TimeOnly value) => value.ToString();
    public static implicit operator TimeSpan(TimeOnly value) => value.TimeSpan;

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
    public int CompareTo(TimeSpan other) => other.CompareTo(TimeSpan);
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
    public bool Equals(TimeSpan other) => other.Equals(TimeSpan);
    #endregion

    #region IFormattable
    public override string ToString() => Date.ToString("HH:mm:ss.fff", CultureInfo.CurrentCulture);
    public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

  }
}

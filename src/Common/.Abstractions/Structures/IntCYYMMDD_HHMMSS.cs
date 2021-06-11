using System;

namespace Common.Structures {
  public struct IntCYYMMDD_HHMMSS : IFormattable {

    public IntCYYMMDD_HHMMSS(decimal cyymmdd_hhmmss) : this() {
      CYYMMDD_HHMMSS = cyymmdd_hhmmss;
    }

    public IntCYYMMDD_HHMMSS(IntCYYMMDD cyymmdd, IntHHMMSS? hhmmss = null) : this() {
      IntCYYMMDD = cyymmdd;
      IntHHMMSS = hhmmss ?? new IntHHMMSS(0);
    }

    public IntCYYMMDD_HHMMSS(int cyymmdd, int hhmmss = 0) : this() {
      CYYMMDD = cyymmdd;
      HHMMSS = hhmmss;
    }

    public IntCYYMMDD_HHMMSS(int year, int month, int day, int hour = 0, int minute = 0, int second = 0) {
      IntCYYMMDD = new IntCYYMMDD(year, month, day);
      IntHHMMSS = new IntHHMMSS(hour, minute, second);
    }

    public IntCYYMMDD_HHMMSS(DateTime d) : this(d.Year, d.Month, d.Day) { }

    public static readonly IntCYYMMDD_HHMMSS _Max = new IntCYYMMDD_HHMMSS(9991231, 235959); // 9999-12-31 23:59:59
    public static readonly IntCYYMMDD_HHMMSS _Min = new IntCYYMMDD_HHMMSS(10101, 0); // 1901-01-01 00:00:00

    public DateTime Date {
      get => new DateTime(IntCYYMMDD.Year, IntCYYMMDD.Month, IntCYYMMDD.Day, IntHHMMSS.Hour, IntHHMMSS.Minute, IntHHMMSS.Second, IntHHMMSS.Millisecond);
      set {
        IntCYYMMDD = new IntCYYMMDD(value.Year, value.Month, value.Day);
        IntHHMMSS = new IntHHMMSS(value.Hour, value.Minute, value.Second);
      }
    }

    public decimal CYYMMDD_HHMMSS {
      get => new decimal(IntCYYMMDD.CYYMMDD + (double)IntHHMMSS.HHMMSS / 1000000);
      set {
        IntCYYMMDD = (int)value;
        IntHHMMSS = (int)(value % 1 * 1000000);
      }
    }

    public int CYYMMDD {
      get => IntCYYMMDD.CYYMMDD;
      set => IntCYYMMDD = value;
    }

    public int HHMMSS {
      get => IntHHMMSS.HHMMSS;
      set => IntHHMMSS = value;
    }

    public IntCYYMMDD IntCYYMMDD { get; set; }
    public IntHHMMSS IntHHMMSS { get; set; }

    public static implicit operator IntCYYMMDD_HHMMSS(decimal value) => new IntCYYMMDD_HHMMSS(value);
    public static implicit operator IntCYYMMDD_HHMMSS(int value) => new IntCYYMMDD_HHMMSS(value);
    public static implicit operator IntCYYMMDD_HHMMSS(DateTime value) => new IntCYYMMDD_HHMMSS(value);

    #region IFormattable
    public override string ToString() => CYYMMDD_HHMMSS.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    //public IntCYYMMDD_HHMMSS(DateTime d) => Date = d;
    //public decimal CYYMMDD_HHMMSS() => ToCYYMMDD_HHMMSS(Date);
    //public static decimal ToCYYMMDD_HHMMSS(this DateTime d) => ToCYYMMDD_HHMMSS(d.ToCYYMMDD(), d.ToHHMMSS());
    //public static decimal ToCYYMMDD_HHMMSS(this DateTime d) => ToCYYMMDD_HHMMSS(IntCYYMMDD.ToCYYMMDD(d), IntHHMMSS.ToHHMMSS(d));
    //public static decimal? ToCYYMMDD_HHMMSS(this DateTime? d, decimal? defaultIfNull = null) => d.HasValue ? d.Value.ToCYYMMDD_HHMMSS() : defaultIfNull;
    //public static decimal ToCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) => new decimal(cyymmdd + (double)hhmmss / 1000000);
    //public static decimal ToCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) => cyymmdd + (hhmmss / 1000000);
    //public static decimal ToCYYMMDD_HHMMSS(this int year, int month, int day, int hour, int minute, int second) => ToCYYMMDD_HHMMSS(IntCYYMMDD.ToCYYMMDD(year, month, day), IntHHMMSS.ToHHMMSS(hour, minute, second));
    //public static decimal ToCYYMMDD_HHMMSS(this int year, int month, int day, int hour, int minute, int second) => ToCYYMMDD_HHMMSS(ToCYYMMDD(year, month, day), ToHHMMSS(hour, minute, second) / 1000000);

    //public string SqlValue(int milliSecondsPrecision) => Date.ToSqlTimestamp(milliSecondsPrecision);
    //public string SqlExpression(int milliSecondsPrecision) => Date.ToSqlTimestampExpression(milliSecondsPrecision);


  }
}
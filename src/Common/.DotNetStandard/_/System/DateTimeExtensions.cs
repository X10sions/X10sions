using Common.Structures;
using System.Globalization;

namespace System;

public static class DateTimeExtensions {


  #region "iSeries/iDb2Date Dates"

  public static int ToCYYMM(this DateTime d) => new IntCYYMMDD(d).CYYMM;//IntCYYMMDD.ToCYYMM(d);
  public static int ToCYYMMDD(this DateTime d) => new IntCYYMMDD(d).Value;// IntCYYMMDD.ToCYYMMDD(d);
  public static int? ToCYYMMDD(this DateTime? d) =>  d.HasValue ? d.Value.ToCYYMMDD() : 0;
  //public static int ToIntC(this DateTime d) => d.ToIntCYY() / 100;
  //public static int ToIntCYY(this DateTime d) => d.Year - 1900;
  //public static int ToIntCYYMM(this DateTime d) => (d.ToIntCYY() * 100) + d.Month;
  //public static int ToIntCYYMMDD(this DateTime d, int? day = null) => (d.ToIntCYYMM() * 100) + (day ?? d.Day);
  //public static int? ToIntCYYMMDD(this DateTime? d, int? day = null) => d.HasValue ? d.Value.ToIntCYYMMDD(day) : null;
  //public static int ToIntCYYMM00(this DateTime d) => d.ToIntCYYMMDD(0);
  //public static int ToIntCYYMM01(this DateTime d) => d.ToIntCYYMMDD(1);
  //public static int ToIntCYYMM99(this DateTime d) =>   d.ToIntCYYMMDD(99);
  //public static int ToIntDDMMYY(this DateTime d) => (d.Day * 10000) + (d.Month * 100) + (d.Year % 100);
  //public static decimal ToDecimalCYYMMDD_HHMMSS(this DateTime d, int? day, int? second = null) => d.ToIntCYYMMDD(day) + (d.ToIntHHMMSS(second) / 1000000);
  //public static int ToIntHHMM(this DateTime d) => (d.Hour * 100) + d.Minute;
  //public static int ToIntHHMMSS(this DateTime d, int? second = null) => (d.ToIntHHMM() * 100) + (second ?? d.Second);
  //public static long ToLongYYYYMMDDHHMMSS(this DateTime d) => (d.ToYYYYMMDD() * 1000000) + d.ToIntHHMMSS();

  public static int xToHHMMSS(this DateTime d) => (d.Hour * 10000) + (d.Minute * 100) + d.Second;
  public static int? xToHHMMSS(this DateTime? d) => d.HasValue ? xToHHMMSS(d.Value) : 0;


  #endregion "iSeries"

}

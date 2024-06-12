using Common.Structures;

namespace System;

public static class Int32Extensions {

  public static Year ToYear(this int yyyy) => new(yyyy);
  public static IntCYYMM ToIntCYYMM(this int cyymm) => new IntCYYMM(cyymm);
  public static IntCYYMMDD ToIntCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd);
  public static DecimalCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss = 0) => new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss);
  public static DecimalCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0) => new DecimalCYYMMDD_HHMMSS(year, month, day, hour, minute, second, millisecond);
  public static IntHHMMSS ToIntHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss);
  public static IntHHMMSS ToIntHHMMSS(this int hour, int minute, int second) => new IntHHMMSS(new(hour), new(minute), new(second));
  public static DateTime? ToDateFromCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd);
  public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) => new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss).DateTime;
  public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int c, int yymmdd, int hhmmss) => (c * 1000000 + yymmdd).ToDateFromCYYMMDD_HHMMSS(hhmmss);
  //public static DateTime? ToDateFromHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss).GetDateTime();
}
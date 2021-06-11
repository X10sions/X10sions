using Common.Structures;

namespace System {
  public static class Int32Extensions {

    public static IntYear ToYear(this int yyyy) => new IntYear(yyyy);

    public static IntCYYMM ToIntCYYMM(this int cyymm) => new IntCYYMM(cyymm);

    public static IntCYYMMDD ToIntCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd);
    public static IntCYYMMDD ToIntCYYMMDD(this int year, int month, int day) => new IntCYYMMDD(year, month, day);

    public static IntCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss = 0) => new IntCYYMMDD_HHMMSS(cyymmdd, hhmmss);
    public static IntCYYMMDD_HHMMSS ToIntCYYMMDD_HHMMSS(this int year, int month, int day, int hour = 0, int minute = 0, int second = 0) => new IntCYYMMDD_HHMMSS(year, month, day, hour, minute, second);

    public static IntHHMMSS ToIntHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss);
    public static IntHHMMSS ToIntHHMMSS(this int hour, int minute, int second) => new IntHHMMSS(hour, minute, second);

    public static DateTime? ToDateFromCYYMMDD(this int cyymmdd) => new IntCYYMMDD(cyymmdd).Date;
    public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) => new IntCYYMMDD_HHMMSS(cyymmdd, hhmmss).Date;
    public static DateTime? ToDateFromHHMMSS(this int hhmmss) => new IntHHMMSS(hhmmss).Date;

  }
}
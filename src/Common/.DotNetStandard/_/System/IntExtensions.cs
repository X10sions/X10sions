namespace System;

public static class IntExtensions {

  public static DateTime? ToDateFromCYYMMDD(this int cyymmdd) {
    int yyyy = cyymmdd / 10000 + 1900;
    int mm = cyymmdd / 100 % 100;
    int dd = cyymmdd % 100;
    try {
      return new DateTime(yyyy, mm, dd);
    } catch (Exception ex) {
      throw new Exception($"{ex.Message}: {yyyy}:{mm}:{dd}");
    }
  }

  public static DateTime? ToDateFromCYYMMDD_HHMMSS(this int cyymmdd, int hhmmss) {
    DateTime d = cyymmdd.ToDateFromCYYMMDD().Value;
    DateTime t = hhmmss.ToDateFromHHMMSS().Value;
    return new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);
  }

  public static DateTime? ToDateFromHHMMSS(this int hhmmss) {
    int hh = hhmmss / 10000;
    int mm = hhmmss / 100 % 100;
    int ss = hhmmss % 100;
    try {
      return new DateTime(1, 1, 1, hh, mm, ss);
    } catch (Exception ex) {
      throw new Exception($"{ex.Message} hhmmss: {hh}:{mm}:{ss}");
    }
  }

}

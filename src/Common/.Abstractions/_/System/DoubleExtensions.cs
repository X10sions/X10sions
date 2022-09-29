using Common.Structures;

namespace System {
  public static class DoubleExtensions {

    public static DateTime? ToDateCYYMMDD(this double cyymmdd_hhmmss) => new ISeriesDateTime(cyymmdd_hhmmss).AsDateOnly();
    public static DateTime? ToDateTimeCYYMMDD_HHMMSS(this double cyymmdd_hhmmss) => new ISeriesDateTime(cyymmdd_hhmmss).AsDateAndTime();
    public static DateTime? ToDateTimeCYYMMDD_HHMMSS(this double? cyymmdd_hhmmss) => !cyymmdd_hhmmss.HasValue ? null : ToDateTimeCYYMMDD_HHMMSS(cyymmdd_hhmmss.Value);
    public static DateTime? ToTimeHHMMSS(this double cyymmdd_hhmmss) => new ISeriesDateTime(cyymmdd_hhmmss).AsTimeOnly();

  }
}
using Common.Structures;

namespace System {
  public static class DoubleExtensions {

    public static DateTime? ToDateCYYMMDD(this double cyymmdd_hhmmss) {
      var d = new ISeriesDateTime(cyymmdd_hhmmss);
      return d.AsDateOnly();
    }

    public static DateTime? ToDateTimeCYYMMDD_HHMMSS(this double cyymmdd_hhmmss) {
      var d = new ISeriesDateTime(cyymmdd_hhmmss);
      return d.AsDateAndTime();
    }

    public static DateTime? ToDateTimeCYYMMDD_HHMMSS(this double? cyymmdd_hhmmss) {
      if (!cyymmdd_hhmmss.HasValue) {
        return null;
      }
      return ToDateTimeCYYMMDD_HHMMSS(cyymmdd_hhmmss.Value);
    }

    public static DateTime? ToTimeHHMMSS(this double cyymmdd_hhmmss) {
      var d = new ISeriesDateTime(cyymmdd_hhmmss);
      return d.AsTimeOnly();
    }

  }
}
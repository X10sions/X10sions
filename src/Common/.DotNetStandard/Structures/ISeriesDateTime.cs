using System;

namespace Common.Structures {
  public class ISeriesDateTime {
    public int Year { get; set; } = 1;
    public int Month { get; set; } = 1;
    public int Day { get; set; } = 1;
    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }

    public ISeriesDateTime(double cyymmdd_hhmmss) {
      SetCYYMMDD((int)cyymmdd_hhmmss);
      SetHHMMSS((int)cyymmdd_hhmmss);
    }

    public ISeriesDateTime(int cyymmdd) {
      SetCYYMMDD(cyymmdd);
    }

    public ISeriesDateTime(int cyymmdd, int hhmmss) {
      SetCYYMMDD(cyymmdd);
      SetHHMMSS(hhmmss);
    }

    public ISeriesDateTime(DateTime d) {
      Year = d.Year;
      Month = d.Month;
      Day = d.Day;
      Hour = d.Hour;
      Minute = d.Minute;
      Second = d.Second;
    }

    public void SetCYYMMDD(int cyymmdd) {
      Year = (cyymmdd / 10000) + 1900;
      Month = (cyymmdd / 100) % 100;
      Day = cyymmdd % 100;
    }

    public void SetHHMMSS(int hhmmss) {
      Hour = hhmmss / 10000;
      Minute = (hhmmss / 100) % 100;
      Second = hhmmss % 100;
    }

    public DateTime AsDateOnly() => new DateTime(Year, Month, Day);

    public int AsDateOnlyInteger() => checked((Year - 1900) * 10000 + Month * 100 + Day);

    public DateTime AsDateAndTime() => new DateTime(Year, Month, Day, Hour, Minute, Second);

    public double AsDateAndTimeDouble() => AsDateOnlyInteger() + AsTimeOnlyInteger() / 1000000.0;

    public DateTime AsTimeOnly() => new DateTime(Hour, Minute, Second);

    public int AsTimeOnlyInteger() => checked(Hour * 10000 + Minute * 100 + Second);
  }
}

using System;
using System.Globalization;

namespace Common.Enums {
  public enum DateInterval {
    Day = 4,
    DayOfYear = 3,
    Hour = 7,
    Minute = 8,
    Month = 2,
    Quarter = 1,
    Second = 9,
    Weekday = 6,
    WeekOfYear = 5,
    Year = 0
  }

  public static class DateIntervalExtensions {

    [Obsolete]
    public static long DateDiff(this DateInterval intervalType, DateTime dateOne, DateTime dateTwo, DayOfWeek? firstDayOfWeek = null) {
      switch (intervalType) {
        case DateInterval.Day:
        case DateInterval.DayOfYear:
          return (long)(dateTwo - dateOne).TotalDays;
        case DateInterval.Hour: return (long)(dateTwo - dateOne).TotalHours;
        case DateInterval.Minute: return (long)(dateTwo - dateOne).TotalMinutes;
        case DateInterval.Month: return ((dateTwo.Year - dateOne.Year) * 12) + (dateTwo.Month - dateOne.Month);
        case DateInterval.Quarter: return (long)((4 * (dateTwo.Year - dateOne.Year)) + Math.Ceiling(dateTwo.Month / 3.0) - Math.Ceiling(dateOne.Month / 3.0));
        case DateInterval.Second: return (long)(dateTwo - dateOne).TotalSeconds;
        case DateInterval.Weekday: return (long)((dateTwo - dateOne).TotalDays / 7.0);
        case DateInterval.WeekOfYear:
          firstDayOfWeek = firstDayOfWeek ?? DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;
          return (long)((dateOne.FirstDayOfWeek(firstDayOfWeek) - dateTwo.FirstDayOfWeek(firstDayOfWeek)).TotalDays / 7.0);
        case DateInterval.Year: return dateTwo.Year - dateOne.Year;
        default: return 0;
      }
    }

  }
}
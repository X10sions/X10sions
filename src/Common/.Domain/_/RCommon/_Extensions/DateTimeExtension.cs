using System.Diagnostics;

namespace RCommon;
public static class DateTimeExtensions {
  private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
  private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

  [DebuggerStepThrough]
  public static bool IsValid(this DateTime target) => (target >= MinDate) && (target <= MaxDate);
  public static DateTime FirstDayOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);
  public static DateTime LastDayOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));

 

}

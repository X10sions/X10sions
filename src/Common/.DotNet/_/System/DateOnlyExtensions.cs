namespace System;

public static class DateOnlyExtensions {

  public static int DaysInMonth(this DateOnly d) => DateTime.DaysInMonth(d.Year, d.Month);

}

namespace System;

public static class DateTimeExtensions_DotNet {

  public static int GetAge(this DateTime birthDate, DateTime asAtDate) => birthDate.ToDateOnly().GetAge(asAtDate);
  public static int GetAge(this DateTime birthDate, DateOnly asAtDate) => birthDate.ToDateOnly().GetAge(asAtDate);

  public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);
  public static TimeOnly ToTimeOnly(this DateTime dateTime) => TimeOnly.FromDateTime(dateTime);

}

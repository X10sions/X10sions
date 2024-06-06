namespace System;
public static class DateOnlyExtensions {
  public static int GetAge(this DateOnly birthDate, DateOnly asAtDate) {
    var age = asAtDate.Year - birthDate.Year;
    if (birthDate > asAtDate.AddYears(-age)) age--;
    return age;
  }

  public static int GetAge(this DateOnly birthDate, DateTime asAtDate) => birthDate.GetAge(asAtDate.ToDateOnly());
  public static DateTime ToDateTime(this DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);
  public static DateTime ToDateMaxTime(this DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MaxValue);
}

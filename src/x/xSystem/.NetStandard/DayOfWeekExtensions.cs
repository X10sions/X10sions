namespace System {
  public static class DayOfWeekExtensions {
    public static bool IsWeekend(this DayOfWeek dow) => dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday;
    public static bool IsWeekDay(this DayOfWeek dow) => !dow.IsWeekend();

    public static DayOfWeek NextDayOfWeek(this DayOfWeek dayOfWeek) {
      switch (dayOfWeek) {
        case DayOfWeek.Sunday: return DayOfWeek.Monday;
        case DayOfWeek.Monday: return DayOfWeek.Tuesday;
        case DayOfWeek.Tuesday: return DayOfWeek.Wednesday;
        case DayOfWeek.Wednesday: return DayOfWeek.Thursday;
        case DayOfWeek.Thursday: return DayOfWeek.Friday;
        case DayOfWeek.Friday: return DayOfWeek.Saturday;
        case DayOfWeek.Saturday: return DayOfWeek.Sunday;
        default: throw new Exception($"Unknown day of the week: {dayOfWeek}");
      }
    }

    public static DayOfWeek PreviousDayOfWeek(this DayOfWeek dayOfWeek) {
      switch (dayOfWeek) {
        case DayOfWeek.Sunday: return DayOfWeek.Saturday;
        case DayOfWeek.Monday: return DayOfWeek.Sunday;
        case DayOfWeek.Tuesday: return DayOfWeek.Monday;
        case DayOfWeek.Wednesday: return DayOfWeek.Tuesday;
        case DayOfWeek.Thursday: return DayOfWeek.Wednesday;
        case DayOfWeek.Friday: return DayOfWeek.Thursday;
        case DayOfWeek.Saturday: return DayOfWeek.Friday;
        default: throw new Exception($"Unknown day of the week: {dayOfWeek}");
      }
    }

  }
}
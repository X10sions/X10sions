namespace Common.Interfaces;

public interface IDateTimeString : IDateString, ITimeString {
  public string DateTimeString { get; }
}

public interface IDateString {
  public string DateString { get; }
}

public interface ITimeString {
  public string TimeString { get; }
}

public static class IDateTimeStringExtensions {

  public static string ToDateTimeString(this IDateTimeString dt) => dt.DateString + dt.TimeString;
}
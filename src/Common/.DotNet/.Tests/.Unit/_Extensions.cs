
namespace NSubstitute;
public static class GetSubstitute {

  public static TimeProvider ForTimeProvider() {
    var timeProvider = Substitute.For<TimeProvider>();
    timeProvider.LocalTimeZone.Returns(TimeZoneInfo.Utc);
    timeProvider.TimestampFrequency.Returns(TimeProvider.System.TimestampFrequency);
    return timeProvider;
  }

}

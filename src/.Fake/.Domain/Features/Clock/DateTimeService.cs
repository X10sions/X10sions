
namespace Common.Features.DummyFakeExamples.Clock;

public class DateTimeService : IDateTimeService {
  public DateTime Now { get; } = DateTime.Now;
}

public class SystemDateTimeService : IDateTimeService {
  public DateTime Now => DateTime.UtcNow;
}

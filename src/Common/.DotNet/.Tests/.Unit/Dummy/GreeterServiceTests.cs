using FluentAssertions;
using NSubstitute;
using Xunit.Abstractions;

namespace Common.Dummy;

public class GreeterService(TimeProvider timeProvider) {
  public const string Morning = "Morning";
  public const string Afternoon = "Afternoon";
  public const string Evening = "Evening";
  public string GenerateText() => timeProvider.GetLocalNow().Hour switch {
    >= 5 and < 12 => Morning,
    >= 12 and < 18 => Afternoon,
    _ => Evening
  };
}

public class GreeterServiceTests {

  public GreeterServiceTests(ITestOutputHelper testOutputHelper) {
    this.testOutputHelper = testOutputHelper;
    _sut = new GreeterService(timeProvider);
  }

  readonly ITestOutputHelper testOutputHelper;
  readonly TimeProvider timeProvider = GetSubstitute.ForTimeProvider();
  readonly GreeterService _sut;

  [Fact]
  public void GenerateText_ShouldReturnAfternoon_WhenItIsAfternoon() {
    timeProvider.GetUtcNow().Returns(new DateTimeOffset(2024, 7, 24, 15, 0, 0, TimeSpan.Zero));
    testOutputHelper.WriteLine($"Utc: {timeProvider.GetUtcNow()} Local:{timeProvider.GetLocalNow()}");

    _sut.GenerateText().Should().Be(GreeterService.Afternoon);
  }

  [Fact]
  public void GenerateText_ShouldReturnEvening_WhenItIsEvening() {
    timeProvider.GetUtcNow().Returns(new DateTimeOffset(2024, 7, 24, 22, 0, 0, TimeSpan.Zero));
    testOutputHelper.WriteLine($"Utc: {timeProvider.GetUtcNow()} Local:{timeProvider.GetLocalNow()}");

    _sut.GenerateText().Should().Be(GreeterService.Evening);
  }

  [Fact]
  public void GenerateText_ShouldReturnMorning_WhenItIsMorning() {
    timeProvider.GetUtcNow().Returns(new DateTimeOffset(2024, 7, 24, 8, 0, 0, TimeSpan.Zero));
    testOutputHelper.WriteLine($"Utc: {timeProvider.GetUtcNow()} Local:{timeProvider.GetLocalNow()}");

    _sut.GenerateText().Should().Be(GreeterService.Morning);
  }


}

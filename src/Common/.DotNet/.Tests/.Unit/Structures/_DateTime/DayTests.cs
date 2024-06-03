using FluentAssertions;
using Xunit;
namespace Common.Structures;

public class DayTests {
  public static TheoryData<int, int> ValueExpectedData => new() {
    { -10, Day.MinValue },
    { -1, Day.MinValue },
    {  0, Day.MinValue},
    { 01, 01},
    { 09, 09},
    { 12, 12},
    { 20, 20},
    { 30, 30},
    { 31, Day.MaxValue},
    { 32, Day.MaxValue},
    { 99, Day.MaxValue},
    { 100, Day.MaxValue},
    { 1000, Day.MaxValue},
    { 10000, Day.MaxValue},
  };

  [Theory, MemberData(nameof(ValueExpectedData))] public void Value_ShouldBeExpected_GivenValue(int given, int expected) => new Day(given).Value.Should().Be(expected);

}

using FluentAssertions;
using Xunit;
namespace Common.Structures;
public class HourTests {
  public static TheoryData<int, int> ValueExpectedData => new() {
    { -1, Hour.MinValue },
    {  0, Hour.MinValue},
    {  1, 1 },
    { 23, 23 },
    { 24, Hour.MaxValue},
    { 25, Hour.MaxValue},
    { 99, Hour.MaxValue},
  };
  [Theory, MemberData(nameof(ValueExpectedData))] public void Value_ShouldBeExpected_GivenValue(int given, int expected) => new Hour(given).Value.Should().Be(expected);

}

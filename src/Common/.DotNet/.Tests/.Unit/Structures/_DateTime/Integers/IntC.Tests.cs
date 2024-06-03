using FluentAssertions;
using Xunit;

namespace Common.Structures {
  public class IntCTests {
    public static TheoryData<int, int> ValueExpectedData => new() {
      { -10, Day.MinValue },
      { -1, Day.MinValue },
      {  0, Day.MinValue},
      {  1, 01},
      {  2, 09},
      {  3, 09},
      {  4, 09},
      {  5, 09},
      {  6, 09},
      {  7, 09},
      {  8, 09},
      {  9, IntC.MaxValue},
      { 10, IntC.MaxValue},
      { 12, IntC.MaxValue},
      { 20, IntC.MaxValue},
      { 30, IntC.MaxValue},
      { 31, IntC.MaxValue},
      { 32, IntC.MaxValue},
      { 99, IntC.MaxValue},
      { 100, IntC.MaxValue},
      { 1000, IntC.MaxValue},
      { 10000, IntC.MaxValue},
    };

    [Theory, MemberData(nameof(ValueExpectedData))] public void Value_ShouldBeExpected_GivenValue(int given, int expected) => new IntC(given).Value.Should().Be(expected);

  }

}
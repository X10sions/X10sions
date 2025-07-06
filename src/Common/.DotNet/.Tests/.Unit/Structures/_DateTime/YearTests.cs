using FluentAssertions;
using Xunit;
namespace Common.Structures;

public class YearTests {

  public static TheoryData<int, int> GivenExpectedData => new() {
    {    -1, Year.MinValue },
    {     0, Year.MinValue },
    {     1, 1 },
    {  1900, 1900 },
    {  2000, 2000 },
    {  2024, 2024 },
    {  9998, 9998},
    {  9999, Year.MaxValue },
    { 10000, Year.MaxValue },
  };

  [Theory, MemberData(nameof(GivenExpectedData))] public void Value_ShouldBeExpected_GivenValue(int given, int expected) => new Year(given).Value.Should().Be(expected);

}
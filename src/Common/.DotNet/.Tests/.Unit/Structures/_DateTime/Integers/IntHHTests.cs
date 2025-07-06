using Common.Testing.XUnit;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace Common.Structures;

public class IntHHTests(ITestOutputHelper testOutputHelper) {
  TestOutputHelperTextWriter testOutputHelperTextWriter = TestOutputHelperTextWriter.UseConsole(testOutputHelper);

  public record GivenValue(int InputValue, int ExpectedValue, int HourValue) {
    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
      new (  -10, IntHH.MinValue, Hour.MinValue ),
      //new (   -1, IntHH.MinValue, Hour.MinValue ),
      //new (    0, IntHH.MinValue, Hour.MinValue),
      //new (    1, 01, 01 ),
      //new (    2, 02, 02 ),
      //new (    4, 04, 04 ),
      //new (    8, 08, 08 ),
      //new (   10, 10, 10 ),
      //new (   12, 12, 12 ),
      //new (   13, 13, 13 ),
      //new (   15, 15, 15 ),
      //new (   16, 16, 16 ),
      //new (   20, 20, 20 ),
      //new (   21, 21, 21 ),
      //new (   22, 22, 22 ),
      //new (   23, 23, Hour.MaxValue ),
      //new (   24, 24, Hour.MaxValue ),
      //new (   25, 25, Hour.MaxValue ),
      //new (   30, 30, Hour.MaxValue ),
      //new (   31, 31, Hour.MaxValue ),
      //new (   98, 98, Hour.MaxValue ),
      //new (   99, IntHH.MaxValue, Hour.MaxValue ),
      //new (  100, IntHH.MaxValue, Hour.MaxValue ),
      new ( 1000, IntHH.MaxValue, Hour.MaxValue ),
      new (10000, IntHH.MaxValue, Hour.MaxValue ),
      };
  }

  //[Theory, MemberData(nameof(TheoryData), MemberType = typeof(GivenValue))]
  //public void Value_ShouldBeExpected_GivenValue(GivenValue data) {
  //  var sut = new IntHH(data.InputValue);
  //  testOutputHelper.WriteLine($"sut.Call-01: {sut.Hour}");
  //  testOutputHelper.WriteLine($"sut.Call-02: {sut.Hour}");
  //  testOutputHelper.WriteLine($"sut.Call-03: {sut.Hour}");
  //  using (new AssertionScope()) {
  //    sut.Value.Should().Be(data.ExpectedValue);
  //    sut.Hour.Value.Should().Be(data.HourValue);
  //  }
  //}

}
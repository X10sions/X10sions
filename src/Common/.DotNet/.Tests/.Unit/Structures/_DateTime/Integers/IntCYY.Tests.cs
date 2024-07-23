using FluentAssertions.Execution;
using FluentAssertions;
using Xunit;

namespace Common.Structures;

public class IntCYYTests {

  //[Fact]
  //public void Year_ShouldBeToday_WhenConstructorIsEmpty() {
  //  var sut = new IntCYY();
  //  var expectedYear = DateTime.Today.Year;
  //  using (new AssertionScope()) {
  //    sut.Year.Should().Be(new Year(expectedYear));
  //    sut.YYYY.Should().Be(expectedYear);
  //  }
  //}

  public record GivenValue(int InputValue, int ExpectedValue, Year Year, int C, int YY, int YYYY) {
    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
        new (     -1,   0, new(1900), 0,    0,  1900),
        new (      0,   0, new(1900), 0,    0,  1900),
        new (      1,   1, new(1901), 0,    1,  1901),
        new (     98,  98, new(1998), 0,   98,  1998),
        new (     99,  99, new(1999), 0,   99,  1999),
        new (    100, 100, new(2000), 1,   00,  2000),
        new (    101, 101, new(2001), 1,   01,  2001),
        new (    117, 117, new(2017), 1,   17,  2017),
        new (    120, 120, new(2020), 1,   20,  2020),
        new (    199, 199, new(2099), 1,   99,  2099),
        new (    899, 899, new(2799), 8,   99,  2799),
        new (    900, 900, new(2800), 9,   00,  2800),
        new (    999, 999, new(2899), 9,   99,  2899),
        new (   1000, 999, new(2899), 9,   99,  2899),
      };
  }

  //[Theory, MemberData(nameof(TheoryData), MemberType = typeof(GivenValue))]
  //public void Year_ShouldBeExpected_WhenConstructorIsValue(GivenValue data) {
  //  var sut = new IntCYY(data.InputValue);
  //  using (new AssertionScope()) {
  //    sut.C.Should().Be(data.C);
  //    sut.YY.Should().Be(data.YY);
  //    sut.YYYY.Should().Be(data.YYYY);
  //    sut.Year.Should().Be(data.Year);
  //    sut.Value.Should().Be(data.ExpectedValue);
  //  }
  //}

}
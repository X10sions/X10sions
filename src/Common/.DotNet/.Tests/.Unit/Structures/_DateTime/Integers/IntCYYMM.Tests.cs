using FluentAssertions.Execution;
using FluentAssertions;
using Xunit;

namespace Common.Structures;
public class IntCYYMMTests {

  [Fact]
  public void Year_ShouldBeToday_WhenConstructorIsEmpty() {
    var sut = new IntCYYMM();
    var today = DateTime.Today;
    using (new AssertionScope()) {
      sut.Year.Should().Be(new Year(today.Year));
      sut.Month.Should().Be(new Month(today.Month));
      sut.YYYY.Should().Be(today.Year);
    }
  }

  public record GivenValue(int InputValue, int ExpectedValue, int C, int YY, int MM, int YearValue, int MonthValue) {
    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
        new (    -1,      0, 0,  0,  0, 1900,  1),
        new (     0,      0, 0,  0,  0, 1900,  1),
        new (     1,      1, 0,  0,  1, 1900,  1),
        new (     5,      5, 0,  0,  5, 1900,  5),
        new (    98,     98, 0,  0, 98, 1900, 12),
        new (    99,     99, 0,  0, 99, 1900, 12),
        new (   100,    100, 0,  1, 00, 1901,  1),
        new (   101,    101, 0,  1, 01, 1901,  1),
        new (   117,    117, 0,  1, 17, 1901, 12),
        new (   120,    120, 0,  1, 20, 1901, 12),
        new (   199,    199, 0,  1, 99, 1901, 12),
        new (   899,    899, 0,  8, 99, 1908, 12),
        new (   900,    900, 0,  9, 00, 1909,  1),
        new (   999,    999, 0,  9, 99, 1909, 12),
        new (  1000,   1000, 0, 10, 00, 1910,  1),
        new (  9900,   9900, 0, 99, 00, 1999,  1),
        new (  9912,   9912, 0, 99, 12, 1999, 12),
        new ( 10000,  10000, 1, 00, 00, 2000,  1),
        new ( 10001,  10001, 1, 00, 01, 2000,  1),
        new ( 10011,  10011, 1, 00, 11, 2000, 11),
        new ( 10101,  10101, 1, 01, 01, 2001,  1),
        new ( 10111,  10111, 1, 01, 11, 2001, 11),
        new ( 10799,  10799, 1, 07, 99, 2007, 12),
        new ( 11111,  11111, 1, 11, 11, 2011, 11),
        new ( 11712,  11712, 1, 17, 12, 2017, 12),
        new ( 11901,  11901, 1, 19, 01, 2019,  1),
        new ( 12002,  12002, 1, 20, 02, 2020,  2),
        new ( 12102,  12102, 1, 21, 02, 2021,  2),
        new ( 12400,  12400, 1, 24, 00, 2024,  1),
        new ( 12413,  12413, 1, 24, 13, 2024, 12),
        new ( 99801,  99801, 9, 98, 01, 2898,  1),
        new ( 99901,  99901, 9, 99, 01, 2899,  1),
        new ( 99912,  99912, 9, 99, 12, 2899, 12),
        new ( 99999,  99999, 9, 99, 99, 2899, 12),
        new (100000,  99999, 9, 99, 99, 2899, 12),
      };
  }

  [Theory, MemberData(nameof(TheoryData), MemberType = typeof(GivenValue))]
  public void DateOnly_ShouldBeExpected_WhenConstructorIsValue(GivenValue data) {
    var sut = new IntCYYMM(data.InputValue);
    using (new AssertionScope()) {
      sut.Value.Should().Be(data.ExpectedValue);
      sut.C.Should().Be(data.C);
      sut.YY.Should().Be(data.YY);
      sut.MM.Should().Be(data.MM);
      sut.Year.Value.Should().Be(data.YearValue);
      sut.YYYY.Should().Be(data.YearValue);
      sut.Month.Value.Should().Be(data.MonthValue);
    }
  }
 
}
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Common.Structures;

public class IntCYYMMDDTests {

  [Fact]
  public void DateOnly_ShouldBeToday_WhenConstructorIsEmpty() {
    var sut = new IntCYYMMDD();
    using (new AssertionScope()) {
      sut.DateOnly.Should().Be(DateTime.Today.ToDateOnly());
      sut.DateTime.Should().Be(DateTime.Today);
    }
  }

  public record GivenDateOnly(int Value, DateOnly DateOnly, int C, int YY, int MM, int DD) {
    public DateTime DateTime { get; } = DateOnly.ToDateTime(TimeOnly.MinValue);
    public static TheoryData<GivenDateOnly> TheoryData { get; } = new TheoryData<GivenDateOnly> {
      new (  10101, new(   1,  1,  1), 0,  1,  1,  1),
      new (  10101, new(  10,  1,  1), 0,  1,  1,  1),
      new (  10101, new( 100,  1,  1), 0,  1,  1,  1),
      new (  10101, new(1000,  1,  1), 0,  1,  1,  1),
      new (  10101, new(1901,  1,  1), 0,  1,  1,  1),
      new ( 990724, new(1999,  7, 24), 0, 99,  7, 24),
      new ( 990799, new(1999,  7, 31), 0, 99,  7, 99),
      new (1010101, new(2001,  1,  1), 1,  1,  1,  1),
      new (1170831, new(2017,  8, 31), 1, 17,  8, 31),
      new (1200229, new(2020,  2, 29), 1, 20,  2, 29),
      new (1210229, new(2021,  2, 28), 1, 21,  2, 28),
      new (1210229, new(2021,  2, 29), 1, 21,  2, 29),
      new (1240101, new(2024,  1,  1), 1, 24,  1,  1),
      new (1241100, new(2024, 11,  1), 1, 24, 11,  0),
      new (1241130, new(2024, 11, 30), 1, 24, 11, 30),
      new (1241201, new(2024, 12, 11), 1, 24, 12,  1),
      new (1241231, new(2024, 12, 31), 1, 24, 12, 31),
      new (9999999, new(9999, 99, 29), 9, 99, 99, 99),
    };
  }

  [Theory, MemberData(nameof(GivenDateOnly.TheoryData), MemberType = typeof(GivenDateOnly))]
  public void Value_ShouldBeExpected_WhenConstructorIsDateOnly(GivenDateOnly data) {
    var sut = new IntCYYMMDD(data.DateOnly);
    using (new AssertionScope()) {
      sut.DateOnly.Should().Be(data.DateOnly);
      sut.DateTime.Should().Be(data.DateOnly.ToDateTime(TimeOnly.MinValue));
      sut.C.Should().Be(data.C);
      sut.YY.Should().Be(data.YY);
      sut.MM.Should().Be(data.MM);
      sut.DD.Should().Be(data.DD);
      sut.Value.Should().Be(data.Value);
    }
  }

  [Theory, MemberData(nameof(GivenDateOnly.TheoryData), MemberType = typeof(GivenDateOnly))]
  public void Value_ShouldBeExpected_WhenConstructorIsDateTime(GivenDateOnly data) {
    var sut = new IntCYYMMDD(data.DateTime);
    using (new AssertionScope()) {
      sut.DateOnly.Should().Be(data.DateOnly);
      sut.DateTime.Should().Be(data.DateOnly.ToDateTime(TimeOnly.MinValue));
      sut.C.Should().Be(data.C);
      sut.YY.Should().Be(data.YY);
      sut.MM.Should().Be(data.MM);
      sut.DD.Should().Be(data.DD);
      sut.Value.Should().Be(data.Value);
    }
  }

  public record GivenValue(int Value, DateOnly DateOnly, int C, int YY, int MM, int DD) {

    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
      new (     -1, new(1900,  1,  1), 0,  0,  0,  0),
      new (      0, new(1900,  1,  1), 0,  0,  0,  0),
      new (      1, new(1900,  1,  1), 0,  0,  0,  1),
      new (      5, new(1900,  1,  5), 0,  0,  0,  5),
      new (     99, new(1900,  1, 31), 0,  0,  0, 99),
      new (    100, new(1900,  1,  1), 0,  0,  1,  0),
      new (    101, new(1900,  1,  1), 0,  0,  1,  1),
      new (    500, new(1900,  5,  1), 0,  0,  5,  0),
      new (    999, new(1900,  9, 30), 0,  0,  9, 99),
      new (   1000, new(1900, 10,  1), 0,  0, 10,  0),
      new (   9912, new(1900, 12, 12), 0,  0, 99, 12),
      new (   9999, new(1900, 12, 31), 0,  0, 99, 99),
      new (  10000, new(1901,  1,  1), 0,  1,  0,  1),
      new (  10001, new(1901,  1,  1), 0,  1,  0,  1),
      new (  10101, new(1901,  1,  1), 0,  1,  1,  1),
      new (  99812, new(1909, 12, 12), 0,  9, 98, 12),
      new (  99912, new(1909, 12, 12), 0,  9, 99, 12),
      new ( 100000, new(1910,  1,  1), 1,  0,  0,  0),
      new ( 100101, new(1910,  1,  1), 1,  0,  1,  1),
      new ( 990000, new(1999,  1,  1), 0, 99,  0,  0),
      new ( 990724, new(1999,  7, 24), 0, 99,  7, 24),
      new ( 990799, new(1999,  7, 31), 0, 99,  7, 99),
      new ( 991231, new(1999, 12, 31), 0, 99, 12, 31),
      new (1000000, new(2000,  1,  1), 1,  0,  0,  0),
      new (1000101, new(2000,  1,  1), 1,  0,  1,  1),
      new (1010000, new(2001,  1,  1), 1,  1,  0,  0),
      new (1010101, new(2001,  1,  1), 1,  1,  1,  1),
      new (1079900, new(2007, 12,  1), 1,  7, 99,  0),
      new (1079999, new(2007, 12, 31), 1,  7, 99, 99),
      new (1010101, new(2001,  1,  1), 1,  1,  1,  1),
      new (1160231, new(2016,  2, 29), 1, 16,  2, 31),
      new (1170231, new(2017,  2, 28), 1, 17,  2, 31),
      new (1170831, new(2017,  8, 31), 1, 17,  8, 31),
      new (1170832, new(2017,  8, 31), 1, 17,  8, 32),
      new (1200229, new(2020,  2, 29), 1, 20,  2, 29),
      new (1210228, new(2021,  2, 28), 1, 21,  2, 28),
      new (1210229, new(2021,  2, 29), 1, 21,  2, 29),
      new (1240000, new(2024,  1,  1), 1, 24,  0,  0),
      new (1240100, new(2024,  1,  1), 1, 24,  1,  0),
      new (1240101, new(2024,  1,  1), 1, 24,  1,  1),
      new (1241100, new(2024, 11,  1), 1, 24, 11,  0),
      new (1241130, new(2024, 11, 30), 1, 24, 11, 30),
      new (1241131, new(2024, 11, 30), 1, 24, 11, 31),
      new (1241199, new(2024, 11, 30), 1, 24, 11, 99),
      new (1241201, new(2024, 12, 11), 1, 24, 12,  1),
      new (1241232, new(2024, 12, 31), 1, 24, 12, 32),
      new (1249999, new(2024, 12, 31), 1, 24, 99, 99),
      new (9990000, new(2899,  1,  1), 9, 99, 00, 00),
      new (9990101, new(2899,  1,  1), 9, 99, 01, 01),
      new (9991231, new(2899, 12, 31), 9, 99, 12, 31),
      new (9999931, new(2899, 12, 31), 9, 99, 99, 31),
      new (9999932, new(2899, 12, 31), 9, 99, 99, 32),
      new (9999999, new(9999, 99, 29), 9, 99, 99, 99),
    };
  }

  [Theory, MemberData(nameof(GivenValue.TheoryData), MemberType = typeof(GivenValue))]
  public void DateOnly_ShouldBeExpected_WhenConstructorIsValue(GivenValue data) {
    var sut = new IntCYYMMDD(data.Value);
    using (new AssertionScope()) {
      sut.DateOnly.Should().Be(data.DateOnly);
      sut.DateTime.Should().Be(data.DateOnly.ToDateTime(TimeOnly.MinValue));
      sut.C.Should().Be(data.C);
      sut.YY.Should().Be(data.YY);
      sut.MM.Should().Be(data.MM);
      sut.DD.Should().Be(data.DD);
      sut.Value.Should().Be(data.Value);
    }
  }

}
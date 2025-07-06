using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace Common.Structures;

public class IntCYYMMDDTests(ITestOutputHelper testOutputHelper) {

  [Fact]
  public void ConstructorTests_GivenEmpty_ShouldBeToday() {
    var sut = new IntCYYMMDD();
    var expected = TimeProvider.System.GetLocalNow().Date;
    using (new AssertionScope()) {
      sut.Year.Value.Should().Be(expected.Year);
      sut.Month.Value.Should().Be(expected.Month);
      sut.DateOnly.Should().Be(expected.ToDateOnly());
      sut.DateTime.Should().Be(expected);
    }
  }

  public record GivenYearMonthDay(int Year, int Month, int Day, int ExpectedValue) {
    public DateOnly DateOnly { get; } = new(Year, Month, Day);
    public DateTime DateTime { get; } = new(Year, Month, Day, 0, 0, 0, DateTimeKind.Unspecified);
    public static TheoryData<GivenYearMonthDay> TheoryData { get; } = new TheoryData<GivenYearMonthDay> {
      new(   1,  1,  1,   IntCYYMMDD.MinValue),
      new(  10,  1,  1,   IntCYYMMDD.MinValue),
      new( 100,  1,  1,   IntCYYMMDD.MinValue),
      new(1000,  1,  1,   IntCYYMMDD.MinValue),
      new(1900,  1,  1,     101),
      new(1901,  1,  1,   10101),
      new(1999,  7, 24,  990724),
      new(1999,  7, 31,  990731),
      new(2001,  1,  1, 1010101),
      new(2017,  8, 31, 1170831),
      new(2020,  2, 29, 1200229),
      new(2021,  2, 28, 1210228),
      new(2024,  1,  1, 1240101),
      new(2024, 11,  1, 1241101),
      new(2024, 11, 30, 1241130),
      new(2024, 12,  1, 1241201),
      new(2024, 12, 31, 1241231),
      new(2899, 12, 31, 9991231),
      new(2900,  1,  1, 9999999),
      new(9999, 12, 31, 9999999),
    };
  }

  [Theory, MemberData(nameof(GivenYearMonthDay.TheoryData), MemberType = typeof(GivenYearMonthDay))]
  public void Value_GivenDateOnly(GivenYearMonthDay data) {
    var sut = new IntCYYMMDD(data.DateOnly);
    using (new AssertionScope()) {
      sut.Value.Should().Be(data.ExpectedValue);
    }
  }


  [Theory, MemberData(nameof(GivenYearMonthDay.TheoryData), MemberType = typeof(GivenYearMonthDay))]
  public void Value_GivenDateTime(GivenYearMonthDay data) {
    var sut = new IntCYYMMDD(data.DateTime);
    using (new AssertionScope()) {
      sut.Value.Should().Be(data.ExpectedValue);
    }
  }

  public record GivenValue(int Value, int ExpectedValue, DateOnly ExpectedDateOnly, int ExpectedC, int ExpectedYY, int ExpectedMM, int ExpectedDD) {

    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
      new (      -1, IntCYYMMDD.MinValue, new(1900,  1,  1), 0,  0,  0,  0),
      new (       0, IntCYYMMDD.MinValue, new(1900,  1,  1), 0,  0,  0,  0),
      new (       1,       1, new(1900,  1,  1), 0,  0,  0,  1),
      new (       5,       5, new(1900,  1,  5), 0,  0,  0,  5),
      new (      99,      99, new(1900,  1, 31), 0,  0,  0, 99),
      new (     100,     100, new(1900,  1,  1), 0,  0,  1,  0),
      new (     101,     101, new(1900,  1,  1), 0,  0,  1,  1),
      new (     500,     500, new(1900,  5,  1), 0,  0,  5,  0),
      new (     999,     999, new(1900,  9, 30), 0,  0,  9, 99),
      new (    1000,    1000, new(1900, 10,  1), 0,  0, 10,  0),
      new (    9912,    9912, new(1900, 12, 12), 0,  0, 99, 12),
      new (    9999,    9999, new(1900, 12, 31), 0,  0, 99, 99),
      new (   10000,   10000, new(1901,  1,  1), 0,  1,  0,  0),
      new (   10001,   10001, new(1901,  1,  1), 0,  1,  0,  1),
      new (   10101,   10101, new(1901,  1,  1), 0,  1,  1,  1),
      new (   99812,   99812, new(1909, 12, 12), 0,  9, 98, 12),
      new (   99912,   99912, new(1909, 12, 12), 0,  9, 99, 12),
      new (  100000,  100000, new(1910,  1,  1), 0, 10,  0,  0),
      new (  100101,  100101, new(1910,  1,  1), 0, 10,  1,  1),
      new (  990000,  990000, new(1999,  1,  1), 0, 99,  0,  0),
      new (  990724,  990724, new(1999,  7, 24), 0, 99,  7, 24),
      new (  990799,  990799, new(1999,  7, 31), 0, 99,  7, 99),
      new (  991231,  991231, new(1999, 12, 31), 0, 99, 12, 31),
      new ( 1000000, 1000000, new(2000,  1,  1), 1,  0,  0,  0),
      new ( 1000101, 1000101, new(2000,  1,  1), 1,  0,  1,  1),
      new ( 1010000, 1010000, new(2001,  1,  1), 1,  1,  0,  0),
      new ( 1010101, 1010101, new(2001,  1,  1), 1,  1,  1,  1),
      new ( 1079900, 1079900, new(2007, 12,  1), 1,  7, 99,  0),
      new ( 1079999, 1079999, new(2007, 12, 31), 1,  7, 99, 99),
      new ( 1010101, 1010101 ,new(2001,  1,  1), 1,  1,  1,  1),
      new ( 1111111, 1111111, new(2011, 11, 11), 1, 11, 11, 11),
      new ( 1160231, 1160231, new(2016,  2, 29), 1, 16,  2, 31),
      new ( 1170231, 1170231, new(2017,  2, 28), 1, 17,  2, 31),
      new ( 1170831, 1170831, new(2017,  8, 31), 1, 17,  8, 31),
      new ( 1170832, 1170832, new(2017,  8, 31), 1, 17,  8, 32),
      new ( 1200229, 1200229, new(2020,  2, 29), 1, 20,  2, 29),
      new ( 1210228, 1210228, new(2021,  2, 28), 1, 21,  2, 28),
      new ( 1210229, 1210229, new(2021,  2, 28), 1, 21,  2, 29),
      new ( 1240000, 1240000, new(2024,  1,  1), 1, 24,  0,  0),
      new ( 1240100, 1240100, new(2024,  1,  1), 1, 24,  1,  0),
      new ( 1240101, 1240101, new(2024,  1,  1), 1, 24,  1,  1),
      new ( 1240325, 1240325, new(2024,  3, 25), 1, 24,  3, 25),
      new ( 1240417, 1240417, new(2024,  4, 17), 1, 24,  4, 17),
      new ( 1241100, 1241100, new(2024, 11,  1), 1, 24, 11,  0),
      new ( 1241130, 1241130, new(2024, 11, 30), 1, 24, 11, 30),
      new ( 1241131, 1241131, new(2024, 11, 30), 1, 24, 11, 31),
      new ( 1241199, 1241199, new(2024, 11, 30), 1, 24, 11, 99),
      new ( 1241201, 1241201, new(2024, 12,  1), 1, 24, 12,  1),
      new ( 1241231, 1241231, new(2024, 12, 31), 1, 24, 12, 31),
      new ( 1241232, 1241232, new(2024, 12, 31), 1, 24, 12, 32),
      new ( 1249999, 1249999, new(2024, 12, 31), 1, 24, 99, 99),
      new ( 9990000, 9990000, new(2899,  1,  1), 9, 99, 00, 00),
      new ( 9990101, 9990101, new(2899,  1,  1), 9, 99, 01, 01),
      new ( 9991231, 9991231, new(2899, 12, 31), 9, 99, 12, 31),
      new ( 9999931, 9999931, new(2899, 12, 31), 9, 99, 99, 31),
      new ( 9999932, 9999932, new(2899, 12, 31), 9, 99, 99, 32),
      new ( 9999999, IntCYYMMDD.MaxValue, new(2899, 12, 31), 9, 99, 99, 99),
      new (19999999, IntCYYMMDD.MaxValue,new(2899, 12, 31), 9, 99, 99, 99),
    };
  }

  [Theory, MemberData(nameof(GivenValue.TheoryData), MemberType = typeof(GivenValue))]
  public void ConstructorTests_GivenValue(GivenValue data) {
    var sut = new IntCYYMMDD(data.Value);
    using (new AssertionScope()) {
      sut.DateOnly.Should().Be(data.ExpectedDateOnly);
      sut.DateTime.Should().Be(data.ExpectedDateOnly.ToDateTime(TimeOnly.MinValue));
      sut.C.Should().Be(data.ExpectedC);
      sut.YY.Should().Be(data.ExpectedYY);
      sut.MM.Should().Be(data.ExpectedMM);
      sut.DD.Should().Be(data.ExpectedDD);
      sut.Value.Should().Be(data.ExpectedValue);
    }
  }

}
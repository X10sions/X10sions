using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Common.Structures;

public class IntHHMMSSTests {

  [Fact]
  public void DateOnly_ShouldBeToday_WhenConstructorIsEmpty() {
    var sut = new IntHHMMSS();
    var now = DateTime.Now;
    using (new AssertionScope()) {
      sut.TimeOnly.Should().Be(now.ToTimeOnly());
      //sut.DateTime.Should().Be(now);
    }
  }

  public record GivenTimeOnly(int Value, TimeOnly TimeOnly, int HH, int MM, int SS) {
    //public DateTime DateTime { get; } = DateOnly.ToDateTime(TimeOnly.MinValue);
    public static TheoryData<GivenTimeOnly> TheoryData { get; } = new TheoryData<GivenTimeOnly> {
      new (      0, new( 0,  0,  0) , 0,  0,  0),
      new (      1, new( 0,  0,  1) , 0,  0,  1),
      new (     12, new( 0,  0, 12) , 0,  0, 12),
      new (    123, new( 0,  1, 23) , 0,  1, 23),
      new (   1234, new( 0, 12, 34) , 0, 12, 34),
      new (  10101, new( 1,  1,  1) , 1,  1,  1),
      new (  12345, new( 1, 23, 45) , 1, 23, 45),
      new ( 123456, new(12, 34, 56), 12, 34, 56),
      new (9999999, new(23, 59, 59), 23, 59, 59),
    };
  }

  [Theory, MemberData(nameof(GivenTimeOnly.TheoryData), MemberType = typeof(GivenTimeOnly))]
  public void Value_ShouldBeExpected_WhenConstructorIsTimeOnly(GivenTimeOnly data) {
    var sut = new IntHHMMSS(data.TimeOnly);
    using (new AssertionScope()) {
      sut.TimeOnly .Should().Be(data.TimeOnly);
      sut.HH.Should().Be(data.HH);
      sut.MM.Should().Be(data.MM);
      sut.SS.Should().Be(data.SS);
      sut.Value.Should().Be(data.Value);
    }
  }

  //[Theory, MemberData(nameof(GivenTimeOnly.TheoryData), MemberType = typeof(GivenTimeOnly))]
  //public void Value_ShouldBeExpected_WhenConstructorIsDateTime(GivenTimeOnly data) {
  //  var sut = new IntHHMMSS(data.DateTime);
  //  using (new AssertionScope()) {
  //    sut.DateOnly.Should().Be(data.DateOnly);
  //    sut.DateTime.Should().Be(data.DateOnly.ToDateTime(TimeOnly.MinValue));
  //    sut.C.Should().Be(data.C);
  //    sut.YY.Should().Be(data.YY);
  //    sut.MM.Should().Be(data.MM);
  //    sut.DD.Should().Be(data.DD);
  //    sut.Value.Should().Be(data.Value);
  //  }
  //}

  public record GivenValue(int Value, TimeOnly TimeOnly, int HH, int MM, int SS) {

    public static readonly TheoryData<GivenValue> TheoryData = new TheoryData<GivenValue> {
      new (     -1, new( 0,  0,  0),  0,  0,  0),
      new (      0, new( 0,  0,  0),  0,  0,  0),
      new (      1, new( 0,  0,  1),  0,  0,  1),
      new (      9, new( 0,  0,  9),  0,  0,  9),
      new (     10, new( 0,  0, 10),  0,  0, 10),
      new (     12, new( 0,  0, 12),  0,  0, 12),
      new (     59, new( 0,  0, 59),  0,  0, 59),
      new (     60, new( 0,  0, 59),  0,  0, 60),
      new (     99, new( 0,  0, 59),  0,  0, 99),
      new (    100, new( 0,  1,  0),  0,  1,  0),
      new (    101, new( 0,  1,  1),  0,  1,  1),
      new (    111, new( 0,  1, 11),  0,  1, 11),
      new (    123, new( 0,  1, 23),  0,  1, 23),
      new (    160, new( 0,  1, 59),  0,  1, 60),
      new (    555, new( 0,  5, 55),  0,  5, 55),
      new (    666, new( 0,  6, 59),  0,  6, 66),
      new (    777, new( 0,  7, 59),  0,  7, 77),
      new (    999, new( 0,  9, 59),  0,  9, 99),
      new (   1000, new( 0, 10,  0),  0, 10,  0),
      new (   1001, new( 0, 10,  1),  0, 10,  1),
      new (   1234, new( 0, 12, 34),  0, 12, 34),
      new (   6666, new( 0, 59, 59),  0, 66, 66),
      new (   9912, new( 0, 59, 12),  0, 99, 12),
      new (   9999, new( 0, 59, 59),  0, 99, 99),
      new (  10000, new( 1,  0,  0),  1,  0,  0),
      new (  10001, new( 1,  0,  1),  1,  0,  1),
      new (  10101, new( 1,  1,  1),  1,  1,  1),
      new (  12345, new( 1, 23, 45),  1, 23, 45),
      new (  66666, new( 6, 59, 59),  6, 66, 66),
      new (  99812, new( 9, 59, 12),  9, 98, 12),
      new (  99912, new( 9, 59, 12),  9, 99, 12),
      new ( 100000, new(10,  0,  0), 10,  0,  0),
      new ( 100101, new(10,  1,  1), 10,  1,  1),
      new ( 240000, new(23,  0,  0), 24,  0,  0),
      new ( 249999, new(23, 59, 59), 24, 99, 99),
      new ( 990000, new(23,  0,  0), 99,  0,  0),
      new ( 990724, new(23,  7, 24), 99,  7, 24),
      new ( 990799, new(23,  7, 31), 99,  7, 99),
      new ( 991231, new(23, 12, 31), 99, 12, 31),
      new ( 999999, new(23, 59, 59), 99, 99, 99),
      new (1000000, new(23, 59, 59), 99,  0,  0),
    };
  }

  [Theory, MemberData(nameof(GivenValue.TheoryData), MemberType = typeof(GivenValue))]
  public void TimeOnlyOnly_ShouldBeExpected_WhenConstructorIsValue(GivenValue data) {
    var sut = new IntHHMMSS(data.Value);
    using (new AssertionScope()) {
      sut.TimeOnly.Should().Be(data.TimeOnly);
      //sut.DateTime.Should().Be(data.DateOnly.ToDateTime(TimeOnly.MinValue));
      sut.HH.Should().Be(data.HH);
      sut.MM.Should().Be(data.MM);
      sut.SS.Should().Be(data.SS);
      sut.Value.Should().Be(data.Value);
    }
  }

}
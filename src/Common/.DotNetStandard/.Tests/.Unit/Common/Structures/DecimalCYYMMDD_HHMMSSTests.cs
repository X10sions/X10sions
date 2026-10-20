using Xunit;

namespace Common.Structures;


public class DecimalCYYMMDD_HHMMSSTests {
  [Fact]
  public void DateOnly_ReturnsExpectedDate() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.DateOnly;
    Assert.Equal(new DateOnly(2025, 10, 20), actual);
  }

  [Fact]
  public void TimeOnly_ReturnsExpectedTime() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.TimeOnly;
    Assert.Equal(new TimeOnly(14, 30, 15), actual);
  }

  [Fact]
  public void DateTime_ReturnsExpectedDateAndTime() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.DateTime;
    Assert.Equal(new DateTime(2025, 10, 20, 14, 30, 15, DateTimeKind.Unspecified), actual);
  }

  [Fact]
  public void IntCYYMMDD_IsExtractedFromIntegerPart() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    Assert.Equal(1251020, value.IntCYYMMDD.Value);
  }

  [Fact]
  public void IntHHMMSS_IsExtractedFromFractionalPart() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    Assert.Equal(143015, value.IntHHMMSS.Value);
  }

  [Fact]
  public void ToStringYYYY_MM_DD_DoesNotRecurse() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.ToStringYYYY_MM_DD();
    Assert.Equal("2025-10-20", actual);
  }

  [Fact]
  public void ToStringHH_MM_SS_DoesNotRecurse() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.ToStringHH_MM_SS();
    Assert.Equal("14:30:15", actual);
  }

  [Fact]
  public void ToStringYYYY_MM_DD_HH_MM_SS_ReturnsExpectedValue() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var actual = value.ToStringYYYY_MM_DD_HH_MM_SS();
    Assert.Equal("2025-10-20 14:30:15", actual);
  }

  [Fact]
  public void DateTime_RoundTrip_PreservesValue() {
    var expected = new DateTime(2025, 10, 20, 14, 30, 15, DateTimeKind.Unspecified);
    var value = new DecimalCYYMMDD_HHMMSS(expected);
    Assert.Equal(expected, value.DateTime);
  }

  [Theory]
  [InlineData(101, 0, 1900, 1, 1, 0, 0, 0)]
  [InlineData(1240101, 1, 2024, 1, 1, 0, 0, 1)]
  [InlineData(1251020, 143015, 2025, 10, 20, 14, 30, 15)]
  [InlineData(1991231, 235959, 2099, 12, 31, 23, 59, 59)]
  public void Components_AreDecodedCorrectly(int cyymmdd, int hhmmss, int year, int month, int day, int hour, int minute, int second) {
    var value = new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss);
    Assert.Equal(year, value.DateTime.Year);
    Assert.Equal(month, value.DateTime.Month);
    Assert.Equal(day, value.DateTime.Day);
    Assert.Equal(hour, value.DateTime.Hour);
    Assert.Equal(minute, value.DateTime.Minute);
    Assert.Equal(second, value.DateTime.Second);
  }

  [Fact]
  public void Properties_CanBeAccessedRepeatedly() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    for (var i = 0; i < 10_000; i++) {
      Assert.Equal(1251020, value.IntCYYMMDD.Value);
      Assert.Equal(143015, value.IntHHMMSS.Value);
      Assert.Equal(2025, value.DateOnly.Year);
      Assert.Equal(14, value.TimeOnly.Hour);
      Assert.Equal(2025, value.DateTime.Year);
    }
  }




  [Fact]
  public void DateTime_Should_Not_StackOverflow() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var dt = value.DateTime;
    Assert.Equal(2025, dt.Year);
    Assert.Equal(10, dt.Month);
    Assert.Equal(20, dt.Day);
    Assert.Equal(14, dt.Hour);
    Assert.Equal(30, dt.Minute);
    Assert.Equal(15, dt.Second);
  }

  [Fact]
  public void DateOnly_Should_Be_Resolved() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 0);
    Assert.Equal(new DateOnly(2025, 10, 20), value.DateOnly);
  }

  [Fact]
  public void TimeOnly_Should_Be_Resolved() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    Assert.Equal(new TimeOnly(14, 30, 15), value.TimeOnly);
  }

  [Fact]
  public void RoundTrip_DateTime_Should_Match() {
    var original = new DateTime(2025, 10, 20, 14, 30, 15, DateTimeKind.Unspecified);
    var value = new DecimalCYYMMDD_HHMMSS(original);
    var rebuilt = value.DateTime;
    Assert.Equal(original, rebuilt);
  }

  [Fact]
  public void IntCYYMMDD_Should_Be_Correct() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    Assert.Equal(1251020, value.IntCYYMMDD.Value);
  }

  [Fact]
  public void IntHHMMSS_Should_Be_Correct() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    Assert.Equal(143015, value.IntHHMMSS.Value);
  }

  [Theory]
  [InlineData(1240101, 0)]
  [InlineData(1241231, 235959)]
  [InlineData(1250101, 123456)]
  [InlineData(1991231, 235959)]
  public void Conversion_Should_Not_Throw(int cyymmdd, int hhmmss) {
    var value = new DecimalCYYMMDD_HHMMSS(cyymmdd, hhmmss);
    var dt = value.DateTime;
    Assert.NotEqual(DateTime.MinValue, dt);
  }

  [Fact]
  public void DateTime_Can_Be_Accessed_Multiple_Times() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    for (int i = 0; i < 10000; i++) {
      _ = value.DateTime;
      _ = value.DateOnly;
      _ = value.TimeOnly;
      _ = value.IntCYYMMDD;
      _ = value.IntHHMMSS;
    }
  }

}

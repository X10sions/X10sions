using Xunit;

namespace Common.Structures;

public class IntHHMMSSTests {
  [Fact]
  public void Constructor_FromComponents_ProducesHHMMSS() {
    var value = new IntHHMMSS(14, 30, 15);
    Assert.Equal(143015, value.Value);
  }

  [Theory]
  [InlineData(0, 0, 0, 0)]
  [InlineData(1, 0, 0, 1)]
  [InlineData(30, 0, 0, 30)]
  [InlineData(100, 0, 1, 0)]
  [InlineData(10000, 1, 0, 0)]
  [InlineData(143015, 14, 30, 15)]
  [InlineData(235959, 23, 59, 59)]
  public void Components_AreDecodedCorrectly(int hhmmss, int expectedHour, int expectedMinute, int expectedSecond) {
    var value = new IntHHMMSS(hhmmss);
    Assert.Equal(expectedHour, value.HH);
    Assert.Equal(expectedMinute, value.MM);
    Assert.Equal(expectedSecond, value.SS);
  }

  [Fact]
  public void TimeOnly_ReturnsExpectedTime() {
    var value = new IntHHMMSS(143015);
    Assert.Equal(new TimeOnly(14, 30, 15), value.TimeOnly);
  }

  [Fact]
  public void Constructor_FromTimeOnly_RoundTrips() {
    var expected = new TimeOnly(14, 30, 15);
    var value = new IntHHMMSS(expected);
    Assert.Equal(143015, value.Value);
    Assert.Equal(expected, value.TimeOnly);
  }

  [Fact]
  public void Constructor_FromDateTime_UsesTimeComponent() {
    var source = new DateTime(2025, 10, 20, 14, 30, 15);
    var value = new IntHHMMSS(source);
    Assert.Equal(143015, value.Value);
  }

  [Theory]
  [InlineData(1251020.000000, 0)]
  [InlineData(1251020.000001, 1)]
  [InlineData(1251020.120000, 120000)]
  [InlineData(1251020.143015, 143015)]
  [InlineData(1251020.235959, 235959)]
  public void Constructor_FromCombinedDecimal_ExtractsTime(double combinedValue, int expectedTime) {
    var value = new IntHHMMSS((decimal)combinedValue);
    Assert.Equal(expectedTime, value.Value);
  }

  [Fact]
  public void ToString_ReturnsSixDigits() {
    var value = new IntHHMMSS(10305);
    Assert.Equal("010305", value.ToString());
  }

  [Fact]
  public void ExtensionFormatting_ReturnsExpectedTime() {
    var value = new IntHHMMSS(143015);
    Assert.Equal("14", value.ToStringHH());
    Assert.Equal("30", value.ToStringMM());
    Assert.Equal("15", value.ToStringSS());
    Assert.Equal("14:30:15", value.ToStringHH_MM_SS());
  }

  [Fact]
  public void GetTimeSpan_ReturnsExpectedDuration() {
    var value = new IntHHMMSS(143015);
    var actual = value.GetTimeSpan();
    Assert.Equal(new TimeSpan(0, 14, 30, 15, 0), actual);
  }

  [Fact]
  public void GetTimeSpan_WithMilliseconds_ReturnsExpectedDuration() {
    var value = new IntHHMMSS(143015);
    var actual = value.GetTimeSpan(250);
    Assert.Equal(new TimeSpan(0, 14, 30, 15, 250), actual);
  }

  [Fact]
  public void TimeProperties_CanBeAccessedRepeatedly() {
    var value = new IntHHMMSS(143015);
    for (var i = 0; i < 10_000; i++) {
      Assert.Equal(14, value.HH);
      Assert.Equal(30, value.MM);
      Assert.Equal(15, value.SS);
      Assert.Equal(14, value.Hour.Value);
      Assert.Equal(30, value.Minute.Value);
      Assert.Equal(15, value.Second.Value);
      Assert.Equal(new TimeOnly(14, 30, 15), value.TimeOnly);
    }
  }

  [Fact]
  public void Constructor_FromCombinedDecimal_ExtractsExactTime() {
    const decimal source = 1251020.143015m;
    var value = new IntHHMMSS(source);
    Assert.Equal(143015, value.Value);
  }

  [Theory]
  [InlineData(143015)]
  [InlineData(1)]
  [InlineData(235959)]
  public void DecimalRoundTrip_PreservesTime(int hhmmss) {
    var source = 1251020m + (hhmmss / 1_000_000m);
    var actual = new IntHHMMSS(source);
    Assert.Equal(hhmmss, actual.Value);
  }

}

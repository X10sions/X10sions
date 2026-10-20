using Xunit;

namespace Common.Structures;

public class IntCYYMMDDTests {

  [Theory]
  [InlineData(2025, 10, 20, 1251020)]
  [InlineData(2024, 01, 01, 1240101)]
  [InlineData(1901, 01, 01, 10101)]
  [InlineData(1999, 12, 31, 991231)]
  [InlineData(2099, 12, 31, 1991231)]
  public void Constructor_FromDateOnly_ProducesCYYMMDD(int year, int month, int day, int expected) {
    var value = new IntCYYMMDD(new DateOnly(year, month, day));
    Assert.Equal(expected, value.Value);
  }

  [Theory]
  [InlineData(1251020, 2025, 10, 20)]
  [InlineData(1240101, 2024, 01, 01)]
  [InlineData(10101, 1901, 01, 01)]
  [InlineData(991231, 1999, 12, 31)]
  [InlineData(1991231, 2099, 12, 31)]
  public void DateOnly_ReturnsExpectedDate(int cyymmdd, int year, int month, int day) {
    var value = new IntCYYMMDD(cyymmdd);
    Assert.Equal(new DateOnly(year, month, day), value.DateOnly);
  }

  [Theory]
  [InlineData(2025, 10, 20)]
  [InlineData(2024, 01, 01)]
  [InlineData(1901, 01, 01)]
  [InlineData(2099, 12, 31)]
  public void DateOnly_RoundTrip(int year, int month, int day) {
    var expected = new DateOnly(year, month, day);
    var cyymmdd = new IntCYYMMDD(expected);
    Assert.Equal(expected, cyymmdd.DateOnly);
  }

  [Theory]
  [InlineData(10101, 1901, 1, 1)]
  [InlineData(1240101, 2024, 1, 1)]
  [InlineData(1251020, 2025, 10, 20)]
  [InlineData(1991231, 2099, 12, 31)]
  public void DateOnly_Returns_Expected_Date(int cyymmdd, int year, int month, int day) {
    var value = new IntCYYMMDD(cyymmdd);
    Assert.Equal(new DateOnly(year, month, day), value.DateOnly);
  }

  [Fact]
  public void DateTime_Does_Not_Cause_Stack_Overflow() {
    var value = new DecimalCYYMMDD_HHMMSS(1251020, 143015);
    var dt = value.DateTime;
    Assert.Equal(2025, dt.Year);
  }

  [Fact]
  public void DateOnly_ProducesExpectedDate() {
    var value = new IntCYYMMDD(1251020);
    Assert.Equal(new DateOnly(2025, 10, 20), value.DateOnly);
  }

  [Fact]
  public void DateTime_ProducesMidnight() {
    var value = new IntCYYMMDD(1251020);
    Assert.Equal(new DateTime(2025, 10, 20, 0, 0, 0), value.DateTime);
  }

  [Fact]
  public void Components_AreDecodedCorrectly() {
    var value = new IntCYYMMDD(1251020);
    Assert.Equal(1, value.C);
    Assert.Equal(125, value.CYY);
    Assert.Equal(25, value.YY);
    Assert.Equal(10, value.MM);
    Assert.Equal(20, value.DD);
    Assert.Equal(2025, value.YYYY);
  }

  [Fact]
  public void CompareToObject_IntCYYMMDD_DoesNotRecurse() {
    var earlier = new IntCYYMMDD(1240101);
    object later = new IntCYYMMDD(1250101);
    var result = earlier.CompareTo(later);
    Assert.True(result < 0);
  }

  [Fact]
  public void CompareTo_IntCYYMMDD_UsesAscendingOrder() {
    var earlier = new IntCYYMMDD(1240101);
    var later = new IntCYYMMDD(1250101);
    Assert.True(earlier.CompareTo(later) < 0);
    Assert.True(later.CompareTo(earlier) > 0);
    Assert.Equal(0, earlier.CompareTo(earlier));
  }

  [Fact]
  public void CompareToObject_Int_UsesAscendingOrder() {
    var value = new IntCYYMMDD(1251020);
    object later = 1251021;
    Assert.True(value.CompareTo(later) < 0);
  }

  [Fact]
  public void CompareTo_Null_ReturnsPositive() {
    var value = new IntCYYMMDD(1251020);
    Assert.True(value.CompareTo(null) > 0);
  }

  [Fact]
  public void CompareToObject_UnsupportedType_ThrowsArgumentException() {
    var value = new IntCYYMMDD(1251020);
    Assert.Throws<ArgumentException>(() => value.CompareTo(new Version(1, 0)));
  }

  [Fact]
  public void Formatting_ReturnsExpectedValues() {
    var value = new IntCYYMMDD(1251020);
    Assert.Equal("2025", value.ToStringYYYY());
    Assert.Equal("10", value.ToStringMM());
    Assert.Equal("20", value.ToStringDD());
    Assert.Equal("2025-10-20", value.ToStringYYYY_MM_DD());
  }

  [Fact]
  public void Equals_NullString_ReturnsFalse() {
    var value = new IntCYYMMDD(1251020);
    Assert.False(value.Equals((string?)null));
  }

}
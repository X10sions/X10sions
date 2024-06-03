using Xunit;

namespace Common.Structures {

  public class IntCYYTests {

    //[Fact]
    //public void ConstructorEmpty_DoesNotThrowAndHasDefaultValue() {
    //  // Arrange & Act
    //  IntCYY actual = null;
    //  var exception = Record.Exception(() => actual = new IntCYY());
    //  // Assert
    //  Assert.Null(exception);
    //  Assert.Equal(0, actual.CYY);
    //}

    //[Theory, MemberData(nameof(TheoryItem.MemberData), MemberType = typeof(TheoryItem))]
    //public void Constructor(TheoryItem data) {
    //  Assert.Equal(data.ExpectedCYY, data.Actual.CYY);
    //  Assert.Equal(data.ExpectedYear, data.Actual.YYYY);
    //}

    //public static (int ExpectedCYY, int ExpectedYear, IntCYY Actual, string TestDescription)[] TheoryData = {
    //    (  0, 1900, new IntCYY()                          , "Default"),
    //    (100, 2000, new IntCYY( 1, 0)                     , "Default: Set C"),
    //    (101, 2001, new IntCYY(  101 )                    , "Default: Set CYY"),
    //    ( 99, 1999, new IntCYY{ YYYY = 1999 }             , "Default: Set Year"),
    //    ( 99, 1999, new IntCYY(0, 99 )                    , "Default: Set YY"),

    //    (  0, 1900, new IntCYY(DateTime.MinValue)         , "DateTime: Min"),
    //    (999, 2899, new IntCYY(DateTime.MaxValue)         , "DateTime: Max"),
    //    (  0, 1900, new IntCYY(new DateTime())            , "DateTime: Default"),
    //    (  0, 1900, new IntCYY(new DateTime(1899,  1,  1)), "DateTime: Min IntCyy - 1 day"),
    //    (  0, 1900, new IntCYY(new DateTime(1900,  1,  1)), "DateTime: Min IntCyy"),
    //    (  1, 1901, new IntCYY(new DateTime(1901,  1,  1)), "DateTime: Min IntCyy + 1 day"),

    //    ( 99, 1999, new IntCYY(new DateTime(1999, 12, 31)), "DateTime: Max day of Century "),
    //    (100, 2000, new IntCYY(new DateTime(2000,  1,  1)), "DateTime: Min day of Century "),
    //    (117, 2017, new IntCYY(new DateTime(2017, 12, 31)), "DateTime: Min day of Year"),
    //    (119, 2019, new IntCYY(new DateTime(2019,  1,  1)), "DateTime: Max day of Year"),

    //    (998, 2898, new IntCYY(new DateTime(2898,  1,  1)), "DateTime: Max IntCyy - 1 day"),
    //    (999, 2899, new IntCYY(new DateTime(2899,  1,  1)), "DateTime: Max IntCyy"),
    //    (999, 2899, new IntCYY(new DateTime(2900,  1,  1)), "DateTime: Max IntCyy + 1 day"),

    //    (  0, 1900, new IntCYY(int.MinValue)              , "Int: Min" ),
    //    (999, 2899, new IntCYY(int.MaxValue)              , "Int: Max" ),
    //    (  0, 1900, new IntCYY(-100)                      , "Int: Max IntCyy - 100" ),
    //    (  0, 1900, new IntCYY(-10)                       , "Int: Max IntCyy - 10" ),
    //    (  0, 1900, new IntCYY(-1)                        , "Int: Max IntCyy - 1" ),
    //    (  0, 1900, new IntCYY(0)                         , "Int: Min IntCyy"),
    //    (  1, 1901, new IntCYY(1)                         , "Int: Min IntCyy + 1" ),
    //    ( 99, 1999, new IntCYY(99)                        , "Int: Max year of Century "),
    //    (100, 2000, new IntCYY(100)                       , "Int: Min year of Century "),
    //    (101, 2001, new IntCYY(101)                       , "Int: Min year of Century + 1"),
    //    (998, 2898, new IntCYY(998)                       , "Int: Max IntCyy - 1" ),
    //    (999, 2899, new IntCYY(999)                       , "Int: Max IntCyy" ),
    //    (999, 2899, new IntCYY(1000)                      , "Int: Max IntCyy + 1" ),

    //    (  0, 1900, new IntCYY(-1,  -1)                   , "C: min - 1, yy: min - 1"),
    //    (  0, 1900, new IntCYY(-1,   0)                   , "C: min - 1, yy: min    "),
    //    (  1, 1901, new IntCYY(-1,   1)                   , "C: min - 1, yy: min + 1"),
    //    (  0, 1900, new IntCYY( 0,  -1)                   , "C: min    , yy: min - 1"),
    //    (  0, 1900, new IntCYY( 0,   0)                   , "C: min    , yy: min    "),
    //    (  1, 1901, new IntCYY( 0,   1)                   , "C: min    , yy: min + 1"),
    //    ( 98, 1998, new IntCYY( 0,  98)                   , "C: min    , yy: max - 1"),
    //    ( 99, 1999, new IntCYY( 0,  99)                   , "C: min    , yy: max    "),
    //    ( 99, 1999, new IntCYY( 0, 100)                   , "C: min    , yy: max + 1"),
    //    (100, 2000, new IntCYY( 1,   0)                   , "C: min + 1, yy:  0 (first year of century)"),
    //    (117, 2017, new IntCYY( 1,  17)                   , "C: min + 1, yy: 17 (test year 2017)"),
    //    (120, 2020, new IntCYY( 1,  20)                   , "C: min + 1, yy: 20 (test year 2020)"),
    //    (121, 2021, new IntCYY( 1,  21)                   , "C: min + 1, yy: 21 (test year 2021)"),
    //    (199, 2099, new IntCYY( 1,  99)                   , "C: min + 1, yy: max"),
    //    (899, 2799, new IntCYY( 8,  99)                   , "C: max - 1, yy: Max"),
    //    (999, 2899, new IntCYY( 9,  99)                   , "C: max    , yy: Max"),
    //    (999, 2899, new IntCYY(10, 100)                   , "C: max + 1, yy: Max"),

    //  };

    //public class TheoryItem {
    //  public TheoryItem(
    //    int expectedCYY, int expectedYear, IntCYY actual, string testDescription) {
    //    Actual = actual;
    //    ExpectedCYY = expectedCYY;
    //    ExpectedYear = expectedYear;
    //    TestDescription = testDescription;
    //  }
    //  public IntCYY Actual { get; set; }
    //  public int ExpectedCYY { get; set; }
    //  public int ExpectedYear { get; set; }
    //  public string TestDescription { get; set; }

    //  public static IEnumerable<TheoryItem> Items = TheoryData.Select(x => new TheoryItem(x.ExpectedCYY, x.ExpectedYear, x.Actual, x.TestDescription));
    //  public static TheoryData<TheoryItem> MemberData => Items.ToTheoryData();
    //}


  }

  //public class IntCYYTests2 {
  //  public IntCYYTests2(ITestOutputHelper output) {
  //    this.output = output;
  //  }
  //  private readonly ITestOutputHelper output;

  //  [Theory]
  //  [InlineAutoData]
  //  [InlineAutoData(0)]
  //  public void Constructor_InlineAutoData_CYY(int cyy) {
  //    var fixture = new Fixture().Customize(new AutoMoqCustomization());
  //    var actual = fixture.Build<IntCYY>().With(x => x.CYY, cyy);
  //    Assert.NotNull(actual);
  //  }

  //  [Fact]
  //  public void Constructor_AutoFixture_Create() {
  //    var fixture = new Fixture().Customize(new AutoMoqCustomization());
  //    var actual = fixture.Create<IntCYY>();
  //    Assert.NotNull(actual);
  //  }

  //  [Theory, AutoData]
  //  public void Constructor_AutoFixture_Build(int cyy) {
  //    var fixture = new Fixture().Customize(new AutoMoqCustomization());
  //    var actual = fixture.Build<IntCYY>().With(x => x.CYY, cyy);
  //    Assert.NotNull(actual);
  //  }

  //  [Theory, AutoData]
  //  public void Constructor_AutoData(int c, int yy, int cyy, DateTime d) {
  //    // Arrange
  //    IntCYY actual = null;
  //    // Act
  //    var exceptionEmpty = Record.Exception(() => actual = new IntCYY());
  //    var exceptionCandYY = Record.Exception(() => actual = new IntCYY(c, yy));
  //    var exceptionCYY = Record.Exception(() => actual = new IntCYY(cyy));
  //    var exceptionDate = Record.Exception(() => actual = new IntCYY(d));
  //    // Assert
  //    Assert.Null(exceptionEmpty);
  //    Assert.Null(exceptionCandYY);
  //    Assert.Null(exceptionCYY);
  //    Assert.Null(exceptionDate);
  //  }

  //  //[Theory, PairwiseData]
  //  //public void Constructor_PairwiseData_YearAndMonthAndDay(
  //  //  [CombinatorialValues(1, 1899, 1900, 1901, 1999, 2000, 2001, 2898, 2899, 2900, 9999)] int year,
  //  //  [CombinatorialValues(1, 2, 12)] int month,
  //  //  [CombinatorialValues(1, 28, 29, 31)] int day
  //  //  ) {
  //  //  var d = new DateTime(year, month, day);
  //  //  IntCYY actual = null;
  //  //  var exception = Record.Exception(() => actual = new IntCYY(d));
  //  //  // Assert
  //  //  Assert.Null(exception);
  //  //}

  //  [Theory, PairwiseData]
  //  public void Constructor_PairwiseData_CandYY(
  //    [CombinatorialValues(-1, 0, 1, 8, 9, 10)] int c,
  //    [CombinatorialValues(-1, 0, 1, 20, 21, 98, 99, 100)] int yy
  //    ) {
  //    IntCYY actual = null;
  //    var exception = Record.Exception(() => actual = new IntCYY(c, yy));
  //    // Assert
  //    Assert.Null(exception);
  //  }

  //  [Theory]
  //  [InlineData(0, 1900, 0, 0)]
  //  [InlineData(999, 2899, 9, 99)]
  //  public void Constructor1(int expectedCYY, int expectedYear, int c, int yy) {
  //    var actual = new IntCYY(c, yy);
  //    Assert.Equal(expectedCYY, actual.CYY);
  //    Assert.Equal(expectedYear, actual.Year);
  //  }

  //  [Theory, MemberData(nameof(Constructor_EqualsMinTheoryData))]
  //  public void Constructor_EqualsMin(IntCYYTests.TheoryItem data) {
  //    Assert.Equal(0, data.Actual.CYY);
  //    Assert.Equal(1900, data.Actual.Year);
  //  }

  //  [Theory, MemberData(nameof(Constructor_EqualsMaxTheoryData))]
  //  public void Constructor_EqualsMax(IntCYYTests.TheoryItem data) {
  //    Assert.Equal(999, data.Actual.CYY);
  //    Assert.Equal(2899, data.Actual.Year);
  //  }

  //  public static TheoryData<IntCYY> Constructor_EqualsMinTheoryData => new IntCYY[]{
  //    new IntCYY(),
  //    new IntCYY(new DateTime()),
  //    new IntCYY(DateTime.MinValue),
  //    new IntCYY(new DateTime(1899, 1, 1)),
  //    new IntCYY(new DateTime(1900, 1, 1)),
  //    new IntCYY(int.MinValue),
  //    new IntCYY(-100),
  //    new IntCYY(-10),
  //    new IntCYY(-1),
  //    new IntCYY(-1,   0) ,
  //    new IntCYY( 0,  -1),
  //    new IntCYY( 0,   0)  ,
  //  }.ToTheoryData();

  //  public static TheoryData<IntCYY> Constructor_EqualsMaxTheoryData => new IntCYY[]{
  //    new IntCYY(DateTime.MaxValue),
  //    new IntCYY(new DateTime(2899, 1, 1)),
  //    new IntCYY(new DateTime(2900, 1, 1)),
  //    new IntCYY(int.MaxValue),
  //    new IntCYY(999),
  //    new IntCYY(1000),
  //    new IntCYY(-1,   0),
  //    new IntCYY( 9,  99),
  //    new IntCYY(10, 100),
  //  }.ToTheoryData();



  //  //public class TheoryItem2 {
  //  //  public static void AssertEqualByCYY(TheoryItem2 expected, TheoryItem2 actual) => Assert.Equal(expected.CYY, actual.CYY);


  //  //  public static void AssertEqualCYY(TheoryItem2 expected, TheoryItem2 actual) => Assert.Equal(expected.CYY, actual.CYY);
  //  //  public static void AssertEQualYear(TheoryItem2 expected, TheoryItem2 actual) => Assert.Equal(expected.Year, actual.Year);

  //  //  public TheoryItem2(int year, int month, int day, int c, int yy, int mm, int dd) {
  //  //    Year = year;
  //  //    Month = month;
  //  //    Day = day;

  //  //    C = c;
  //  //    YY = yy;
  //  //    MM = mm;
  //  //    DD = dd;

  //  //    Date = new DateTime(year, month, day).Date;
  //  //    CYY = C * 100;
  //  //    CYYMM = CYY * 100;
  //  //    CYYMMDD = CYYMM * 100;
  //  //  }

  //  //  public int Year { get; set; }
  //  //  public int Month { get; set; }
  //  //  public int Day { get; set; }

  //  //  public int C { get; set; }
  //  //  public int YY { get; set; }
  //  //  public int MM { get; set; }
  //  //  public int DD { get; set; }

  //  //  public DateTime Date { get; set; }
  //  //  public int CYY { get; set; }
  //  //  public int CYYMM { get; set; }
  //  //  public int CYYMMDD { get; set; }
  //  //}

  //}
}
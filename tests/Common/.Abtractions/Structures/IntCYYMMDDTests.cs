using Common.Structures;
using Xunit;

namespace Common.Abtractions.Structures;
public class IntCYYMMDDTests {

  [Fact]
  public void ConstructorEmpty_DoesNotThrowAndHasDefaultValue() {
    // Arrange & Act
    IntCYYMMDD actual = null;
    var exception = Record.Exception(() => actual = new IntCYYMMDD());
    // Assert
    Assert.Null(exception);
    Assert.Equal(0, actual.CYYMMDD);
  }

  [Theory, MemberData(nameof(TheoryDataItem.Instance), MemberType = typeof(TheoryDataItem))]
  public void Constructor(TheoryDataItem data) {
    Assert.Equal(data.ExpectedCYYMMDD, data.Actual.CYYMMDD);
    Assert.Equal(data.ExpectedDay, data.Actual.Day);
    Assert.Equal(data.ExpectedMonth, data.Actual.Month);
    Assert.Equal(data.ExpectedYear, data.Actual.Year);
  }


  public class TheoryDataItem {
    public TheoryDataItem(int expectedCYYMMDD, int expectedYear, int expectedMonth, int expectedDay, IntCYYMMDD actual, string testDescription) {
      Actual = actual;
      ExpectedCYYMMDD = expectedCYYMMDD;
      ExpectedYear = expectedYear;
      ExpectedMonth = expectedMonth;
      ExpectedDay = expectedDay;
      TestDescription = testDescription;
    }
    public IntCYYMMDD Actual { get; set; }
    public int ExpectedCYYMMDD { get; set; }
    public int ExpectedDay { get; set; }
    public int ExpectedMonth { get; set; }
    public int ExpectedYear { get; set; }
    public string TestDescription { get; set; }

    public static TheoryData<TheoryDataItem> Instance = new TheoryData<TheoryDataItem>{
        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD()                          , "Default"),
        new TheoryDataItem (1000000, 1900,  1,  1, new IntCYYMMDD{ C = 1 }                   , "Default: Set C"),
        new TheoryDataItem (1010000, 2001,  1,  1, new IntCYYMMDD{ CYY = 101 }               , "Default: Set CYY"),
        new TheoryDataItem (1079900, 2007, 12,  1, new IntCYYMMDD{ CYYMM = 10799 }           , "Default: Set CYYMM"),
        new TheoryDataItem (1079999, 2007, 12, 31, new IntCYYMMDD{ CYYMM = 1079999 }         , "Default: Set CYYMMDD"),
        new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ Year = 1999 }             , "Default: Set Year"),
        new TheoryDataItem (    500, 1900,  5,  1, new IntCYYMMDD{ Month = 5 }               , "Default: Set Month"),
        new TheoryDataItem (      5, 1900,  5, 25, new IntCYYMMDD{ Day = 25 }                , "Default: Set Month"),
        new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ YY   = 99 }               , "Default: Set YY"),


        /*
      new IntCYYMMDDTheoryDataItem(  10101               , new DateTime(1901,  1,  1)),
      new IntCYYMMDDTheoryDataItem( 990799               , new DateTime(1999,  7, 31)),
      new IntCYYMMDDTheoryDataItem( 990724               , new DateTime(1999,  7, 24)),
      new IntCYYMMDDTheoryDataItem(1170831               , new DateTime(2017,  8, 31)),
      new IntCYYMMDDTheoryDataItem(1200229               , new DateTime(2020,  2, 29)),
      new IntCYYMMDDTheoryDataItem(1200520               , new DateTime(2020,  5, 20)),
      new IntCYYMMDDTheoryDataItem(1210229               , new DateTime(2021,  2, 28)),

      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
      new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),

      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), null        , DateTime.MinValue.Date),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(8099, 12, 31), null        , DateTime.MaxValue.Date    ),

         */


        new TheoryDataItem (    101,    0,  1,  1, new IntCYYMMDD(new DateTime())            , "DateTime: Default"),
        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(DateTime.MinValue)         , "DateTime: Min"),
        new TheoryDataItem (9991231, 2899, 12, 31, new IntCYYMMDD(DateTime.MaxValue)         , "DateTime: Max"),

        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(new DateTime(1899,  1,  1)), "DateTime: Min IntCYYMM - 1 day"),
        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(new DateTime(1900,  1,  1)), "DateTime: Min IntCYYMM"),
        new TheoryDataItem (1010101, 1901,  1,  1, new IntCYYMMDD(new DateTime(1901,  1,  1)), "DateTime: Min IntCYYMM + 1 day"),

        new TheoryDataItem ( 991231, 1999, 12, 31, new IntCYYMMDD(new DateTime(1999, 12, 31)), "DateTime: Max day of Century "),
        new TheoryDataItem (1000101, 2000,  1,  1, new IntCYYMMDD(new DateTime(2000,  1,  1)), "DateTime: Min day of Century "),
        new TheoryDataItem (1171231, 2017, 12, 31, new IntCYYMMDD(new DateTime(2017, 12, 31)), "DateTime: Min day of Year"),
        new TheoryDataItem (1190101, 2019,  1,  1, new IntCYYMMDD(new DateTime(2019,  1,  1)), "DateTime: Max day of Year"),
        new TheoryDataItem (1200229, 2020,  2, 29, new IntCYYMMDD(new DateTime(2020,  2, 29)), "DateTime: Max day of Month (Leap year)"),
        new TheoryDataItem (1210228, 2021,  2, 28, new IntCYYMMDD(new DateTime(2021,  2, 28)), "DateTime: Max day of Month (Non-Leap year)"),

        new TheoryDataItem (9980101, 2898,  1,  1, new IntCYYMMDD(new DateTime(2898,  1,  1)), "DateTime: Max IntCYYMM - 1 day"),
        new TheoryDataItem (9990101, 2899,  1,  1, new IntCYYMMDD(new DateTime(2899,  1,  1)), "DateTime: Max IntCYYMM"),
        new TheoryDataItem (9990101, 2899,  1,  1, new IntCYYMMDD(new DateTime(2900,  1,  1)), "DateTime: Max IntCYYMM + 1 day"),

        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(int.MinValue)              , "Int: Min" ),
        new TheoryDataItem (9999931, 2899, 12, 31, new IntCYYMMDD(int.MaxValue)              , "Int: Max" ),
        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-100)                      , "Int: Max IntCYYMM - 100" ),
        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-10)                       , "Int: Max IntCYYMM - 10" ),
        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-1)                        , "Int: Max IntCYYMM - 1" ),
        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(0)                         , "Int: Min IntCYYMM"),
        new TheoryDataItem (      1, 1900,  1,  1, new IntCYYMMDD(1)                         , "Int: Min IntCYYMM + 1" ),
        new TheoryDataItem (     99, 1900,  1, 31, new IntCYYMMDD(99)                        , "Int: Max year of Century "),
        new TheoryDataItem (    100, 1900,  1,   1, new IntCYYMMDD(100)                       , "Int: Min year of Century "),
        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(101)                       , "Int: Min year of Century + 1"),
        new TheoryDataItem (    998, 1900,  9, 30, new IntCYYMMDD(998)                       , "Int: Max IntCYYMM - 1" ),
        new TheoryDataItem (    999, 1900,  9, 30, new IntCYYMMDD(999)                       , "Int: Max IntCYYMM" ),
        new TheoryDataItem (    999, 1900, 10,  1, new IntCYYMMDD(1000)                      , "Int: Max IntCYYMM + 1" ),

        new TheoryDataItem (   9912, 1900, 12, 31, new IntCYYMMDD(9912)                        , "Int: Max year of Century "),
        //new TheoryDataItem (  10001, 2000,  1, new IntCYYMMDD(10001)                       , "Int: Min year of Century "),
        //new TheoryDataItem (  10101, 2001,  1, new IntCYYMMDD(10101)                       , "Int: Min year of Century + 1"),
        //new TheoryDataItem (  99812, 2998, 12, new IntCYYMMDD(99812)                       , "Int: Max IntCYYMM - 1" ),
        //new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(99912)                       , "Int: Max IntCYYMM" ),
        //new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(100012)                      , "Int: Max IntCYYMM + 1" ),


        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,  -1, -1)                   , "C: min - 1, yy: min - 1"),
        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,   0, -1)                   , "C: min - 1, yy: min    "),
        //new TheoryDataItem (      1, 1901,  1, new IntCYYMMDD(-1,   1, -1)                   , "C: min - 1, yy: min + 1"),
        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,  -1, -1)                   , "C: min    , yy: min - 1"),
        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,   0, -1)                   , "C: min    , yy: min    "),



        //new TheoryDataItem (  1, 1901, new IntCYYMMDD( 0,   1)                   , "C: min    , yy: min + 1"),
        //new TheoryDataItem ( 98, 1998, new IntCYYMMDD( 0,  98)                   , "C: min    , yy: max - 1"),
        //new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0,  99)                   , "C: min    , yy: max    "),
        //new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0, 100)                   , "C: min    , yy: max + 1"),
        //new TheoryDataItem (100, 2000, new IntCYYMMDD( 1,   0)                   , "C: min + 1, yy:  0 (first year of century)"),
        //new TheoryDataItem (117, 2017, new IntCYYMMDD( 1,  17)                   , "C: min + 1, yy: 17 (test year 2017)"),
        //new TheoryDataItem (120, 2020, new IntCYYMMDD( 1,  20)                   , "C: min + 1, yy: 20 (test year 2020)"),
        //new TheoryDataItem (121, 2021, new IntCYYMMDD( 1,  21)                   , "C: min + 1, yy: 21 (test year 2021)"),
        //new TheoryDataItem (199, 2099, new IntCYYMMDD( 1,  99)                   , "C: min + 1, yy: max"),
        //new TheoryDataItem (899, 2799, new IntCYYMMDD( 8,  99)                   , "C: max - 1, yy: Max"),
        //new TheoryDataItem (999, 2899, new IntCYYMMDD( 9,  99)                   , "C: max    , yy: Max"),
        //new TheoryDataItem (999, 2899, new IntCYYMMDD(10, 100)                   , "C: max + 1, yy: Max"),

        /*

      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
      new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),
 */


      };
  }


  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  public void ConstructorCYYMMDD(IntCYYMMDDTheoryDataItem data) {
    // Arrange & Act
    var actual = new IntCYYMMDD(data.Test.CYYMMDD);
    // Assert
    Assert.Equal(data.Expected.DD, actual.DD);
    Assert.Equal(data.Expected.Date.Day, actual.Day);
    Assert.Equal(data.Expected.Date, actual.Date);
    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  }

  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  public void ConstructorDate(IntCYYMMDDTheoryDataItem data) {
    // Arrange & Act
    var actual = new IntCYYMMDD(data.Test.Date);
    // Assert
    Assert.Equal(data.Expected.DD, actual.DD);
    Assert.Equal(data.Expected.Date.Day, actual.Day);
    Assert.Equal(data.Expected.Date, actual.Date);
    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  }

  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  public void ConstructorYearAndMonthAndDay(IntCYYMMDDTheoryDataItem data) {
    // Arrange & Act
    var actual = new IntCYYMMDD(data.Test.Date.Year, data.Test.Date.Month, data.Test.Date.Day);
    // Assert
    Assert.Equal(data.Expected.DD, actual.DD);
    Assert.Equal(data.Expected.Date.Day, actual.Day);
    Assert.Equal(data.Expected.Date, actual.Date);
    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  }

  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  public void ConstructorCandYYandMMandDD(IntCYYMMDDTheoryDataItem data) {
    // Arrange & Act
    var actual = new IntCYYMMDD(data.Test.C, data.Test.YY, data.Test.MM, data.Test.DD);
    // Assert
    Assert.Equal(data.Expected.DD, actual.DD);
    Assert.Equal(data.Expected.Date.Day, actual.Day);
    Assert.Equal(data.Expected.Date, actual.Date);
    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  }

  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.DD, new IntCYYMMDD { DD = data.Test.DD }.DD);
  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDay(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date.Day, new IntCYYMMDD { Day = data.Test.Date.Day }.Day);
  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDate(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date, new IntCYYMMDD() { Date = data.Test.Date }.Date);
  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetCYYMMDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.CYYMMDD, new IntCYYMMDD { CYYMMDD = data.Test.CYYMMDD }.CYYMMDD);



}

public class IntCYYMMDDTheoryDataItem {
  public IntCYYMMDDTheoryDataItem(int expectedCYYMMDD, DateTime expectedDate, int? testCyymmdd = null, DateTime? testDate = null) {
    Expected = new TheoryDataItem(expectedCYYMMDD, expectedDate);
    Test = new TheoryDataItem(testCyymmdd ?? expectedCYYMMDD, testDate ?? expectedDate);
  }

  public TheoryDataItem Test { get; set; }
  public TheoryDataItem Expected { get; set; }

  public static TheoryData<IntCYYMMDDTheoryDataItem> Instance => new TheoryData<IntCYYMMDDTheoryDataItem> {
      new IntCYYMMDDTheoryDataItem(    101               , new DateTime(1900,  1,  1)),
      new IntCYYMMDDTheoryDataItem(  10101               , new DateTime(1901,  1,  1)),
      new IntCYYMMDDTheoryDataItem( 990799               , new DateTime(1999,  7, 31)),
      new IntCYYMMDDTheoryDataItem( 990724               , new DateTime(1999,  7, 24)),
      new IntCYYMMDDTheoryDataItem(1170831               , new DateTime(2017,  8, 31)),
      new IntCYYMMDDTheoryDataItem(1200229               , new DateTime(2020,  2, 29)),
      new IntCYYMMDDTheoryDataItem(1200520               , new DateTime(2020,  5, 20)),
      new IntCYYMMDDTheoryDataItem(1210229               , new DateTime(2021,  2, 28)),

      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
      new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),

      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), null        , DateTime.MinValue.Date),
      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(8099, 12, 31), null        , DateTime.MaxValue.Date    ),
                                                                    //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(9999, 12, 31),null  , DateTime.MaxValue.Date    )
    };

  public class TheoryDataItem {
    public TheoryDataItem(int cyymmdd, DateTime date) {
      CYYMMDD = cyymmdd;
      Date = date;
    }
    public int CYYMMDD { get; set; }
    public DateTime Date { get; set; }
    public int C => CYY / 100;
    public int CYY => CYYMM / 100;
    public int CYYMM => CYYMMDD / 100;
    public int YY => CYY % 100;
    public int MM => CYYMM % 100;
    public int DD => CYYMMDD % 100;
    public int CYYMM00 => CYYMM * 100;
    public int CYYMM01 => CYYMM00 + 1;
    public int CYYMM99 => CYYMM00 + 99;
  }

}
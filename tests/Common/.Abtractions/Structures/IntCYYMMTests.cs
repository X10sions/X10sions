using Xunit;

namespace Common.Structures {
  public class IntCYYMMTests {

    [Fact]
    public void ConstructorEmpty_DoesNotThrowAndHasDefaultValue() {
      // Arrange & Act
      IntCYYMM actual = null;
      var exception = Record.Exception(() => actual = new IntCYYMM());
      // Assert
      Assert.Null(exception);
      Assert.Equal(0, actual.CYYMM);
    }

    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
    //public void ConstructorDate(IntCYYMMDDTheoryDataItem data) {
    //  // Arrange & Act
    //  var actual = new IntCYYMM(data.Test.Date);
    //  // Assert
    //  Assert.Equal(data.Expected.CYYMM, actual.CYYMM);
    //  Assert.Equal(data.Expected.CYYMM00, actual.CYYMM00);
    //  Assert.Equal(data.Expected.CYYMM01, actual.CYYMM01);
    //  Assert.Equal(data.Expected.CYYMM99, actual.CYYMM99);
    //  Assert.Equal(data.Expected.MM, actual.MM);
    //  Assert.Equal(data.Expected.Date.Month, actual.Month);
    //}

    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
    //public void ConstructorCYYMM(IntCYYMMDDTheoryDataItem data) {
    //  // Arrange & Act
    //  var actual = new IntCYYMM(data.Test.CYYMM);
    //  // Assert
    //  Assert.Equal(data.Expected.CYYMM, actual.CYYMM);
    //  Assert.Equal(data.Expected.CYYMM00, actual.CYYMM00);
    //  Assert.Equal(data.Expected.CYYMM01, actual.CYYMM01);
    //  Assert.Equal(data.Expected.CYYMM99, actual.CYYMM99);
    //  Assert.Equal(data.Expected.MM, actual.MM);
    //  Assert.Equal(data.Expected.Date.Month, actual.Month);
    //}

    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
    //public void ConstructorYearAndMonth(IntCYYMMDDTheoryDataItem data) {
    //  // Arrange & Act
    //  var actual = new IntCYYMM(data.Test.Date.Year, data.Test.Date.Month);
    //  // Assert
    //  Assert.Equal(data.Expected.CYYMM, actual.CYYMM);
    //  Assert.Equal(data.Expected.CYYMM00, actual.CYYMM00);
    //  Assert.Equal(data.Expected.CYYMM01, actual.CYYMM01);
    //  Assert.Equal(data.Expected.CYYMM99, actual.CYYMM99);
    //  Assert.Equal(data.Expected.MM, actual.MM);
    //  Assert.Equal(data.Expected.Date.Month, actual.Month);
    //}

    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
    //public void ConstructorCandYYandMM(IntCYYMMDDTheoryDataItem data) {
    //  // Arrange & Act
    //  var actual = new IntCYYMM(data.Test.C, data.Test.YY, data.Test.MM);
    //  // Assert
    //  Assert.Equal(data.Expected.CYYMM, actual.CYYMM);
    //  Assert.Equal(data.Expected.CYYMM00, actual.CYYMM00);
    //  Assert.Equal(data.Expected.CYYMM01, actual.CYYMM01);
    //  Assert.Equal(data.Expected.CYYMM99, actual.CYYMM99);
    //  Assert.Equal(data.Expected.MM, actual.MM);
    //  Assert.Equal(data.Expected.Date.Month, actual.Month);
    //}

    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetCYYMM(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.CYYMM, new IntCYYMM { CYYMM = data.Test.CYYMM });
    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetMM(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.MM, new IntCYYMM { MM = data.Test.MM });
    //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetMonth(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date.Month, new IntCYYMM { Month = data.Test.Date.Month });

    [Theory, MemberData(nameof(TheoryDataItem.Instance), MemberType = typeof(TheoryDataItem))]
    public void Constructor(TheoryDataItem data) {
      Assert.Equal(data.ExpectedCYYMM, data.Actual.CYYMM);
      Assert.Equal(data.ExpectedYear, data.Actual.YYYY);
      Assert.Equal(data.ExpectedMonth, data.Actual.MM);
    }

    public class TheoryDataItem {
      public TheoryDataItem(int expectedCYYMM, int expectedYear, int expectedMonth, IntCYYMM actual, string testDescription) {
        Actual = actual;
        ExpectedCYYMM = expectedCYYMM;
        ExpectedYear = expectedYear;
        ExpectedMonth = expectedMonth;
        TestDescription = testDescription;
      }
      public IntCYYMM Actual { get; set; }
      public int ExpectedCYYMM { get; set; }
      public int ExpectedMonth { get; set; }
      public int ExpectedYear { get; set; }
      public string TestDescription { get; set; }

      public static TheoryData<TheoryDataItem> Instance = new TheoryData<TheoryDataItem>{
        new TheoryDataItem (    0, 1900,  1, new IntCYYMM()                          , "Default"),
        new TheoryDataItem (10000, 2000,  1, new IntCYYMM{ C = 1 }                   , "Default: Set C"),
        new TheoryDataItem (10100, 2001,  1, new IntCYYMM{ CYY = 101 }               , "Default: Set CYY"),
        new TheoryDataItem (10799, 2007, 12, new IntCYYMM{ CYYMM = 10799 }           , "Default: Set CYYMM"),
        new TheoryDataItem ( 9900, 1999,  1, new IntCYYMM{ YYYY = 1999 }             , "Default: Set Year"),
        new TheoryDataItem (    5, 1999,  5, new IntCYYMM{ MM = 5 }               , "Default: Set Month"),
        new TheoryDataItem ( 9900, 1999,  1, new IntCYYMM{ YY   = 99 }               , "Default: Set YY"),


        new TheoryDataItem (    1,    0,  1, new IntCYYMM(new DateTime())            , "DateTime: Default"),
        new TheoryDataItem (    1, 1900,  1, new IntCYYMM(DateTime.MinValue)         , "DateTime: Min"),
        new TheoryDataItem (99912, 2899, 12, new IntCYYMM(DateTime.MaxValue)         , "DateTime: Max"),

        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(new DateTime(1899,  1,  1)), "DateTime: Min IntCYYMM - 1 day"),
        new TheoryDataItem (  101, 1900,  1, new IntCYYMM(new DateTime(1900,  1,  1)), "DateTime: Min IntCYYMM"),
        new TheoryDataItem (10101, 1901,  1, new IntCYYMM(new DateTime(1901,  1,  1)), "DateTime: Min IntCYYMM + 1 day"),

        new TheoryDataItem ( 9912, 1999, 12, new IntCYYMM(new DateTime(1999, 12, 31)), "DateTime: Max day of Century "),
        new TheoryDataItem (10001, 2000,  1, new IntCYYMM(new DateTime(2000,  1,  1)), "DateTime: Min day of Century "),
        new TheoryDataItem (11712, 2017, 12, new IntCYYMM(new DateTime(2017, 12, 31)), "DateTime: Min day of Year"),
        new TheoryDataItem (11901, 2019,  1, new IntCYYMM(new DateTime(2019,  1,  1)), "DateTime: Max day of Year"),
        new TheoryDataItem (12002, 2020,  2, new IntCYYMM(new DateTime(2020,  2, 29)), "DateTime: Max day of Month (Leap year)"),
        new TheoryDataItem (12102, 2021,  2, new IntCYYMM(new DateTime(2021,  2, 28)), "DateTime: Max day of Month (Non-Leap year)"),

        new TheoryDataItem (99801, 2898,  1, new IntCYYMM(new DateTime(2898,  1,  1)), "DateTime: Max IntCYYMM - 1 day"),
        new TheoryDataItem (99901, 2899,  1, new IntCYYMM(new DateTime(2899,  1,  1)), "DateTime: Max IntCYYMM"),
        new TheoryDataItem (99901, 2899,  1, new IntCYYMM(new DateTime(2900,  1,  1)), "DateTime: Max IntCYYMM + 1 day"),

        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(int.MinValue)              , "Int: Min" ),
        new TheoryDataItem (99999, 2899, 12, new IntCYYMM(int.MaxValue)              , "Int: Max" ),
        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(-100)                      , "Int: Max IntCYYMM - 100" ),
        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(-10)                       , "Int: Max IntCYYMM - 10" ),
        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(-1)                        , "Int: Max IntCYYMM - 1" ),
        new TheoryDataItem (    0, 1900,  1, new IntCYYMM(0)                         , "Int: Min IntCYYMM"),
        new TheoryDataItem (    1, 1901,  1, new IntCYYMM(1)                         , "Int: Min IntCYYMM + 1" ),
        new TheoryDataItem (   99, 1999, 12, new IntCYYMM(99)                        , "Int: Max year of Century "),
        new TheoryDataItem (  100, 1901,  1, new IntCYYMM(100)                       , "Int: Min year of Century "),
        new TheoryDataItem (  101, 1901,  1, new IntCYYMM(101)                       , "Int: Min year of Century + 1"),
        new TheoryDataItem (  998, 1909, 12, new IntCYYMM(998)                       , "Int: Max IntCYYMM - 1" ),
        new TheoryDataItem (  999, 1909, 12, new IntCYYMM(999)                       , "Int: Max IntCYYMM" ),
        new TheoryDataItem (  999, 1910,  1, new IntCYYMM(1000)                      , "Int: Max IntCYYMM + 1" ),

        new TheoryDataItem ( 9912, 1999, 12, new IntCYYMM(9912)                        , "Int: Max year of Century "),
        new TheoryDataItem (10001, 2000,  1, new IntCYYMM(10001)                       , "Int: Min year of Century "),
        new TheoryDataItem (10101, 2001,  1, new IntCYYMM(10101)                       , "Int: Min year of Century + 1"),
        new TheoryDataItem (99812, 2998, 12, new IntCYYMM(99812)                       , "Int: Max IntCYYMM - 1" ),
        new TheoryDataItem (99912, 2899, 12, new IntCYYMM(99912)                       , "Int: Max IntCYYMM" ),
        new TheoryDataItem (99912, 2899, 12, new IntCYYMM(100012)                      , "Int: Max IntCYYMM + 1" ),

        new TheoryDataItem (  0, 1900,  1, new IntCYYMM(0,-1,  -1)                   , "C: min - 1, yy: min - 1"),
        new TheoryDataItem (  0, 1900,  1, new IntCYYMM(0,-1,   0)                   , "C: min - 1, yy: min    "),
        new TheoryDataItem (  1, 1901,  1, new IntCYYMM(0,-1,   1)                   , "C: min - 1, yy: min + 1"),
        new TheoryDataItem (  0, 1900,  1, new IntCYYMM(0, 0,  -1)                   , "C: min    , yy: min - 1"),
        new TheoryDataItem (  0, 1900,  1, new IntCYYMM(0, 0,   0)                   , "C: min    , yy: min    "),
        //new TheoryDataItem (  1, 1901, new IntCYYMM( 0,   1)                   , "C: min    , yy: min + 1"),
        //new TheoryDataItem ( 98, 1998, new IntCYYMM( 0,  98)                   , "C: min    , yy: max - 1"),
        //new TheoryDataItem ( 99, 1999, new IntCYYMM( 0,  99)                   , "C: min    , yy: max    "),
        //new TheoryDataItem ( 99, 1999, new IntCYYMM( 0, 100)                   , "C: min    , yy: max + 1"),
        //new TheoryDataItem (100, 2000, new IntCYYMM( 1,   0)                   , "C: min + 1, yy:  0 (first year of century)"),
        //new TheoryDataItem (117, 2017, new IntCYYMM( 1,  17)                   , "C: min + 1, yy: 17 (test year 2017)"),
        //new TheoryDataItem (120, 2020, new IntCYYMM( 1,  20)                   , "C: min + 1, yy: 20 (test year 2020)"),
        //new TheoryDataItem (121, 2021, new IntCYYMM( 1,  21)                   , "C: min + 1, yy: 21 (test year 2021)"),
        //new TheoryDataItem (199, 2099, new IntCYYMM( 1,  99)                   , "C: min + 1, yy: max"),
        //new TheoryDataItem (899, 2799, new IntCYYMM( 8,  99)                   , "C: max - 1, yy: Max"),
        //new TheoryDataItem (999, 2899, new IntCYYMM( 9,  99)                   , "C: max    , yy: Max"),
        //new TheoryDataItem (999, 2899, new IntCYYMM(10, 100)                   , "C: max + 1, yy: Max"),

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


  }
}
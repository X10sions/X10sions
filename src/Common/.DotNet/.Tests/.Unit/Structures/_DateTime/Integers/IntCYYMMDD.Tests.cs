using Common.Structures;
using FluentAssertions;
using Xunit;

namespace Common.Abtractions.Structures;
public class IntCYYMMDDTests {

  public static TheoryData<DateTime, int> ValueToDateData => new() {
    { new DateTime(1900,  1,  1),   10101 },
    { new DateTime(1901,  1,  1),   10101 },
    { new DateTime(2001,  1,  1), 1010101 },
    { new DateTime(2004,  1,  1), 1040000 },
    { new DateTime(2005,  3,  1), 1050300 },
    { new DateTime(2006,  1, 30), 1060130 },
    { new DateTime(2006,  7, 31), 1060199 },
    { new DateTime(2007,  7, 31), 1070799 },
    { new DateTime(2007, 12,  1), 1071201 },
    { new DateTime(2007, 12, 31), 1079999 },
    { new DateTime(2024,  6,  3), 1240603 },
  }

  public static TheoryData<DateTime, int> DateToValueData => new() {
    { new DateTime(1900,  1,  1),   10101 },
    { new DateTime(1901,  1,  1),   10101 },
    { new DateTime(2001,  1,  1), 1010101 },
    { new DateTime(2004,  1,  1), 1040000 },
    { new DateTime(2005,  3,  1), 1050300 },
    { new DateTime(2006,  1, 30), 1060130 },
    { new DateTime(2006,  7, 31), 1060199 },
    { new DateTime(2007,  7, 31), 1070799 },
    { new DateTime(2007, 12,  1), 1071201 },
    { new DateTime(2007, 12, 31), 1079999 },
    { new DateTime(2024,  6,  3), 1240603 },



      
  
  //        new theorydataitem (1079900, 2007, 12,  1, new intcyymmdd{ cyymm = 10799 }           , "default: set cyymm"),
  //        new theorydataitem (1079999, 2007, 12, 31, new intcyymmdd{ cyymm = 1079999 }         , "default: set cyymmdd"),
  //        new theorydataitem ( 990000, 1999,  1,  1, new intcyymmdd{ yyyy = 1999 }             , "default: set year"),
  //        new theorydataitem (    500, 1900,  5,  1, new intcyymmdd{ mm = 5 }               , "default: set month"),
  //        new theorydataitem (      5, 1900,  5, 25, new intcyymmdd{ dd = 25 }                , "default: set month"),
  //        new theorydataitem ( 990000, 1999,  1,  1, new intcyymmdd{ yy   = 99 }               , "default: set yy"),


  //      new intcyymmddtheorydataitem(  10101               , new datetime(1901,  1,  1)),
  //      new intcyymmddtheorydataitem( 990799               , new datetime(1999,  7, 31)),
  //      new intcyymmddtheorydataitem( 990724               , new datetime(1999,  7, 24)),
  //      new intcyymmddtheorydataitem(1170831               , new datetime(2017,  8, 31)),
  //      new intcyymmddtheorydataitem(1200229               , new datetime(2020,  2, 29)),
  //      new intcyymmddtheorydataitem(1200520               , new datetime(2020,  5, 20)),
  //      new intcyymmddtheorydataitem(1210229               , new datetime(2021,  2, 28)),

  //      new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), int.minvalue),
  //      new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), -1),
  //      new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), 0),
  //      new intcyymmddtheorydataitem(      1               , new datetime(1900,  1,  1), 1),
  //      new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31), int.maxvalue),
  //      new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31)),
  //      new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(2899, 12, 31)),

  //      new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), null        , datetime.minvalue.date),
  //      new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(8099, 12, 31), null        , datetime.maxvalue.date    ),

  //        new theorydataitem (    101,    0,  1,  1, new intcyymmdd(new datetime())            , "datetime: default"),
  //        new theorydataitem (    101, 1900,  1,  1, new intcyymmdd(datetime.minvalue)         , "datetime: min"),
  //        new theorydataitem (9991231, 2899, 12, 31, new intcyymmdd(datetime.maxvalue)         , "datetime: max"),

  //        new theorydataitem (    101, 1900,  1,  1, new intcyymmdd(new datetime(1899,  1,  1)), "datetime: min intcyymm - 1 day"),
  //        new theorydataitem (    101, 1900,  1,  1, new intcyymmdd(new datetime(1900,  1,  1)), "datetime: min intcyymm"),
  //        new theorydataitem (1010101, 1901,  1,  1, new intcyymmdd(new datetime(1901,  1,  1)), "datetime: min intcyymm + 1 day"),

  //        new theorydataitem ( 991231, 1999, 12, 31, new intcyymmdd(new datetime(1999, 12, 31)), "datetime: max day of century "),
  //        new theorydataitem (1000101, 2000,  1,  1, new intcyymmdd(new datetime(2000,  1,  1)), "datetime: min day of century "),
  //        new theorydataitem (1171231, 2017, 12, 31, new intcyymmdd(new datetime(2017, 12, 31)), "datetime: min day of year"),
  //        new theorydataitem (1190101, 2019,  1,  1, new intcyymmdd(new datetime(2019,  1,  1)), "datetime: max day of year"),
  //        new theorydataitem (1200229, 2020,  2, 29, new intcyymmdd(new datetime(2020,  2, 29)), "datetime: max day of month (leap year)"),
  //        new theorydataitem (1210228, 2021,  2, 28, new intcyymmdd(new datetime(2021,  2, 28)), "datetime: max day of month (non-leap year)"),

  //        new theorydataitem (9980101, 2898,  1,  1, new intcyymmdd(new datetime(2898,  1,  1)), "datetime: max intcyymm - 1 day"),
  //        new theorydataitem (9990101, 2899,  1,  1, new intcyymmdd(new datetime(2899,  1,  1)), "datetime: max intcyymm"),
  //        new theorydataitem (9990101, 2899,  1,  1, new intcyymmdd(new datetime(2900,  1,  1)), "datetime: max intcyymm + 1 day"),

  //        new theorydataitem (      0, 1900,  1,  1, new intcyymmdd(int.minvalue)              , "int: min" ),
  //        new theorydataitem (9999931, 2899, 12, 31, new intcyymmdd(int.maxvalue)              , "int: max" ),
  //        new theorydataitem (      0, 1900,  1,  1, new intcyymmdd(-100)                      , "int: max intcyymm - 100" ),
  //        new theorydataitem (      0, 1900,  1,  1, new intcyymmdd(-10)                       , "int: max intcyymm - 10" ),
  //        new theorydataitem (      0, 1900,  1,  1, new intcyymmdd(-1)                        , "int: max intcyymm - 1" ),
  //        new theorydataitem (      0, 1900,  1,  1, new intcyymmdd(0)                         , "int: min intcyymm"),
  //        new theorydataitem (      1, 1900,  1,  1, new intcyymmdd(1)                         , "int: min intcyymm + 1" ),
  //        new theorydataitem (     99, 1900,  1, 31, new intcyymmdd(99)                        , "int: max year of century "),
  //        new theorydataitem (    100, 1900,  1,   1, new intcyymmdd(100)                       , "int: min year of century "),
  //        new theorydataitem (    101, 1900,  1,  1, new intcyymmdd(101)                       , "int: min year of century + 1"),
  //        new theorydataitem (    998, 1900,  9, 30, new intcyymmdd(998)                       , "int: max intcyymm - 1" ),
  //        new theorydataitem (    999, 1900,  9, 30, new intcyymmdd(999)                       , "int: max intcyymm" ),
  //        new theorydataitem (    999, 1900, 10,  1, new intcyymmdd(1000)                      , "int: max intcyymm + 1" ),

  //        new theorydataitem (   9912, 1900, 12, 31, new intcyymmdd(9912)                        , "int: max year of century "),
  //        //new theorydataitem (  10001, 2000,  1, new intcyymmdd(10001)                       , "int: min year of century "),
  //        //new theorydataitem (  10101, 2001,  1, new intcyymmdd(10101)                       , "int: min year of century + 1"),
  //        //new theorydataitem (  99812, 2998, 12, new intcyymmdd(99812)                       , "int: max intcyymm - 1" ),
  //        //new theorydataitem (  99912, 2899, 12, new intcyymmdd(99912)                       , "int: max intcyymm" ),
  //        //new theorydataitem (  99912, 2899, 12, new intcyymmdd(100012)                      , "int: max intcyymm + 1" ),


  //        //new theorydataitem (      0, 1900,  1, new intcyymmdd(-1,  -1, -1)                   , "c: min - 1, yy: min - 1"),
  //        //new theorydataitem (      0, 1900,  1, new intcyymmdd(-1,   0, -1)                   , "c: min - 1, yy: min    "),
  //        //new theorydataitem (      1, 1901,  1, new intcyymmdd(-1,   1, -1)                   , "c: min - 1, yy: min + 1"),
  //        //new theorydataitem (      0, 1900,  1, new intcyymmdd( 0,  -1, -1)                   , "c: min    , yy: min - 1"),
  //        //new theorydataitem (      0, 1900,  1, new intcyymmdd( 0,   0, -1)                   , "c: min    , yy: min    "),



  //        //new theorydataitem (  1, 1901, new intcyymmdd( 0,   1)                   , "c: min    , yy: min + 1"),
  //        //new theorydataitem ( 98, 1998, new intcyymmdd( 0,  98)                   , "c: min    , yy: max - 1"),
  //        //new theorydataitem ( 99, 1999, new intcyymmdd( 0,  99)                   , "c: min    , yy: max    "),
  //        //new theorydataitem ( 99, 1999, new intcyymmdd( 0, 100)                   , "c: min    , yy: max + 1"),
  //        //new theorydataitem (100, 2000, new intcyymmdd( 1,   0)                   , "c: min + 1, yy:  0 (first year of century)"),
  //        //new theorydataitem (117, 2017, new intcyymmdd( 1,  17)                   , "c: min + 1, yy: 17 (test year 2017)"),
  //        //new theorydataitem (120, 2020, new intcyymmdd( 1,  20)                   , "c: min + 1, yy: 20 (test year 2020)"),
  //        //new theorydataitem (121, 2021, new intcyymmdd( 1,  21)                   , "c: min + 1, yy: 21 (test year 2021)"),
  //        //new theorydataitem (199, 2099, new intcyymmdd( 1,  99)                   , "c: min + 1, yy: max"),
  //        //new theorydataitem (899, 2799, new intcyymmdd( 8,  99)                   , "c: max - 1, yy: max"),
  //        //new theorydataitem (999, 2899, new intcyymmdd( 9,  99)                   , "c: max    , yy: max"),
  //        //new theorydataitem (999, 2899, new intcyymmdd(10, 100)                   , "c: max + 1, yy: max"),


  //      //new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), int.minvalue),
  //      //new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), -1),
  //      //new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900,  1,  1), 0),
  //      //new intcyymmddtheorydataitem(      1               , new datetime(1900,  1,  1), 1),
  //      //new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31), int.maxvalue),
  //      //new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31)),
  //      //new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(2899, 12, 31)),


  //      };
  //  }


  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))]
  //  public void constructorcyymmdd(intcyymmddtheorydataitem data) {
  //    // arrange & act
  //    var actual = new intcyymmdd(data.test.cyymmdd);
  //    // assert
  //    assert.equal(data.expected.dd, actual.dd);
  //    assert.equal(data.expected.date.day, actual.dd);
  //    assert.equal(data.expected.date, actual.date);
  //    assert.equal(data.expected.cyymmdd, actual.cyymmdd);
  //  }

  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))]
  //  public void constructordate(intcyymmddtheorydataitem data) {
  //    // arrange & act
  //    var actual = new intcyymmdd(data.test.date);
  //    // assert
  //    assert.equal(data.expected.dd, actual.dd);
  //    assert.equal(data.expected.date.day, actual.dd);
  //    assert.equal(data.expected.date, actual.date);
  //    assert.equal(data.expected.cyymmdd, actual.cyymmdd);
  //  }

  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))]
  //  public void constructoryearandmonthandday(intcyymmddtheorydataitem data) {
  //    // arrange & act
  //    var actual = new intcyymmdd(data.test.date.year, data.test.date.month, data.test.date.day);
  //    // assert
  //    assert.equal(data.expected.dd, actual.dd);
  //    assert.equal(data.expected.date.day, actual.dd);
  //    assert.equal(data.expected.date, actual.date);
  //    assert.equal(data.expected.cyymmdd, actual.cyymmdd);
  //  }

  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))]
  //  public void constructorcandyyandmmanddd(intcyymmddtheorydataitem data) {
  //    // arrange & act
  //    var actual = new intcyymmdd(data.test.c, data.test.yy, data.test.mm, data.test.dd);
  //    // assert
  //    assert.equal(data.expected.dd, actual.dd);
  //    assert.equal(data.expected.date.day, actual.dd);
  //    assert.equal(data.expected.date, actual.date);
  //    assert.equal(data.expected.cyymmdd, actual.cyymmdd);
  //  }

  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))] public void setdd(intcyymmddtheorydataitem data) => assert.equal(data.expected.dd, new intcyymmdd { dd = data.test.dd }.dd);
  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))] public void setday(intcyymmddtheorydataitem data) => assert.equal(data.expected.date.day, new intcyymmdd { dd = data.test.date.day }.dd);
  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))] public void setdate(intcyymmddtheorydataitem data) => assert.equal(data.expected.date, new intcyymmdd() { date = data.test.date }.date);
  //  [theory, memberdata(nameof(intcyymmddtheorydataitem.instance), membertype = typeof(intcyymmddtheorydataitem))] public void setcyymmdd(intcyymmddtheorydataitem data) => assert.equal(data.expected.cyymmdd, new intcyymmdd { cyymmdd = data.test.cyymmdd }.cyymmdd);



  //}


  //public class intcyymmddtheorydataitem {
  //  public intcyymmddtheorydataitem(int expectedcyymmdd, datetime expecteddate, int? testcyymmdd = null, datetime? testdate = null) {
  //    expected = new theorydataitem(expectedcyymmdd, expecteddate);
  //    test = new theorydataitem(testcyymmdd ?? expectedcyymmdd, testdate ?? expecteddate);
  //  }

  //  public theorydataitem test { get; set; }
  //  public theorydataitem expected { get; set; }

  //  public static theorydata<intcyymmddtheorydataitem> instance => new theorydata<intcyymmddtheorydataitem> {
  //      new intcyymmddtheorydataitem(    101               , new datetime(1900,  1,  1)),
  //      new intcyymmddtheorydataitem(  10101               , new datetime(1901,  1,  1)),
  //      new intcyymmddtheorydataitem( 990799               , new datetime(1999,  7, 31)),
  //      new intcyymmddtheorydataitem( 990724               , new datetime(1999,  7, 24)),
  //      new intcyymmddtheorydataitem(1170831               , new datetime(2017,  8, 31)),
  //      new intcyymmddtheorydataitem(1200229               , new datetime(2020,  2, 29)),
  //      new intcyymmddtheorydataitem(1200520               , new datetime(2020,  5, 20)),
  //      new intcyymmddtheorydataitem(1210229               , new datetime(2021,  2, 28)),

  //      new intcyymmddtheorydataitem(intcyymmdd.mincyymmdd, new datetime(1900,  1,  1), int.minvalue),
  //      new intcyymmddtheorydataitem(intcyymmdd.mincyymmdd, new datetime(1900,  1,  1), -1),
  //      new intcyymmddtheorydataitem(intcyymmdd.mincyymmdd, new datetime(1900,  1,  1), 0),
  //      new intcyymmddtheorydataitem(      1               , new datetime(1900,  1,  1), 1),
  //      new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31), int.maxvalue),
  //      new intcyymmddtheorydataitem(9991231               , new datetime(2899, 12, 31)),
  //      new intcyymmddtheorydataitem(intcyymmdd.maxcyymmdd, new datetime(2899, 12, 31)),

  //      new intcyymmddtheorydataitem(intcyymmdd.mincyymmdd, new datetime(1900,  1,  1), null        , datetime.minvalue.date),
  //      new intcyymmddtheorydataitem(intcyymmdd.maxcyymmdd, new datetime(8099, 12, 31), null        , datetime.maxvalue.date    ),
  //                                                                    //new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(9999, 12, 31),null  , datetime.maxvalue.date    )
  //    };

  //  public class theorydataitem {
  //    public theorydataitem(int cyymmdd, datetime date) {
  //      cyymmdd = cyymmdd;
  //      date = date;
  //    }
  //    public int cyymmdd { get; set; }
  //    public datetime date { get; set; }
  //    public int c => cyy / 100;
  //    public int cyy => cyymm / 100;
  //    public int cyymm => cyymmdd / 100;
  //    public int yy => cyy % 100;
  //    public int mm => cyymm % 100;
  //    public int dd => cyymmdd % 100;
  //    public int cyymm00 => cyymm * 100;
  //    public int cyymm01 => cyymm00 + 1;
  //    public int cyymm99 => cyymm00 + 99;
  //  }



















    //{ -1, IntCYYMMDD.MinValue },
    //{  0, IntCYYMMDD.MinValue},
    //{  1,  1},
    //{  9,  9},
    //{ 12, 12},
    //{ 20, 20},
    //{ 2024, 30},
    //{ 9999999, IntCYYMMDD.MaxValue},
    //{ 31, IntCYYMMDD.MaxValue},
    //{ 32, IntCYYMMDD.MaxValue},
    //{ 99, IntCYYMMDD.MaxValue},
    //{ 100, IntCYYMMDD.MaxValue},
    //{ 1000, IntCYYMMDD.MaxValue},
    //{ 10000, IntCYYMMDD.MaxValue},
  };

  [Theory, MemberData(nameof(DateValueData))] public void Value_ShouldBeExpected_GivenDate(DateTime given, int expected) => new IntCYYMMDD(given).Value.Should().Be(expected);
  [Theory, MemberData(nameof(DateValueData))] public void Date_ShouldBeExpected_GivenValue(DateTime expected, int given) => new IntCYYMMDD(given).Date.Should().Be(expected);

  public static TheoryData<int, int> ValueExpectedData => new() {
    { -10, IntCYYMMDD.MinValue },
    { -1, IntCYYMMDD.MinValue },
    {  0, IntCYYMMDD.MinValue},
    {  1,  1},
    {  9,  9},
    { 12, 12},
    { 20, 20},
    { 2024, 30},
    { 9999999, IntCYYMMDD.MaxValue},
    { 31, IntCYYMMDD.MaxValue},
    { 32, IntCYYMMDD.MaxValue},
    { 99, IntCYYMMDD.MaxValue},
    { 100, IntCYYMMDD.MaxValue},
    { 1000, IntCYYMMDD.MaxValue},
    { 10000, IntCYYMMDD.MaxValue},
  };

  [Theory, MemberData(nameof(ValueExpectedData))] public void Value_ShouldBeExpected_GivenValue(int given, int expected) => new IntCYYMMDD(given).Value.Should().Be(expected);


  //[Fact]
  //public void ConstructorEmpty_DoesNotThrowAndHasDefaultValue() {
  //  // Arrange & Act
  //  IntCYYMMDD actual = null;
  //  var exception = Record.Exception(() => actual = new IntCYYMMDD());
  //  // Assert
  //  Assert.Null(exception);
  //  Assert.Equal(0, actual.CYYMMDD);
  //}

  //[Theory, MemberData(nameof(TheoryDataItem.Instance), MemberType = typeof(TheoryDataItem))]
  //public void Constructor(TheoryDataItem data) {
  //  Assert.Equal(data.ExpectedCYYMMDD, data.Actual.CYYMMDD);
  //  Assert.Equal(data.ExpectedDay, data.Actual.DD);
  //  Assert.Equal(data.ExpectedMonth, data.Actual.MM);
  //  Assert.Equal(data.ExpectedYear, data.Actual.YYYY);
  //}

  //  public class TheoryDataItem {
  //    public TheoryDataItem(int expectedCYYMMDD, int expectedYear, int expectedMonth, int expectedDay, IntCYYMMDD actual, string testDescription) {
  //      Actual = actual;
  //      ExpectedCYYMMDD = expectedCYYMMDD;
  //      ExpectedYear = expectedYear;
  //      ExpectedMonth = expectedMonth;
  //      ExpectedDay = expectedDay;
  //      TestDescription = testDescription;
  //    }
  //    public IntCYYMMDD Actual { get; set; }
  //    public int ExpectedCYYMMDD { get; set; }
  //    public int ExpectedDay { get; set; }
  //    public int ExpectedMonth { get; set; }
  //    public int ExpectedYear { get; set; }
  //    public string TestDescription { get; set; }

  //    public static TheoryData<TheoryDataItem> Instance = new TheoryData<TheoryDataItem>{
  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD()                          , "Default"),
  //        new TheoryDataItem (1000000, 1900,  1,  1, new IntCYYMMDD{ C = 1 }                   , "Default: Set C"),
  //        new TheoryDataItem (1010000, 2001,  1,  1, new IntCYYMMDD{ CYY = 101 }               , "Default: Set CYY"),
  //        new TheoryDataItem (1079900, 2007, 12,  1, new IntCYYMMDD{ CYYMM = 10799 }           , "Default: Set CYYMM"),
  //        new TheoryDataItem (1079999, 2007, 12, 31, new IntCYYMMDD{ CYYMM = 1079999 }         , "Default: Set CYYMMDD"),
  //        new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ YYYY = 1999 }             , "Default: Set Year"),
  //        new TheoryDataItem (    500, 1900,  5,  1, new IntCYYMMDD{ MM = 5 }               , "Default: Set Month"),
  //        new TheoryDataItem (      5, 1900,  5, 25, new IntCYYMMDD{ DD = 25 }                , "Default: Set Month"),
  //        new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ YY   = 99 }               , "Default: Set YY"),


  //      new IntCYYMMDDTheoryDataItem(  10101               , new DateTime(1901,  1,  1)),
  //      new IntCYYMMDDTheoryDataItem( 990799               , new DateTime(1999,  7, 31)),
  //      new IntCYYMMDDTheoryDataItem( 990724               , new DateTime(1999,  7, 24)),
  //      new IntCYYMMDDTheoryDataItem(1170831               , new DateTime(2017,  8, 31)),
  //      new IntCYYMMDDTheoryDataItem(1200229               , new DateTime(2020,  2, 29)),
  //      new IntCYYMMDDTheoryDataItem(1200520               , new DateTime(2020,  5, 20)),
  //      new IntCYYMMDDTheoryDataItem(1210229               , new DateTime(2021,  2, 28)),

  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
  //      new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
  //      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
  //      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),

  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), null        , DateTime.MinValue.Date),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(8099, 12, 31), null        , DateTime.MaxValue.Date    ),

  //        new TheoryDataItem (    101,    0,  1,  1, new IntCYYMMDD(new DateTime())            , "DateTime: Default"),
  //        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(DateTime.MinValue)         , "DateTime: Min"),
  //        new TheoryDataItem (9991231, 2899, 12, 31, new IntCYYMMDD(DateTime.MaxValue)         , "DateTime: Max"),

  //        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(new DateTime(1899,  1,  1)), "DateTime: Min IntCYYMM - 1 day"),
  //        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(new DateTime(1900,  1,  1)), "DateTime: Min IntCYYMM"),
  //        new TheoryDataItem (1010101, 1901,  1,  1, new IntCYYMMDD(new DateTime(1901,  1,  1)), "DateTime: Min IntCYYMM + 1 day"),

  //        new TheoryDataItem ( 991231, 1999, 12, 31, new IntCYYMMDD(new DateTime(1999, 12, 31)), "DateTime: Max day of Century "),
  //        new TheoryDataItem (1000101, 2000,  1,  1, new IntCYYMMDD(new DateTime(2000,  1,  1)), "DateTime: Min day of Century "),
  //        new TheoryDataItem (1171231, 2017, 12, 31, new IntCYYMMDD(new DateTime(2017, 12, 31)), "DateTime: Min day of Year"),
  //        new TheoryDataItem (1190101, 2019,  1,  1, new IntCYYMMDD(new DateTime(2019,  1,  1)), "DateTime: Max day of Year"),
  //        new TheoryDataItem (1200229, 2020,  2, 29, new IntCYYMMDD(new DateTime(2020,  2, 29)), "DateTime: Max day of Month (Leap year)"),
  //        new TheoryDataItem (1210228, 2021,  2, 28, new IntCYYMMDD(new DateTime(2021,  2, 28)), "DateTime: Max day of Month (Non-Leap year)"),

  //        new TheoryDataItem (9980101, 2898,  1,  1, new IntCYYMMDD(new DateTime(2898,  1,  1)), "DateTime: Max IntCYYMM - 1 day"),
  //        new TheoryDataItem (9990101, 2899,  1,  1, new IntCYYMMDD(new DateTime(2899,  1,  1)), "DateTime: Max IntCYYMM"),
  //        new TheoryDataItem (9990101, 2899,  1,  1, new IntCYYMMDD(new DateTime(2900,  1,  1)), "DateTime: Max IntCYYMM + 1 day"),

  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(int.MinValue)              , "Int: Min" ),
  //        new TheoryDataItem (9999931, 2899, 12, 31, new IntCYYMMDD(int.MaxValue)              , "Int: Max" ),
  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-100)                      , "Int: Max IntCYYMM - 100" ),
  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-10)                       , "Int: Max IntCYYMM - 10" ),
  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(-1)                        , "Int: Max IntCYYMM - 1" ),
  //        new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD(0)                         , "Int: Min IntCYYMM"),
  //        new TheoryDataItem (      1, 1900,  1,  1, new IntCYYMMDD(1)                         , "Int: Min IntCYYMM + 1" ),
  //        new TheoryDataItem (     99, 1900,  1, 31, new IntCYYMMDD(99)                        , "Int: Max year of Century "),
  //        new TheoryDataItem (    100, 1900,  1,   1, new IntCYYMMDD(100)                       , "Int: Min year of Century "),
  //        new TheoryDataItem (    101, 1900,  1,  1, new IntCYYMMDD(101)                       , "Int: Min year of Century + 1"),
  //        new TheoryDataItem (    998, 1900,  9, 30, new IntCYYMMDD(998)                       , "Int: Max IntCYYMM - 1" ),
  //        new TheoryDataItem (    999, 1900,  9, 30, new IntCYYMMDD(999)                       , "Int: Max IntCYYMM" ),
  //        new TheoryDataItem (    999, 1900, 10,  1, new IntCYYMMDD(1000)                      , "Int: Max IntCYYMM + 1" ),

  //        new TheoryDataItem (   9912, 1900, 12, 31, new IntCYYMMDD(9912)                        , "Int: Max year of Century "),
  //        //new TheoryDataItem (  10001, 2000,  1, new IntCYYMMDD(10001)                       , "Int: Min year of Century "),
  //        //new TheoryDataItem (  10101, 2001,  1, new IntCYYMMDD(10101)                       , "Int: Min year of Century + 1"),
  //        //new TheoryDataItem (  99812, 2998, 12, new IntCYYMMDD(99812)                       , "Int: Max IntCYYMM - 1" ),
  //        //new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(99912)                       , "Int: Max IntCYYMM" ),
  //        //new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(100012)                      , "Int: Max IntCYYMM + 1" ),


  //        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,  -1, -1)                   , "C: min - 1, yy: min - 1"),
  //        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,   0, -1)                   , "C: min - 1, yy: min    "),
  //        //new TheoryDataItem (      1, 1901,  1, new IntCYYMMDD(-1,   1, -1)                   , "C: min - 1, yy: min + 1"),
  //        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,  -1, -1)                   , "C: min    , yy: min - 1"),
  //        //new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,   0, -1)                   , "C: min    , yy: min    "),



  //        //new TheoryDataItem (  1, 1901, new IntCYYMMDD( 0,   1)                   , "C: min    , yy: min + 1"),
  //        //new TheoryDataItem ( 98, 1998, new IntCYYMMDD( 0,  98)                   , "C: min    , yy: max - 1"),
  //        //new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0,  99)                   , "C: min    , yy: max    "),
  //        //new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0, 100)                   , "C: min    , yy: max + 1"),
  //        //new TheoryDataItem (100, 2000, new IntCYYMMDD( 1,   0)                   , "C: min + 1, yy:  0 (first year of century)"),
  //        //new TheoryDataItem (117, 2017, new IntCYYMMDD( 1,  17)                   , "C: min + 1, yy: 17 (test year 2017)"),
  //        //new TheoryDataItem (120, 2020, new IntCYYMMDD( 1,  20)                   , "C: min + 1, yy: 20 (test year 2020)"),
  //        //new TheoryDataItem (121, 2021, new IntCYYMMDD( 1,  21)                   , "C: min + 1, yy: 21 (test year 2021)"),
  //        //new TheoryDataItem (199, 2099, new IntCYYMMDD( 1,  99)                   , "C: min + 1, yy: max"),
  //        //new TheoryDataItem (899, 2799, new IntCYYMMDD( 8,  99)                   , "C: max - 1, yy: Max"),
  //        //new TheoryDataItem (999, 2899, new IntCYYMMDD( 9,  99)                   , "C: max    , yy: Max"),
  //        //new TheoryDataItem (999, 2899, new IntCYYMMDD(10, 100)                   , "C: max + 1, yy: Max"),


  //      //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
  //      //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
  //      //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
  //      //new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
  //      //new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
  //      //new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
  //      //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),


  //      };
  //  }


  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  //  public void ConstructorCYYMMDD(IntCYYMMDDTheoryDataItem data) {
  //    // Arrange & Act
  //    var actual = new IntCYYMMDD(data.Test.CYYMMDD);
  //    // Assert
  //    Assert.Equal(data.Expected.DD, actual.DD);
  //    Assert.Equal(data.Expected.Date.Day, actual.DD);
  //    Assert.Equal(data.Expected.Date, actual.Date);
  //    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  //  }

  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  //  public void ConstructorDate(IntCYYMMDDTheoryDataItem data) {
  //    // Arrange & Act
  //    var actual = new IntCYYMMDD(data.Test.Date);
  //    // Assert
  //    Assert.Equal(data.Expected.DD, actual.DD);
  //    Assert.Equal(data.Expected.Date.Day, actual.DD);
  //    Assert.Equal(data.Expected.Date, actual.Date);
  //    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  //  }

  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  //  public void ConstructorYearAndMonthAndDay(IntCYYMMDDTheoryDataItem data) {
  //    // Arrange & Act
  //    var actual = new IntCYYMMDD(data.Test.Date.Year, data.Test.Date.Month, data.Test.Date.Day);
  //    // Assert
  //    Assert.Equal(data.Expected.DD, actual.DD);
  //    Assert.Equal(data.Expected.Date.Day, actual.DD);
  //    Assert.Equal(data.Expected.Date, actual.Date);
  //    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  //  }

  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))]
  //  public void ConstructorCandYYandMMandDD(IntCYYMMDDTheoryDataItem data) {
  //    // Arrange & Act
  //    var actual = new IntCYYMMDD(data.Test.C, data.Test.YY, data.Test.MM, data.Test.DD);
  //    // Assert
  //    Assert.Equal(data.Expected.DD, actual.DD);
  //    Assert.Equal(data.Expected.Date.Day, actual.DD);
  //    Assert.Equal(data.Expected.Date, actual.Date);
  //    Assert.Equal(data.Expected.CYYMMDD, actual.CYYMMDD);
  //  }

  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.DD, new IntCYYMMDD { DD = data.Test.DD }.DD);
  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDay(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date.Day, new IntCYYMMDD { DD = data.Test.Date.Day }.DD);
  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDate(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date, new IntCYYMMDD() { Date = data.Test.Date }.Date);
  //  [Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetCYYMMDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.CYYMMDD, new IntCYYMMDD { CYYMMDD = data.Test.CYYMMDD }.CYYMMDD);



  //}


  //public class IntCYYMMDDTheoryDataItem {
  //  public IntCYYMMDDTheoryDataItem(int expectedCYYMMDD, DateTime expectedDate, int? testCyymmdd = null, DateTime? testDate = null) {
  //    Expected = new TheoryDataItem(expectedCYYMMDD, expectedDate);
  //    Test = new TheoryDataItem(testCyymmdd ?? expectedCYYMMDD, testDate ?? expectedDate);
  //  }

  //  public TheoryDataItem Test { get; set; }
  //  public TheoryDataItem Expected { get; set; }

  //  public static TheoryData<IntCYYMMDDTheoryDataItem> Instance => new TheoryData<IntCYYMMDDTheoryDataItem> {
  //      new IntCYYMMDDTheoryDataItem(    101               , new DateTime(1900,  1,  1)),
  //      new IntCYYMMDDTheoryDataItem(  10101               , new DateTime(1901,  1,  1)),
  //      new IntCYYMMDDTheoryDataItem( 990799               , new DateTime(1999,  7, 31)),
  //      new IntCYYMMDDTheoryDataItem( 990724               , new DateTime(1999,  7, 24)),
  //      new IntCYYMMDDTheoryDataItem(1170831               , new DateTime(2017,  8, 31)),
  //      new IntCYYMMDDTheoryDataItem(1200229               , new DateTime(2020,  2, 29)),
  //      new IntCYYMMDDTheoryDataItem(1200520               , new DateTime(2020,  5, 20)),
  //      new IntCYYMMDDTheoryDataItem(1210229               , new DateTime(2021,  2, 28)),

  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), -1),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), 0),
  //      new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
  //      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
  //      new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MaxCYYMMDD, new DateTime(2899, 12, 31)),

  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), null        , DateTime.MinValue.Date),
  //      new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MaxCYYMMDD, new DateTime(8099, 12, 31), null        , DateTime.MaxValue.Date    ),
  //                                                                    //new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(9999, 12, 31),null  , DateTime.MaxValue.Date    )
  //    };

  //  public class TheoryDataItem {
  //    public TheoryDataItem(int cyymmdd, DateTime date) {
  //      CYYMMDD = cyymmdd;
  //      Date = date;
  //    }
  //    public int CYYMMDD { get; set; }
  //    public DateTime Date { get; set; }
  //    public int C => CYY / 100;
  //    public int CYY => CYYMM / 100;
  //    public int CYYMM => CYYMMDD / 100;
  //    public int YY => CYY % 100;
  //    public int MM => CYYMM % 100;
  //    public int DD => CYYMMDD % 100;
  //    public int CYYMM00 => CYYMM * 100;
  //    public int CYYMM01 => CYYMM00 + 1;
  //    public int CYYMM99 => CYYMM00 + 99;
  //  }

}

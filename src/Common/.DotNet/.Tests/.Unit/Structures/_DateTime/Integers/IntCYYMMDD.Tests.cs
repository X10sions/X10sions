using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Common.Structures;

public class IntCYYMMDDTests {

  /*


new theorydataitem(1079900, 2007, 12,  1, new intcyymmdd{ cyymm = 10799 }           , "default: set cyymm"),
          new theorydataitem(1079999, 2007, 12, 31, new intcyymmdd { cyymm = 1079999 }, "default: set cyymmdd"),
          new theorydataitem(990000, 1999, 1, 1, new intcyymmdd { yyyy = 1999 }, "default: set year"),
          new theorydataitem(500, 1900, 5, 1, new intcyymmdd { mm = 5 }, "default: set month"),
          new theorydataitem(5, 1900, 5, 25, new intcyymmdd { dd = 25 }, "default: set month"),
          new theorydataitem(990000, 1999, 1, 1, new intcyymmdd { yy = 99 }, "default: set yy"),


        new intcyymmddtheorydataitem(10101, new datetime(1901, 1, 1)),
        new intcyymmddtheorydataitem(990799, new datetime(1999, 7, 31)),
        new intcyymmddtheorydataitem(990724, new datetime(1999, 7, 24)),
        new intcyymmddtheorydataitem(1170831, new datetime(2017, 8, 31)),
        new intcyymmddtheorydataitem(1200229, new datetime(2020, 2, 29)),
        new intcyymmddtheorydataitem(1200520, new datetime(2020, 5, 20)),
        new intcyymmddtheorydataitem(1210229, new datetime(2021, 2, 28)),

        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), int.minvalue),
        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), -1),
        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), 0),
        new intcyymmddtheorydataitem(1, new datetime(1900, 1, 1), 1),
        new intcyymmddtheorydataitem(9991231, new datetime(2899, 12, 31), int.maxvalue),
        new intcyymmddtheorydataitem(9991231, new datetime(2899, 12, 31)),
        new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(2899, 12, 31)),

        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), null, datetime.minvalue.date),
        new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(8099, 12, 31), null, datetime.maxvalue.date),

          new theorydataitem(101, 0, 1, 1, new intcyymmdd(new datetime()), "datetime: default"),
          new theorydataitem(101, 1900, 1, 1, new intcyymmdd(datetime.minvalue), "datetime: min"),
          new theorydataitem(9991231, 2899, 12, 31, new intcyymmdd(datetime.maxvalue), "datetime: max"),

          new theorydataitem(101, 1900, 1, 1, new intcyymmdd(new datetime(1899, 1, 1)), "datetime: min intcyymm - 1 day"),
          new theorydataitem(101, 1900, 1, 1, new intcyymmdd(new datetime(1900, 1, 1)), "datetime: min intcyymm"),
          new theorydataitem(1010101, 1901, 1, 1, new intcyymmdd(new datetime(1901, 1, 1)), "datetime: min intcyymm + 1 day"),

          new theorydataitem(991231, 1999, 12, 31, new intcyymmdd(new datetime(1999, 12, 31)), "datetime: max day of century "),
          new theorydataitem(1000101, 2000, 1, 1, new intcyymmdd(new datetime(2000, 1, 1)), "datetime: min day of century "),
          new theorydataitem(1171231, 2017, 12, 31, new intcyymmdd(new datetime(2017, 12, 31)), "datetime: min day of year"),
          new theorydataitem(1190101, 2019, 1, 1, new intcyymmdd(new datetime(2019, 1, 1)), "datetime: max day of year"),
          new theorydataitem(1200229, 2020, 2, 29, new intcyymmdd(new datetime(2020, 2, 29)), "datetime: max day of month (leap year)"),
          new theorydataitem(1210228, 2021, 2, 28, new intcyymmdd(new datetime(2021, 2, 28)), "datetime: max day of month (non-leap year)"),

          new theorydataitem(9980101, 2898, 1, 1, new intcyymmdd(new datetime(2898, 1, 1)), "datetime: max intcyymm - 1 day"),
          new theorydataitem(9990101, 2899, 1, 1, new intcyymmdd(new datetime(2899, 1, 1)), "datetime: max intcyymm"),
          new theorydataitem(9990101, 2899, 1, 1, new intcyymmdd(new datetime(2900, 1, 1)), "datetime: max intcyymm + 1 day"),

          new theorydataitem(0, 1900, 1, 1, new intcyymmdd(int.minvalue), "int: min"),
          new theorydataitem(9999931, 2899, 12, 31, new intcyymmdd(int.maxvalue), "int: max"),
          new theorydataitem(0, 1900, 1, 1, new intcyymmdd(-100), "int: max intcyymm - 100"),
          new theorydataitem(0, 1900, 1, 1, new intcyymmdd(-10), "int: max intcyymm - 10"),
          new theorydataitem(0, 1900, 1, 1, new intcyymmdd(-1), "int: max intcyymm - 1"),
          new theorydataitem(0, 1900, 1, 1, new intcyymmdd(0), "int: min intcyymm"),
          new theorydataitem(1, 1900, 1, 1, new intcyymmdd(1), "int: min intcyymm + 1"),
          new theorydataitem(99, 1900, 1, 31, new intcyymmdd(99), "int: max year of century "),
          new theorydataitem(100, 1900, 1, 1, new intcyymmdd(100), "int: min year of century "),
          new theorydataitem(101, 1900, 1, 1, new intcyymmdd(101), "int: min year of century + 1"),
          new theorydataitem(998, 1900, 9, 30, new intcyymmdd(998), "int: max intcyymm - 1"),
          new theorydataitem(999, 1900, 9, 30, new intcyymmdd(999), "int: max intcyymm"),
          new theorydataitem(999, 1900, 10, 1, new intcyymmdd(1000), "int: max intcyymm + 1"),

          new theorydataitem(9912, 1900, 12, 31, new intcyymmdd(9912), "int: max year of century "),
          new theorydataitem(10001, 2000, 1, new intcyymmdd(10001), "int: min year of century "),
          new theorydataitem(10101, 2001, 1, new intcyymmdd(10101), "int: min year of century + 1"),
          new theorydataitem(99812, 2998, 12, new intcyymmdd(99812), "int: max intcyymm - 1"),
          new theorydataitem(99912, 2899, 12, new intcyymmdd(99912), "int: max intcyymm"),
          new theorydataitem(99912, 2899, 12, new intcyymmdd(100012), "int: max intcyymm + 1"),


          new theorydataitem(0, 1900, 1, new intcyymmdd(-1, -1, -1), "c: min - 1, yy: min - 1"),
          new theorydataitem(0, 1900, 1, new intcyymmdd(-1, 0, -1), "c: min - 1, yy: min    "),
          new theorydataitem(1, 1901, 1, new intcyymmdd(-1, 1, -1), "c: min - 1, yy: min + 1"),
          new theorydataitem(0, 1900, 1, new intcyymmdd(0, -1, -1), "c: min    , yy: min - 1"),
          new theorydataitem(0, 1900, 1, new intcyymmdd(0, 0, -1), "c: min    , yy: min    "),



          new theorydataitem(1, 1901, new intcyymmdd(0, 1), "c: min    , yy: min + 1"),
          new theorydataitem(98, 1998, new intcyymmdd(0, 98), "c: min    , yy: max - 1"),
          new theorydataitem(99, 1999, new intcyymmdd(0, 99), "c: min    , yy: max    "),
          new theorydataitem(99, 1999, new intcyymmdd(0, 100), "c: min    , yy: max + 1"),
          new theorydataitem(100, 2000, new intcyymmdd(1, 0), "c: min + 1, yy:  0 (first year of century)"),
          new theorydataitem(117, 2017, new intcyymmdd(1, 17), "c: min + 1, yy: 17 (test year 2017)"),
          new theorydataitem(120, 2020, new intcyymmdd(1, 20), "c: min + 1, yy: 20 (test year 2020)"),
          new theorydataitem(121, 2021, new intcyymmdd(1, 21), "c: min + 1, yy: 21 (test year 2021)"),
          new theorydataitem(199, 2099, new intcyymmdd(1, 99), "c: min + 1, yy: max"),
          new theorydataitem(899, 2799, new intcyymmdd(8, 99), "c: max - 1, yy: max"),
          new theorydataitem(999, 2899, new intcyymmdd(9, 99), "c: max    , yy: max"),
          new theorydataitem(999, 2899, new intcyymmdd(10, 100), "c: max + 1, yy: max"),


        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), int.minvalue),
        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), -1),
        new intcyymmddtheorydataitem(intcyymmdd._mincyymmdd, new datetime(1900, 1, 1), 0),
        new intcyymmddtheorydataitem(1, new datetime(1900, 1, 1), 1),
        new intcyymmddtheorydataitem(9991231, new datetime(2899, 12, 31), int.maxvalue),
        new intcyymmddtheorydataitem(9991231, new datetime(2899, 12, 31)),
        new intcyymmddtheorydataitem(intcyymmdd._maxcyymmdd, new datetime(2899, 12, 31)),
   
        new IntCYYMMDDTheoryDataItem(    101               , new DateTime(1900,  1,  1)),
        new IntCYYMMDDTheoryDataItem(  10101               , new DateTime(1901,  1,  1)),
        new IntCYYMMDDTheoryDataItem( 990799               , new DateTime(1999,  7, 31)),
        new IntCYYMMDDTheoryDataItem( 990724               , new DateTime(1999,  7, 24)),
        new IntCYYMMDDTheoryDataItem(1170831               , new DateTime(2017,  8, 31)),
        new IntCYYMMDDTheoryDataItem(1200229               , new DateTime(2020,  2, 29)),
        new IntCYYMMDDTheoryDataItem(1200520               , new DateTime(2020,  5, 20)),
        new IntCYYMMDDTheoryDataItem(1210229               , new DateTime(2021,  2, 28)),

        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), -1),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), 0),
        new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
        new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
        new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MaxCYYMMDD, new DateTime(2899, 12, 31)),

        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MinCYYMMDD, new DateTime(1900,  1,  1), null        , DateTime.MinValue.Date),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD.MaxCYYMMDD, new DateTime(8099, 12, 31), null        , DateTime.MaxValue.Date    ),
                                                                      new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(9999, 12, 31),null  , DateTime.MaxValue.Date    )

          new TheoryDataItem (      0, 1900,  1,  1, new IntCYYMMDD()                          , "Default"),
          new TheoryDataItem (1000000, 1900,  1,  1, new IntCYYMMDD{ C = 1 }                   , "Default: Set C"),
          new TheoryDataItem (1010000, 2001,  1,  1, new IntCYYMMDD{ CYY = 101 }               , "Default: Set CYY"),
          new TheoryDataItem (1079900, 2007, 12,  1, new IntCYYMMDD{ CYYMM = 10799 }           , "Default: Set CYYMM"),
          new TheoryDataItem (1079999, 2007, 12, 31, new IntCYYMMDD{ CYYMM = 1079999 }         , "Default: Set CYYMMDD"),
          new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ YYYY = 1999 }             , "Default: Set Year"),
          new TheoryDataItem (    500, 1900,  5,  1, new IntCYYMMDD{ MM = 5 }               , "Default: Set Month"),
          new TheoryDataItem (      5, 1900,  5, 25, new IntCYYMMDD{ DD = 25 }                , "Default: Set Month"),
          new TheoryDataItem ( 990000, 1999,  1,  1, new IntCYYMMDD{ YY   = 99 }               , "Default: Set YY"),


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
          new TheoryDataItem (  10001, 2000,  1, new IntCYYMMDD(10001)                       , "Int: Min year of Century "),
          new TheoryDataItem (  10101, 2001,  1, new IntCYYMMDD(10101)                       , "Int: Min year of Century + 1"),
          new TheoryDataItem (  99812, 2998, 12, new IntCYYMMDD(99812)                       , "Int: Max IntCYYMM - 1" ),
          new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(99912)                       , "Int: Max IntCYYMM" ),
          new TheoryDataItem (  99912, 2899, 12, new IntCYYMMDD(100012)                      , "Int: Max IntCYYMM + 1" ),


          new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,  -1, -1)                   , "C: min - 1, yy: min - 1"),
          new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD(-1,   0, -1)                   , "C: min - 1, yy: min    "),
          new TheoryDataItem (      1, 1901,  1, new IntCYYMMDD(-1,   1, -1)                   , "C: min - 1, yy: min + 1"),
          new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,  -1, -1)                   , "C: min    , yy: min - 1"),
          new TheoryDataItem (      0, 1900,  1, new IntCYYMMDD( 0,   0, -1)                   , "C: min    , yy: min    "),



          new TheoryDataItem (  1, 1901, new IntCYYMMDD( 0,   1)                   , "C: min    , yy: min + 1"),
          new TheoryDataItem ( 98, 1998, new IntCYYMMDD( 0,  98)                   , "C: min    , yy: max - 1"),
          new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0,  99)                   , "C: min    , yy: max    "),
          new TheoryDataItem ( 99, 1999, new IntCYYMMDD( 0, 100)                   , "C: min    , yy: max + 1"),
          new TheoryDataItem (100, 2000, new IntCYYMMDD( 1,   0)                   , "C: min + 1, yy:  0 (first year of century)"),
          new TheoryDataItem (117, 2017, new IntCYYMMDD( 1,  17)                   , "C: min + 1, yy: 17 (test year 2017)"),
          new TheoryDataItem (120, 2020, new IntCYYMMDD( 1,  20)                   , "C: min + 1, yy: 20 (test year 2020)"),
          new TheoryDataItem (121, 2021, new IntCYYMMDD( 1,  21)                   , "C: min + 1, yy: 21 (test year 2021)"),
          new TheoryDataItem (199, 2099, new IntCYYMMDD( 1,  99)                   , "C: min + 1, yy: max"),
          new TheoryDataItem (899, 2799, new IntCYYMMDD( 8,  99)                   , "C: max - 1, yy: Max"),
          new TheoryDataItem (999, 2899, new IntCYYMMDD( 9,  99)                   , "C: max    , yy: Max"),
          new TheoryDataItem (999, 2899, new IntCYYMMDD(10, 100)                   , "C: max + 1, yy: Max"),


        new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), int.MinValue),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), -1),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MinCYYMMDD, new DateTime(1900,  1,  1), 0),
        new IntCYYMMDDTheoryDataItem(      1               , new DateTime(1900,  1,  1), 1),
        new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31), int.MaxValue),
        new IntCYYMMDDTheoryDataItem(9991231               , new DateTime(2899, 12, 31)),
        new IntCYYMMDDTheoryDataItem(IntCYYMMDD._MaxCYYMMDD, new DateTime(2899, 12, 31)),
   
   
   */

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
      new (      0, new(1901,  1,  1), 0,  0,  0,  0),
      new (      1, new(1901,  1,  1), 0,  0,  0,  1),
      new (     -1, new(1901,  1,  1), 0,  0,  0,  0),
      new (    101, new(1901,  1,  1), 0,  0,  1,  1),
      new (  10101, new(1901,  1,  1), 0,  1,  1,  1),
      new ( 990724, new(1999,  7, 24), 0, 99,  7, 24),
      new ( 990799, new(1999,  7, 31), 0, 99,  7, 99),
      new (1010101, new(2001,  1,  1), 1,  1,  1,  1),
      new (1170831, new(2017,  8, 31), 1, 17,  8, 31),
      new (1200229, new(2020,  2, 29), 1, 20,  2, 29),
      new (1210229, new(2021,  2, 28), 1, 21,  2, 29),
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
      new (9991231, new(2899, 12, 31), 9, 99, 12, 31),
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




  //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.DD, new IntCYYMMDD { DD = data.Test.DD }.DD);
  //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDay(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date.Day, new IntCYYMMDD { DD = data.Test.Date.Day }.DD);
  //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetDate(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.Date, new IntCYYMMDD() { Date = data.Test.Date }.Date);
  //[Theory, MemberData(nameof(IntCYYMMDDTheoryDataItem.Instance), MemberType = typeof(IntCYYMMDDTheoryDataItem))] public void SetCYYMMDD(IntCYYMMDDTheoryDataItem data) => Assert.Equal(data.Expected.CYYMMDD, new IntCYYMMDD { CYYMMDD = data.Test.CYYMMDD }.CYYMMDD);
}
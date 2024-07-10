using Common.ValueObjects;

namespace Common.Structures;

//public readonly record struct DayOfMonth : IValueObject<int> {
//  DayOfMonth(int day, int daysInMonth) {
//    Value = day.Clamp(MinValue, daysInMonth);
//  }
//  public DayOfMonth(int day, Month Month, Year Year) : this(day, Month.DaysInMonth(Year)) { }
//  public DayOfMonth(DateOnly d) : this(d.Day, d.DaysInMonth()) { }
//  public DayOfMonth(DateTime d) : this(d.Day, d.DaysInMonth()) { }

//  //public DayOfMonth(IntCYYMMDD cyymmdd) : this(cyymmdd.DateOnly) { }
//  //  public DayOfMonth(string value) : this(int.Parse(value)) { }

//  public const int MinValue = 1;
//  //  public const int MaxValue = 31;

//  public int Value { get; }
//  public override string ToString() => Value.ToString("00");

//  public static readonly DayOfMonth Min = new(MinValue, Month.Min, Year.Min);
//  //  public static readonly Day Max = new(MaxValue);
//}

//public static class DayOfMonthExtensions {

//}
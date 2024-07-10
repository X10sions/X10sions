using Common.ValueObjects;

namespace Common.Structures;

//public readonly record struct MonthOfYear(Month Month, Year Year) : IValueObject<int> {
//  //public MonthOfYear(int Value) :this(d.GetMonth(), d.GetYear()) {  }
//  public MonthOfYear() : this(DateTime.Now) { }
//  public MonthOfYear(DateOnly d) : this(d.GetMonth(), d.GetYear()) { }
//  public MonthOfYear(DateTime d) : this(d.GetMonth(), d.GetYear()) { }
//  //public MonthOfYear(IntCYYMMDD cyymmdd) : this(cyymmdd.DateOnly) { }
//  //  public DayOfMonth(string value) : this(int.Parse(value)) { }

//  //public const int MinValue = 1;
//  //public const int MaxValue = 31;

//  public int Value => Year.Value * 12 + Month.Value;

//  public override string ToString() => $"{Year}-{Month}";

//  public static readonly MonthOfYear Min = new(Month.Max, Year.Max);
//  public static readonly MonthOfYear Max = new(Month.Max, Year.Max);
//}

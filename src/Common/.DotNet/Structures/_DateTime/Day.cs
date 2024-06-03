using Common.ValueObjects;

namespace Common.Structures;

/*
public readonly record struct Day(int Value  ) : IValueObject<int> {
  public Day(DateOnly d) : this(d.Day) { }
  public Day(DateTime d) : this(d.Day) { }
  public Day(int value, Month month) : this(d.Day) { }
  //public Day(TimeSpan ts) : this(ts.Days) { }
  public Day(IntCYYMMDD cyymmdd) : this(cyymmdd.DateOnly.Day) { }
  public Day(string value) : this(int.Parse(value)) { }

  public const int MinValue = 1;
  public const int MaxValue = 31;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Day Min = new(MinValue);
  public static readonly Day Max = new(MaxValue);
}
*/
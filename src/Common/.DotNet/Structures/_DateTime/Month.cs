using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct Month(int Value) : IValueObject<int> {
  public Month(DateOnly d) : this(d.Month) { }
  public Month(DateTime d) : this(d.Month) { }
  public Month(IntCYYMM cyymm) : this(cyymm.MM) { }
  public Month(IntCYYMMDD cyymmdd) : this(cyymmdd.MM) { }

  public Month(string value) : this(int.Parse(value)) { }

  public const int MinValidValue = 1;
  public const int MaxValidValue = 12;
  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int DaysInMonth(Year year) => DateTime.DaysInMonth(year.Value, Value);

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Month Min = new(MinValue);
  public static readonly Month MinValid = new(MinValidValue);
  public static readonly Month Max = new(MaxValue);
  public static readonly Month MaxValid = new(MaxValidValue);
}

public static class MonthExtensions {
  public static Month GetMonth(this DateTime dateTime) => new(dateTime);
  public static Month GetMonth(this DateOnly dateOnly) => new(dateOnly);
}
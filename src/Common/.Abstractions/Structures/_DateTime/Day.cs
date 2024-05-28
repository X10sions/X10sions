using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Day(int Value) : IValueObject<int> {
  public Day(IntCYYMMDD cyymmdd) : this(cyymmdd.Value % 100) { }
  public Day(string value) : this(int.Parse(value)) { }

  public const int MinValidValue = 1;
  public const int MaxValidValue = 31;
  public const int MinValue = 0; 
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Day Min = new(MinValue);
  public static readonly Day MinValid = new(MinValidValue);
  public static readonly Day Max = new (MaxValue);
  public static readonly Day MaxValid = new (MaxValidValue);
}
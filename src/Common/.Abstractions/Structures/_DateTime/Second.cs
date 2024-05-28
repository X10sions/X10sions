using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Second(int Value) : IValueObject<int> {
  public Second(IntHHMMSS hhmmss) :this(hhmmss.Value % 100) { }

  public const int MinValue = 0;
  public const int MaxValue = 59;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Second Min = new(MinValue);
  public static readonly Second Max = new (MaxValue);
}

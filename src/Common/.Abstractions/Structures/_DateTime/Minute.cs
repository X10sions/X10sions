using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Minute(int Value) : IValueObject<int> {
  public Minute(IntHHMMSS hhmmss) : this(new IntHHMM(hhmmss).Value % 100) { }
  public Minute(string value) : this(int.Parse(value)) { }
  public const int MinValue = 0;
  public const int MaxValue = 59;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Minute Min = new(MinValue);
  public static readonly Minute Max = new(MaxValue);
}
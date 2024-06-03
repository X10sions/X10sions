using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Millisecond(int Value) : IValueObject<int> {
  public Millisecond(DateTime d) : this(d.Millisecond) { }
  public Millisecond(TimeOnly t) : this(t.Millisecond) { }
  public const int MinValue = 0;
  public const int MaxValue = 999;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("000");

  public static readonly Millisecond Min = new(MinValue);
  public static readonly Millisecond Max = new (MaxValue);
}
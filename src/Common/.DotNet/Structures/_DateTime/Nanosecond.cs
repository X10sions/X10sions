using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Nanosecond(int Value) : IValueObject<int> {
  public Nanosecond(DateTime d) : this(d.Nanosecond) { }
  public Nanosecond(TimeOnly t) : this(t.Nanosecond) { }
  public const int MinValue = 0;
  public const int MaxValue = 900;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("000");

  public static readonly Nanosecond Min = new (MinValue);
  public static readonly Nanosecond Max = new (MaxValue);
}

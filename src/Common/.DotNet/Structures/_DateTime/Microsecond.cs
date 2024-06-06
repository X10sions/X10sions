using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Microsecond(int Value) : IValueObject<int> {
  public Microsecond(DateTime d) : this(d.Millisecond) { }
  public Microsecond(TimeOnly t) : this(t.Millisecond) { }
  public const int MinValue = 0;
  public const int MaxValue = 999;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("000");

  public static readonly Millisecond Min = new(MinValue);
  public static readonly Millisecond Max = new(MaxValue);
}
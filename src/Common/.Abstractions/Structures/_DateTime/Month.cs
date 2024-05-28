using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct Month(int Value) : IValueObject<int> {
  public Month(IntHHMM hhmm) : this(hhmm.Value % 100) { }
  public Month(IntCYYMM cyymm) : this(cyymm.Value % 100) {}
  public Month(IntCYYMMDD cyymmdd) : this(cyymmdd.CYYMM) { }

  public Month(string value) : this(int.Parse(value)) { }

  public const int MinValidValue = 1;
  public const int MaxValidValue = 12;
  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Month Min = new(MinValue);
  public static readonly Month MinValid = new(MinValidValue);
  public static readonly Month Max = new(MaxValue);
  public static readonly Month MaxValid = new(MaxValidValue);
}
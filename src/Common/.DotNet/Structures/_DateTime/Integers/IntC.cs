using Common.ValueObjects;

namespace Common.Structures;

/// <summary> Centuries after 1900 </summary>
public readonly record struct IntC(int Value) : IValueObject<int> {
  public IntC(IntCYY cyy) : this(cyy.Value / 100) { }
  public IntC(IntCYYMM cyymm) : this(cyymm.Value / 10000) { }
  public IntC (IntCYYMMDD cyymmdd) : this(cyymmdd.Value / 1000000) { }

  public IntC(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 9;
  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("0");

  public static readonly IntC Min = new(MinValue);
  public static readonly IntC Max = new(MaxValue);
}

public static class IntCExtensions {
  //public static IntC ToIntC(this IntCYY cyy) => new(cyy.Value / 100);
  //public static IntC ToIntC(this IntCYYMM cyymm) => new(cyymm.Value / 10000);
  //public static IntC ToIntC(this IntCYYMMDD cyymmdd) => new(cyymmdd.Value / 1000000);
  //public static IntC ToIntC(this IntCYYMMDD_HHMMSS cyymmdd_hhmmss) => new(cyymmdd_hhmmss.IntCYYMMDD.Value / 1000000);
}

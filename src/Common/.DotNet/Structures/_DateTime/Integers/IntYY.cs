using Common.ValueObjects;

namespace Common.Structures;

/*
public readonly record struct IntYY(int Value) : IValueObject<int> {
  public IntYY(IntCYY cyy) : this(cyy.YY) { }
  public IntYY(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly IntYY Min = new(MinValue);
  public static readonly IntYY Max = new(MaxValue);
}

public readonly record struct IntMM(int Value) : IValueObject<int> {
  public IntMM(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly IntMM Min = new(MinValue);
  public static readonly IntMM Max = new(MaxValue);
}

public readonly record struct IntDD(int Value) : IValueObject<int> {
  //  public IntDD(IntCYYMMDD cyymmdd) : this(cyymmdd.Value % 100) { }
  public IntDD(string value) : this(int.Parse(value)) { }

  //  public const int MinValidValue = 1;
  //  public const int MaxValidValue = 31;
  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly IntDD Min = new(MinValue);
  //  public static readonly IntDD MinValid = new(MinValidValue);
  public static readonly IntDD Max = new(MaxValue);
  //  public static readonly IntDD MaxValid = new(MaxValidValue);
}

*/
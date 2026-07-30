namespace Common.ValueObjects;
  public readonly record struct IdCodeName<T>(T Id, string Code, string Name);

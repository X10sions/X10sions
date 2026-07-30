namespace Common.ValueObjects;
  public readonly record struct IdDescription<T>(T Id, string Description);

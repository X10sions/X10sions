namespace Common.ValueObjects;

public readonly record struct Range<T>(T Min, T Max);

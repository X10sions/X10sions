using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct ConstraintName(string Value) : IValueObject<string> { }

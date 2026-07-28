using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct ViewName(string Value) : IValueObject<string> { }

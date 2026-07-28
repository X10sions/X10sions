using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct DatabaseName(string Value) : IValueObject<string> { }

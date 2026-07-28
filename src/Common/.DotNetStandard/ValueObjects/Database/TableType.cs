using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct TableType(string Value) : IValueObject<string> { }

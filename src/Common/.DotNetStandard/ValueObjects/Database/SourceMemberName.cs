namespace Common.ValueObjects.Database;

public readonly record struct SourceMemberName(string Value, SourceFileName SourceFile) : ISourceMemberName { }

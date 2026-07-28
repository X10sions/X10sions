namespace Common.ValueObjects.Database;

public readonly record struct SourceFileName(string Value, SchemaName Schema) : ISchemaObjectName { }

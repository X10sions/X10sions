namespace Common.ValueObjects.Database;

public readonly record struct SchemaObjectName(string Value, SchemaName Schema) : ISchemaObjectName { }

namespace Common.ValueObjects.Database;

public readonly record struct TableName(string Value, SchemaName Schema) : ISchemaObjectName {
  public TableName(string value) : this(value, SchemaName.Empty) { }
  public static TableName Empty { get; } = new(string.Empty);
}

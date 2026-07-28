namespace Common.ValueObjects.Database;

public readonly record struct ColumnName(string Value, TableName Table) : ITableObjectName {
  public ColumnName(string value) : this(value, TableName.Empty) { }
}

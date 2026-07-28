using Common.Models;

namespace Common.ValueObjects.Database;

public interface ITableObjectName : IValueObject<string> {
  public TableName Table { get; }
}

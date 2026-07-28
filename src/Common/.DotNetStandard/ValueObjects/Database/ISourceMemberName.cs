using Common.Models;

namespace Common.ValueObjects.Database;

public interface ISourceMemberName : IValueObject<string> {
  public SourceFileName SourceFile { get; }
}

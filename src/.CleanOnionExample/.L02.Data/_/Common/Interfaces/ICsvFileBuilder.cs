using CleanOnionExample.Data.Entities.Services;

namespace Common.Interfaces;

public interface ICsvFileBuilder {
  byte[] BuildTodoItemsFile(IEnumerable<ExportTodos.ToDoItemRecord> records);
}

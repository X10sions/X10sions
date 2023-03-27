using CleanOnionExample.Application.TodoLists;

namespace Common.Interfaces;

public interface ICsvFileBuilder {
  byte[] BuildTodoItemsFile(IEnumerable<ExportTodos.ToDoItemRecord> records);
}

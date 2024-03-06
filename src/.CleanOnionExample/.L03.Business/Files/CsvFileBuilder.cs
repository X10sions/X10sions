using CleanOnionExample.Files.Maps;
using Common.Interfaces;
using System.Globalization;
using static CleanOnionExample.Application.TodoLists.ExportTodos;

namespace CleanOnionExample.Files;

public class CsvFileBuilder : ICsvFileBuilder {
  public byte[] BuildTodoItemsFile(IEnumerable<ToDoItemRecord> records) {
    using var memoryStream = new MemoryStream();
    using (var streamWriter = new StreamWriter(memoryStream)) {
      using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
      csvWriter.Context.RegisterClassMap<TodoItemRecordMap>();
      csvWriter.WriteRecords(records);
    }
    return memoryStream.ToArray();
  }
}

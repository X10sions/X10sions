using System.Globalization;
using static CleanOnionExample.Application.TodoLists.ExportTodos;

namespace CleanOnionExample.Files.Maps;

public class TodoItemRecordMap : ClassMap<ToDoItemRecord> {
  public TodoItemRecordMap() {
    AutoMap(CultureInfo.InvariantCulture);
    Map(m => m.Done).Convert(c => c.Value.Done ? "Yes" : "No");
  }
}

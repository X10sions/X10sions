using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Infrastructure.Factories;
public class ToDoItemFactory : ToDoItem {
  public ToDoItemFactory() { }

  public ToDoItemFactory(ToDoItemSummary summary, ToDoItemDescription description) {
    Id = new ToDoItemId(Guid.NewGuid());
    Summary = summary;
    Description = description;
  }
}

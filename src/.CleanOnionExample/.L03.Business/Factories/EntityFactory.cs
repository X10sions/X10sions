using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Infrastructure.Factories;

public class EntityFactory : IToDoItemFactory {
  public ToDoItem CreateToDoItemInstance(ToDoItemSummary summary, ToDoItemDescription description) {
    return new ToDoItemFactory(summary, description);
  }
}

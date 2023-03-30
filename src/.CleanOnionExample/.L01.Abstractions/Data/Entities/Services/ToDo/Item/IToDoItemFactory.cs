namespace CleanOnionExample.Data.Entities.Services;

public interface IToDoItemFactory {
  ToDoItem CreateToDoItemInstance(ToDoItemSummary summary, ToDoItemDescription description);
}
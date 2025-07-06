namespace X10sions.Fake.Features.ToDo.Item;

public interface IToDoItemFactory {
  ToDoItem CreateToDoItemInstance(ToDoItemSummary summary, ToDoItemDescription description);
}
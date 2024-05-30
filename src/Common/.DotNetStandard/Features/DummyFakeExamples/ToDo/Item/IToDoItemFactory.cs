namespace Common.Features.DummyFakeExamples.ToDo.Item;

public interface IToDoItemFactory {
  ToDoItem CreateToDoItemInstance(ToDoItemSummary summary, ToDoItemDescription description);
}
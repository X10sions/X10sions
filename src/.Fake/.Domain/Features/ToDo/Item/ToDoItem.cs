using Common.Domain.Entities;
using Common.Enums;
using RCommon.Entities;

namespace X10sions.Fake.Features.ToDo.Item;
public class ToDoItem : BusinessEntity<ToDoItemId> {// EntityBase<ToDoItemId> {
  //public class ToDoItem : IEntityWithId<ToDoItemId> {
  public ToDoItemId Id { get; set; }
  public ToDoItemSummary Summary { get; set; }
  public ToDoItemDescription Description { get; set; }
  public string Title { get; set; } = string.Empty;
  public int? ListId { get; set; }
  public PriorityLevel Priority { get; set; }
  public string Note { get; set; } = string.Empty;
  public bool IsDone { get; set; }
  public void MarkComplete() {
    if (!IsDone) {
      IsDone = true;
      AddLocalEvent(new ToDoItemCompletedEvent(this));
      //Events.Add(new ToDoItemCompletedEvent(this));
    }
  }
  public override string ToString() => $"{Id}: Status: {(IsDone ? "Done!" : "Not done.")} - {Title} - {Description}";
}

public record ToDoItemDescription(string Value) {
  public string Value { get; } = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentException($"Description value is required") : Value;
}

public record ToDoItemSummary(string Value) {
  public string Value { get; } = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentException("Summary value is required") : Value;
}

public record ToDoItemId(Guid Value) {
  public Guid Value { get; } = Value.Equals(Guid.Empty) ? throw new ArgumentException("ToDoItem Id does not have any value") : Value;
}
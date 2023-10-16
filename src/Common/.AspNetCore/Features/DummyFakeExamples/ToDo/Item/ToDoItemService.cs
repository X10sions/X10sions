using CleanOnionExample.Data.Entities;
using Common.Data;
using System.ComponentModel.DataAnnotations;

namespace Common.Features.DummyFakeExamples.ToDo.Item;
public record ToDoItemCompletedEvent(ToDoItem CompletedItem) : DomainEventBase;

public record ToDoItemDTO(Guid Id, [Required] string? Title, string? Description, bool IsDone) {
  public ToDoItemDTO(ToDoItem item) : this(item.Id.Value, item.Title, item.Description.Value, item.IsDone) { }
}

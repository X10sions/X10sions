using Common.Domain.Events;

namespace X10sions.Fake.Features.ToDo.Item;

public record ToDoItemCreatedEvent(Guid Id, string Description, string Summary) : DomainEventBase {
  public ToDoItemCreatedEvent(ToDoItem entity) : this(entity.Id.Value, entity.Description.Value, entity.Summary.Value) { }
}


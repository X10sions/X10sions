using Common.Domain.Events;

namespace X10sions.Fake.Features.ToDo.Item;

public record ToDoItemDeletedEvent(Guid Id) : DomainEventBase {
  public ToDoItemDeletedEvent(ToDoItem entity) : this(entity.Id.Value) { }
}


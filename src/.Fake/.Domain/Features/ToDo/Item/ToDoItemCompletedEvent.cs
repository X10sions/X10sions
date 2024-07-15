using Common.Domain.Events;

namespace X10sions.Fake.Features.ToDo.Item;
public record ToDoItemCompletedEvent(ToDoItem CompletedItem) : DomainEventBase {}

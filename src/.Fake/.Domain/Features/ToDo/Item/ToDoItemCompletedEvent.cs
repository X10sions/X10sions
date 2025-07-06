using Common.Domain.Events;
using RCommon.EventHandling;

namespace X10sions.Fake.Features.ToDo.Item;
public record ToDoItemCompletedEvent(ToDoItem CompletedItem) : DomainEventBase, ISerializableEvent {}

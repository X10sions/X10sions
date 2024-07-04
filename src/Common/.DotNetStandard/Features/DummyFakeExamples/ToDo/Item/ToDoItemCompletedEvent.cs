using Common.Domain; 

namespace Common.Features.DummyFakeExamples.ToDo.Item;
public record ToDoItemCompletedEvent(ToDoItem CompletedItem) : DomainEventBase {}

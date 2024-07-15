using Common.Domain.Events;

namespace X10sions.Fake.Features.ToDo.Item;

public record ToDoItemCreatedEvent(Guid Id, string Description, string Summary) : DomainEventBase { }


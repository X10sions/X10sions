using Common.Domain.Events;
using X10sions.Fake.Features.ToDo.Item;

namespace X10sions.Fake.Features.Project;

public record NewItemAddedEvent(Project Project, ToDoItem NewItem) : DomainEventBase;

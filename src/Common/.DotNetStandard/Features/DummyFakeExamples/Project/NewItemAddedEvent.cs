using Common.Data;
using Common.Features.DummyFakeExamples.ToDo.Item;

namespace Common.Features.DummyFakeExamples.Project;

public record NewItemAddedEvent(Project Project, ToDoItem NewItem) : DomainEventBase;

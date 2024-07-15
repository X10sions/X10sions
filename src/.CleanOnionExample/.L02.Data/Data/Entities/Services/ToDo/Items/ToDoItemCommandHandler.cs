using Common.Data;
using Common.Features.DummyFakeExamples.ToDo.Item;
using MediatR;
using X10sions.Fake.Features.ToDo.Item;
using X10sions.Fake.Features.ToDoItem;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemCommandHandler {
  private readonly IToDoItemFactory _factory;
  private readonly IToDoItemRepository _repository;
  private readonly IMediator _mediator;

  public ToDoItemCommandHandler(IToDoItemRepository repository, IToDoItemFactory factory, IMediator mediator) {
    _repository = repository;
    _factory = factory;
    _mediator = mediator;
  }

  public async Task<ToDoItem> HandleNewToDoItem(CreateNewToDoItemCommand createNewToDoItemCommand) {
    var task = _factory.CreateToDoItemInstance(
        summary: new ToDoItemSummary(createNewToDoItemCommand.Summary),
        description: new ToDoItemDescription(createNewToDoItemCommand.Description)
    );
    await _repository.InsertAsync(task);
    //var taskCreated = await _repository.InsertAsync(task);
    var taskCreated = task;
    // You may raise an event in case you need to propagate this change to other microservices
    await _mediator.Publish(new ToDoItemCreatedEvent(taskCreated.Id.Value,
        taskCreated.Description.ToString(), taskCreated.Summary.ToString()));
    return taskCreated;
  }

  public async Task HandleDeleteToDoItem(DeleteToDoItemCommand deleteCommand) {
    await _repository.DeleteAsync(new ToDoItemId(deleteCommand.Id));

    // You may raise an event in case you need to propagate this change to other microservices
    await _mediator.Publish(new ToDoItemDeletedEvent(deleteCommand.Id));
  }
}

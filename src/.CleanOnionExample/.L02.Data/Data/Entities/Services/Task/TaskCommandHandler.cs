namespace CleanOnionExample.Data.Entities.Services;

public class TaskCommandHandler {
  private readonly ITaskFactory _taskFactory;
  private readonly ITaskRepository _taskRepository;
  private readonly IMediator _mediator;

  public TaskCommandHandler(ITaskRepository taskRepository, ITaskFactory taskFactory, IMediator mediator) {
    _taskRepository = taskRepository;
    _taskFactory = taskFactory;
    _mediator = mediator;
  }

  public async Task<Task> HandleNewTask(CreateNewTaskCommand createNewTaskCommand) {
    var task = _taskFactory.CreateTaskInstance(
        summary: new TaskSummary(createNewTaskCommand.Summary),
        description: new TaskDescription(createNewTaskCommand.Description)
    );
    await _taskRepository.InsertAsync(task);
    //var taskCreated = await _taskRepository.InsertAsync(task);
    var taskCreated = task;
    // You may raise an event in case you need to propagate this change to other microservices
    await _mediator.Publish(new TaskCreatedEvent(taskCreated.Id.Value,
        taskCreated.Description.ToString(), taskCreated.Summary.ToString()));
    return taskCreated;
  }

  public async System.Threading.Tasks.Task HandleDeleteTask(DeleteTaskCommand deleteTaskCommand) {
    await _taskRepository.DeleteAsync(new TaskId(deleteTaskCommand.Id));

    // You may raise an event in case you need to propagate this change to other microservices
    await _mediator.Publish(new TaskDeletedEvent(deleteTaskCommand.Id));
  }
}

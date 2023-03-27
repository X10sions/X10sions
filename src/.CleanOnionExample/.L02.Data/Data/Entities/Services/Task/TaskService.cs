namespace CleanOnionExample.Data.Entities.Services;

public class TaskService : ITaskService {
  private readonly ITaskRepository _taskRepository;
  private readonly ITaskFactory _taskFactory;
  private readonly TaskViewModelMapper _taskViewModelMapper;
  private readonly IMediator _mediator;

  public TaskService(ITaskRepository taskRepository, TaskViewModelMapper taskViewModelMapper, ITaskFactory taskFactory, IMediator mediator) {
    _taskRepository = taskRepository;
    _taskViewModelMapper = taskViewModelMapper;
    _taskFactory = taskFactory;
    _mediator = mediator;
  }

  public async Task<TaskViewModel> Create(TaskViewModel taskViewModel) {
    var newTaskCommand = _taskViewModelMapper.ConvertToNewTaskCommand(taskViewModel);
    var taskResult = await _mediator.Send((IRequest<Task>)newTaskCommand);
    return _taskViewModelMapper.ConstructFromEntity(taskResult);
  }

  public async System.Threading.Tasks.Task Delete(Guid id) {
    var deleteTaskCommand = _taskViewModelMapper.ConvertToDeleteTaskCommand(id);
    await _mediator.Publish(deleteTaskCommand);
  }

  public async Task<IEnumerable<TaskViewModel>> GetAll() {
    var tasksEntities = await _taskRepository.GetAllAsync();
    return _taskViewModelMapper.ConstructFromListOfEntities(tasksEntities);
  }

  public async Task<TaskViewModel> GetById(Guid id) {
    var taskEntity = await _taskRepository.GetByIdAsync(new TaskId(id));
    return _taskViewModelMapper.ConstructFromEntity(taskEntity);
  }
}
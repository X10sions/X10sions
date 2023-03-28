namespace CleanOnionExample.Data.Entities.Services;

public class TaskViewModelMapper {
  public IEnumerable<TaskViewModel> ConstructFromListOfEntities(IEnumerable<ToDoTask> tasks) {
    var tasksViewModel = tasks.Select(i => new TaskViewModel {
      Id = i.Id.Value.ToString(),
      Description = i.Description.ToString(),
      Summary = i.Summary.ToString()
    }
    );
    return tasksViewModel;
  }

  public TaskViewModel ConstructFromEntity(ToDoTask task) {
    return new TaskViewModel {
      Id = task.Id.Value.ToString(),
      Description = task.Description.ToString(),
      Summary = task.Summary.ToString(),
    };
  }

  public CreateNewTaskCommand ConvertToNewTaskCommand(TaskViewModel taskViewModel) => new CreateNewTaskCommand(taskViewModel.Summary, taskViewModel.Description);

  public DeleteTaskCommand ConvertToDeleteTaskCommand(Guid id) => new DeleteTaskCommand(id);
}
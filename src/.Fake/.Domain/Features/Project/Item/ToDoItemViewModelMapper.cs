namespace X10sions.Fake.Features.ToDo.Item;

public class ToDoItemViewModelMapper {
  public IEnumerable<ToDoItemViewModel> ConstructFromListOfEntities(IEnumerable<ToDoItem> tasks) {
    var tasksViewModel = tasks.Select(i => new ToDoItemViewModel {
      Id = i.Id.Value.ToString(),
      Description = i.Description.ToString(),
      Summary = i.Summary.ToString()
    }
    );
    return tasksViewModel;
  }

  public ToDoItemViewModel ConstructFromEntity(ToDoItem task) {
    return new ToDoItemViewModel {
      Id = task.Id.Value.ToString(),
      Description = task.Description.ToString(),
      Summary = task.Summary.ToString(),
    };
  }

  public CreateNewToDoItemCommand ConvertToNewToDoItemCommand(ToDoItemViewModel taskViewModel) => new CreateNewToDoItemCommand(taskViewModel.Summary, taskViewModel.Description);

  public DeleteToDoItemCommand ConvertToDeleteToDoItemCommand(Guid id) => new DeleteToDoItemCommand(id);
}
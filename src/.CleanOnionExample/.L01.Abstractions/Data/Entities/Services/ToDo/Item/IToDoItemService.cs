namespace CleanOnionExample.Data.Entities.Services;

public interface IToDoItemService {
  Task<IEnumerable<ToDoItemViewModel>> GetAll();
  Task<ToDoItemViewModel> GetById(Guid id);
  Task<ToDoItemViewModel> Create(ToDoItemViewModel taskViewModel);
  Task Delete(Guid id);
}

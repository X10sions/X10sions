namespace X10sions.Fake.Features.ToDo.Item;

public interface IToDoItemService {
  Task<IEnumerable<ToDoItemViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<ToDoItemViewModel> GetByIdAsyc(Guid id, CancellationToken cancellationToken = default);
  Task<ToDoItemViewModel> Create(ToDoItemViewModel taskViewModel);
  Task Delete(Guid id);
}

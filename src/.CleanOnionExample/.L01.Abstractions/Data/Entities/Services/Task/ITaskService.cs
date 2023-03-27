namespace CleanOnionExample.Data.Entities.Services;

public interface ITaskService {
  System.Threading.Tasks.Task<IEnumerable<TaskViewModel>> GetAll();
  System.Threading.Tasks.Task<TaskViewModel> GetById(Guid id);
  System.Threading.Tasks.Task<TaskViewModel> Create(TaskViewModel taskViewModel);
  System.Threading.Tasks.Task Delete(Guid id);
}

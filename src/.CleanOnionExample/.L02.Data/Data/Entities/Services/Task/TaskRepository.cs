using System.Linq.Expressions;

namespace CleanOnionExample.Data.Entities.Services;

public class TaskRepository : ITaskRepository {
  private readonly ITaskFactory _taskFactory;

  public TaskRepository(ITaskFactory taskFactory) {
    _taskFactory = taskFactory;
  }

  public IQueryable<Task> Queryable => throw new NotImplementedException();

  public bool Any() {
    throw new NotImplementedException();
  }

  public bool Any(Expression<Func<Task, bool>> where) {
    throw new NotImplementedException();
  }

  public Task<bool> AnyAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<bool> AnyAsync(Expression<Func<Task, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public long Count() {
    throw new NotImplementedException();
  }

  public long Count(Expression<Func<Task, bool>> where) {
    throw new NotImplementedException();
  }

  public Task<long> CountAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<long> CountAsync(Expression<Func<Task, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void Delete(TaskId key) {
    throw new NotImplementedException();
  }

  public void Delete(Expression<Func<Task, bool>> where) {
    throw new NotImplementedException();
  }

  public Task<bool> DeleteAsync(TaskId id, CancellationToken cancellationToken = default) => throw new NotImplementedException();

  public System.Threading.Tasks.Task DeleteAsync(Expression<Func<Task, bool>> where, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<List<Task>> FindAll() {
    var tasks = System.Threading.Tasks.Task.FromResult(new List<Task> { _taskFactory.CreateTaskInstance(new TaskSummary("summary test"), new TaskDescription("description test")) });
    return tasks;
  }

  public Task<Task> FindById(Guid id) {
    return System.Threading.Tasks.Task.FromResult(_taskFactory.CreateTaskInstance(new TaskSummary("summary test"), new TaskDescription("description test")));
  }

  public IEnumerable<Task> GetAll() {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Task>> GetAllAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task GetById(TaskId key) {
    throw new NotImplementedException();
  }

  public Task<Task> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<Task?> GetFirstAsync(Expression<Func<Task, bool>> predicate, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<IList<Task>> GetListAsync(Expression<Func<Task, bool>> predicate, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public IQueryable<Task> GetQueryable(Expression<Func<Task, bool>> predicate) {
    throw new NotImplementedException();
  }

  public void Insert(Task item) {
    throw new NotImplementedException();
  }

  public Task<Task> InsertAsync(Task entity, CancellationToken cancellationToken = default) {
    return System.Threading.Tasks.Task.FromResult(_taskFactory.CreateTaskInstance(entity.Summary, entity.Description));
  }

  public void InsertRange(IEnumerable<Task> items) {
    throw new NotImplementedException();
  }

  public System.Threading.Tasks.Task InsertRangeAsync(IEnumerable<Task> items, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public System.Threading.Tasks.Task Remove(Guid id) {
    return System.Threading.Tasks.Task.CompletedTask;
  }

  public void Update(TaskId key, Task item) {
    throw new NotImplementedException();
  }

  public Task<Task> UpdateAsync(Task entity, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public System.Threading.Tasks.Task UpdateAsync(TaskId key, Task item, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void UpdatePartial(TaskId key, object item) {
    throw new NotImplementedException();
  }

  public System.Threading.Tasks.Task UpdatePartialAsync(TaskId key, object item, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public void UpdateRange(IEnumerable<Task> items) {
    throw new NotImplementedException();
  }

  public System.Threading.Tasks.Task UpdateRangeAsync(IEnumerable<Task> items, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  System.Threading.Tasks.Task ICommandRepository<Task, TaskId>.DeleteAsync(TaskId key, CancellationToken cancellationToken) {
    throw new NotImplementedException();
  }

  System.Threading.Tasks.Task ICommandRepository<Task, TaskId>.InsertAsync(Task item, CancellationToken cancellationToken) {
    throw new NotImplementedException();
  }
}
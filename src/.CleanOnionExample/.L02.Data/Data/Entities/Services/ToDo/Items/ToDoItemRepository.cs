using Common.Data.Repositories;
using System.Linq.Expressions;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemRepository : IToDoItemRepository {
  private readonly IToDoItemFactory _taskFactory;

  public ToDoItemRepository(IToDoItemFactory taskFactory) {
    _taskFactory = taskFactory;
  }

  public IQueryable<ToDoItem> Queryable => throw new NotImplementedException();
  public bool Any() => throw new NotImplementedException();
  public bool Any(Expression<Func<ToDoItem, bool>> where) => throw new NotImplementedException();
  public Task<bool> AnyAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<bool> AnyAsync(Expression<Func<ToDoItem, bool>> where, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public long Count() => throw new NotImplementedException();
  public long Count(Expression<Func<ToDoItem, bool>> where) => throw new NotImplementedException();
  public Task<long> CountAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<long> CountAsync(Expression<Func<ToDoItem, bool>> where, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public void Delete(ToDoItemId key) => throw new NotImplementedException();
  public void Delete(Expression<Func<ToDoItem, bool>> where) => throw new NotImplementedException();
  public Task<bool> DeleteAsync(ToDoItemId id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task DeleteAsync(Expression<Func<ToDoItem, bool>> where, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<List<ToDoItem>> FindAll() => Task.FromResult(new List<ToDoItem> { _taskFactory.CreateToDoItemInstance(new ToDoItemSummary("summary test"), new ToDoItemDescription("description test")) });
  public Task<ToDoItem> FindById(Guid id) => Task.FromResult(_taskFactory.CreateToDoItemInstance(new ToDoItemSummary("summary test"), new ToDoItemDescription("description test")));
  public IEnumerable<ToDoItem> GetAll() => throw new NotImplementedException();
  public Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task GetById(ToDoItemId key) => throw new NotImplementedException();
  ToDoItem IQueryRepository<ToDoItem, ToDoItemId>.GetById(ToDoItemId key) => throw new NotImplementedException();
  public Task<ToDoItem> GetByIdAsync(ToDoItemId id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<ToDoItem?> GetFirstAsync(Expression<Func<ToDoItem, bool>> predicate, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<IList<ToDoItem>> GetListAsync(Expression<Func<ToDoItem, bool>> predicate, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public IQueryable<ToDoItem> GetQueryable(Expression<Func<ToDoItem, bool>> predicate) => throw new NotImplementedException();
  public void Insert(ToDoItem item) => throw new NotImplementedException();
  public Task<ToDoItem> InsertAsync(ToDoItem entity, CancellationToken cancellationToken = default) => Task.FromResult(_taskFactory.CreateToDoItemInstance(entity.Summary, entity.Description));
  public void InsertRange(IEnumerable<ToDoItem> items) => throw new NotImplementedException();
  public Task InsertRangeAsync(IEnumerable<ToDoItem> items, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task Remove(Guid id) => Task.CompletedTask;
  public void Update(ToDoItemId key, ToDoItem item) => throw new NotImplementedException();
  public Task<ToDoItem> UpdateAsync(ToDoItem entity, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task UpdateAsync(ToDoItemId key, ToDoItem item, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public void UpdatePartial(ToDoItemId key, object item) => throw new NotImplementedException();
  public Task UpdatePartialAsync(ToDoItemId key, object item, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public void UpdateRange(IEnumerable<ToDoItem> items) => throw new NotImplementedException();
  public Task UpdateRangeAsync(IEnumerable<ToDoItem> items, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  Task ICommandRepository<ToDoItem, ToDoItemId>.DeleteAsync(ToDoItemId key, CancellationToken cancellationToken) => throw new NotImplementedException();
  Task ICommandRepository<ToDoItem, ToDoItemId>.InsertAsync(ToDoItem item, CancellationToken cancellationToken) => throw new NotImplementedException();
}
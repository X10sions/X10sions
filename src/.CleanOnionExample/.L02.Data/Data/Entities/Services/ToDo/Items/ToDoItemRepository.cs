using Common.Domain;
using Common.Domain.Repositories;
using System.Linq.Expressions;
using X10sions.Fake.Features.ToDo.Item;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemRepository(IToDoItemFactory taskFactory) : IToDoItemRepository {

  public IQuery<ToDoItem> Query => throw new NotImplementedException();

  public IQueryable<ToDoItem> Queryable => throw new NotImplementedException();

  public Task DeleteAsync(ToDoItemId key, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  public Task<int> DeleteAsync(IEnumerable<ToDoItem> rows, CancellationToken token = default) => throw new NotImplementedException();

  public Task DeleteByIdAsync(ToDoItemId key, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<List<ToDoItem>> FindAll() => Task.FromResult(new List<ToDoItem> { taskFactory.CreateToDoItemInstance(new ToDoItemSummary("summary test"), new ToDoItemDescription("description test")) });
  public Task<ToDoItem> FindById(Guid id) => Task.FromResult(taskFactory.CreateToDoItemInstance(new ToDoItemSummary("summary test"), new ToDoItemDescription("description test")));

  public Task<ToDoItem?> GetAsync(Expression<Func<ToDoItem, bool>> predicate, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<ToDoItem> GetByIdAsync(ToDoItemId key, CancellationToken cancellationToken = default) => throw new NotImplementedException();

  public Task<IEnumerable<ToDoItem>> GetListAsync(Expression<Func<ToDoItem, bool>> predicate, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }

  public Task<ToDoItem> InsertAsync(ToDoItem entity, CancellationToken cancellationToken = default) => Task.FromResult(taskFactory.CreateToDoItemInstance(entity.Summary, entity.Description));
  public Task<int> InsertAsync(IEnumerable<ToDoItem> rows, CancellationToken token = default) => throw new NotImplementedException();
  public Task<TKey> InsertWithIdAsync<TKey>(ToDoItem row, Func<ToDoItem, TKey>? idSelector = null, CancellationToken token = default) => throw new NotImplementedException();
  public Task Remove(Guid id) => Task.CompletedTask;
  public Task<int> UpdateAsync(ToDoItem row, CancellationToken token = default) => throw new NotImplementedException();

  public Task<int> UpdateAsync(IEnumerable<ToDoItem> rows, CancellationToken token = default) {
    throw new NotImplementedException();
  }

  public Task UpdateByIdAsync(ToDoItemId key, ToDoItem item, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}
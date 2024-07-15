using Common.Domain.Repositories;

namespace X10sions.Fake.Features.ToDo.Item;

public interface IToDoItemRepository : IRepository<ToDoItem, ToDoItemId> { }

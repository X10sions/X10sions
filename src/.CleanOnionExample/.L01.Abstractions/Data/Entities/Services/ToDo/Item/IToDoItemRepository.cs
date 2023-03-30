using Common.Data.Repositories;

namespace CleanOnionExample.Data.Entities.Services;

public interface IToDoItemRepository : IRepository<ToDoItem, ToDoItemId> { }

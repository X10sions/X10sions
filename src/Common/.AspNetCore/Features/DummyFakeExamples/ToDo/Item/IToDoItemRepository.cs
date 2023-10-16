using Common.Data.Repositories;

namespace Common.Features.DummyFakeExamples.ToDo.Item;

public interface IToDoItemRepository : IRepository<ToDoItem, ToDoItemId> { }

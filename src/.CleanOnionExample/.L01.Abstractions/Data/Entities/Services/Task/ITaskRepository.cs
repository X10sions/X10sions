using Common.Data.Repositories;

namespace CleanOnionExample.Data.Entities.Services;

public interface ITaskRepository : IRepository<ToDoTask, TaskId> { }

using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;

namespace CleanOnionExample.Infrastructure.Factories;

public class EntityFactory : ITaskFactory {
  public CleanOnionExample.Data.Entities.Task CreateTaskInstance(TaskSummary summary, TaskDescription description) {
    return new TaskFactory(summary, description);
  }
}

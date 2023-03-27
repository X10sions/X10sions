using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Infrastructure.Factories;
public class TaskFactory : Data.Entities.Task {
  public TaskFactory() { }

  public TaskFactory(TaskSummary summary, TaskDescription description) {
    Id = new TaskId(Guid.NewGuid());
    Summary = summary;
    Description = description;
  }
}

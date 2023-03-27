namespace CleanOnionExample.Data.Entities.Services;

public interface ITaskFactory {
  Task CreateTaskInstance(TaskSummary summary, TaskDescription description);
}
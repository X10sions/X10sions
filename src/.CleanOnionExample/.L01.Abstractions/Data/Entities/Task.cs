using Common.Data;

namespace CleanOnionExample.Data.Entities;
public class ToDoTask : IEntity<TaskId> {
  public TaskId Id { get; set; }
  public TaskSummary Summary { get; set; }
  public TaskDescription Description { get; set; }
}

public record TaskDescription(string Value) {
  public string Value { get; } = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentException($"Description value is required") : Value;
}

public record TaskSummary(string Value) {
  public string Value { get; } = string.IsNullOrWhiteSpace(Value) ? throw new ArgumentException("Summary value is required") : Value;
}

public record TaskId(Guid Value) {
  public Guid Value { get; } = Value.Equals(Guid.Empty) ? throw new ArgumentException("Task Id does not have any value") : Value;
}
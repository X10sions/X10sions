using CleanOnionExample.Data.Services;
using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;
public partial class ToDoItem : EntityBase<int> {
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public bool IsDone { get; set; }

  public void MarkComplete() {
    if (!IsDone) {
      IsDone = true;
      Events.Add(new ToDoItemCompletedEvent(this));
    }
  }
  public override string ToString() => $"{Id}: Status: {(IsDone ? "Done!" : "Not done.")} - {Title} - {Description}";
}


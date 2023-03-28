using CleanOnionExample.Data.Services;
using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;
public class Project : EntityBase<int> {
  public Project(string name, PriorityStatus priority) {
    Name = Throw.IfNullOrWhiteSpace(name, nameof(name));
    Priority = priority;
  }

  public string Name { get; private set; }
  private List<ToDoItem> _items = new List<ToDoItem>();
  public IEnumerable<ToDoItem> Items => _items.AsReadOnly();
  public ProjectStatus Status => _items.All(i => i.IsDone) ? ProjectStatus.Complete : ProjectStatus.InProgress;
  public PriorityStatus Priority { get; }

  public void AddItem(ToDoItem newItem) {
    Throw.IfNull(newItem, nameof(newItem));
    _items.Add(newItem);
    var newItemAddedEvent = new NewItemAddedEvent(this, newItem);
    Events.Add(newItemAddedEvent);
  }

  public void UpdateName(string newName) {
    Name = Throw.IfNullOrWhiteSpace(newName, nameof(newName));
  }

}

public enum ProjectStatus { InProgress, Complete }
public enum PriorityStatus { Backlog = 0, Critical = 1 }

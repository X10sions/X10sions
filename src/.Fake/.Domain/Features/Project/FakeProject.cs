using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using X10sions.Fake.Features.Project.Item;
using X10sions.Fake.Features.ToDo.Item;

namespace X10sions.Fake.Features.Project;
[Table("FakeProject")]
public class FakeProject : EntityBase<int> {
  public FakeProject(string name, PriorityStatus priority) {
    Name = Throw.IfNullOrWhiteSpace(name, nameof(name));
    Priority = priority;
  }
  [ServiceStack.DataAnnotations.AutoIncrement, Column] public int Id { get; set; }
  [Column] public string Title { get; set; } = string.Empty;


  public string Name { get; private set; }
  private List<ToDoItem> _items = new List<ToDoItem>();
  public IEnumerable<ToDoItem> Items => _items.AsReadOnly();
  // public IEnumerable<FakeProjectItem> Items { get; set; } = null!;

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

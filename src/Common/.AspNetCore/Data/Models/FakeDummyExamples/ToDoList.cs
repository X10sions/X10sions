using CleanOnionExample.Data.Entities.ValueObjects;
using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public partial class ToDoList : EntityAuditableBase<int> {
  public string? Title { get; set; }
  public Colour Colour { get; set; } = Colour.White;
  public IList<ToDoItem> Items { get; private set; } = new List<ToDoItem>();
}

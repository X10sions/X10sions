using Common.Domain.Entities;
using Common.ValueObjects;
using X10sions.Fake.Features.ToDo.Item;

namespace X10sions.Fake.Features.ToDo;

public partial class ToDoList : EntityAuditableBase<int> {
  public string? Title { get; set; }
  public ColourCode Colour { get; set; } = ColourCode.White;
  public IList<ToDoItem> Items { get; private set; } = new List<ToDoItem>();
}

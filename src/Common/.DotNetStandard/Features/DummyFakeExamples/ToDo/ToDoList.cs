using Common.Domain.Entities;
using Common.Domain.ValueObjects;
using Common.Features.DummyFakeExamples.ToDo.Item;

namespace Common.Features.DummyFakeExamples.ToDo;

public partial class ToDoList : EntityAuditableBase<int> {
  public string? Title { get; set; }
  public Colour Colour { get; set; } = Colour.White;
  public IList<ToDoItem> Items { get; private set; } = new List<ToDoItem>();
}

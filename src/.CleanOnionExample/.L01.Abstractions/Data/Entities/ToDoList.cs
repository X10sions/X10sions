using CleanOnionExample.Data.Entities.ValueObjects;
using Common.Data;

namespace CleanOnionExample.Data.Entities;

public partial class ToDoList : BaseEntityAuditable<int> {
  public int Id { get; set; }
  public string? Title { get; set; }
  public Colour Colour { get; set; } = Colour.White;
  public IList<ToDoItem> Items { get; private set; } = new List<ToDoItem>();
}

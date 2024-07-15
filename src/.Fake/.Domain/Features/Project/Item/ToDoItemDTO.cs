using System.ComponentModel.DataAnnotations;

namespace X10sions.Fake.Features.ToDo.Item;

public record ToDoItemDTO(Guid Id, [Required] string? Title, string? Description, bool IsDone) {
  public ToDoItemDTO(ToDoItem item) : this(item.Id.Value, item.Title, item.Description.Value, item.IsDone) { }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace X10sions.Fake.Features.ToDo.Item;

public class ToDoItemViewModel {
  public ToDoItemViewModel() { }

  public ToDoItemViewModel(Guid taskId, string summary, string description) {
    Id = taskId.ToString();
    Summary = summary;
    Description = description;
  }
  public string Id { get; set; }
  [Required, MinLength(5), MaxLength(150)] public string Description { get; set; }
  [MaxLength(1500), JsonPropertyName("summary")] public string Summary { get; set; }
}
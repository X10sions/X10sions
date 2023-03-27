namespace CleanOnionExample.Data.Entities.Services;

public class TaskViewModel {
  public TaskViewModel() { }

  public TaskViewModel(Guid taskId, string summary, string description) {
    Id = taskId.ToString();
    Summary = summary;
    Description = description;
  }
  public string Id { get; set; }
  [Required, MinLength(5), MaxLength(150)] public string Description { get; set; }
  [MaxLength(1500), JsonPropertyName("summary")] public string Summary { get; set; }
}
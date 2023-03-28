using CleanOnionExample.Data.Entities;
using Common.Data;

namespace CleanOnionExample.Data.Services;

public record NewItemAddedEvent(Project Project, ToDoItem NewItem) : DomainEventBase;

public record ProjectDTO(int Id, string Name) : CreateProjectDTO(Name) {
  public ProjectDTO(int Id, string Name, List<ToDoItemDTO>? items = null) : this(Id, Name) => Items = items ?? new List<ToDoItemDTO>();
  public List<ToDoItemDTO> Items { get; set; } = new List<ToDoItemDTO>();
}

public abstract record CreateProjectDTO(string Name);

public static class ProjectExtensions {
  public async static Task<Project?> GetByIdAsync(this IRepository<Project> repository, int id, CancellationToken token = default) => await repository.Query.FirstOrDefaultAsync(x => x.Id == id, token);
}
using RCommon.Persistence.Crud;
using X10sions.Fake.Features.ToDo.Item;

namespace X10sions.Fake.Features.Project;


public record ProjectDTO(int Id, string Name) : CreateProjectDTO(Name) {
  public ProjectDTO(int Id, string Name, List<ToDoItemDTO>? items = null) : this(Id, Name) => Items = items ?? new List<ToDoItemDTO>();
  public List<ToDoItemDTO> Items { get; set; } = new List<ToDoItemDTO>();
}

public abstract record CreateProjectDTO(string Name);

public static class ProjectExtensions {
  public async static Task<Project?> GetByIdAsync(this IReadOnlyRepository<Project> repository, int id, CancellationToken token = default) => await repository.GetAsync(x => x.Id == id, token);
}
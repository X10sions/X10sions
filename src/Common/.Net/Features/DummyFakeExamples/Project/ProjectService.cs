using Common.Data;
using Common.Features.DummyFakeExamples.ToDo.Item;

namespace Common.Features.DummyFakeExamples.Project;


public record ProjectDTO(int Id, string Name) : CreateProjectDTO(Name) {
  public ProjectDTO(int Id, string Name, List<ToDoItemDTO>? items = null) : this(Id, Name) => Items = items ?? new List<ToDoItemDTO>();
  public List<ToDoItemDTO> Items { get; set; } = new List<ToDoItemDTO>();
}

public abstract record CreateProjectDTO(string Name);

public static class ProjectExtensions {
  public async static Task<Project?> GetByIdAsync(this IRepository<Project> repository, int id, CancellationToken token = default) => await repository.Query.FirstOrDefaultAsync(x => x.Id == id, token);
}
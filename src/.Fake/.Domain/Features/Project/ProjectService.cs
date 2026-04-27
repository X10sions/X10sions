using RCommon.Persistence.Crud;
using X10sions.Fake.Features.ToDo.Item;

namespace X10sions.Fake.Features.Project;

public record ProjectDTO(int Id, string Name, List<ToDoItemDTO> Items) : CreateProjectDTO(Name) {
  public ProjectDTO(int Id, string Name) : this(Id, Name, new List<ToDoItemDTO>()) { }
}

public abstract record CreateProjectDTO(string Name);

public static class ProjectExtensions {
  public async static Task<FakeProject?> GetByIdAsync(this IReadOnlyRepository<FakeProject> repository, int id, CancellationToken token = default) => await repository.GetAsync(x => x.Id == id, token);
}
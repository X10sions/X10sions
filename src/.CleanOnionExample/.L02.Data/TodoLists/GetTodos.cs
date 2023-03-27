using CleanOnionExample.Data.Entities;
using Common.Mappings;

namespace CleanOnionExample.Application.TodoLists;

public static class GetTodos {

  public class GetTodosQuery : IRequest<TodosVm> {
  }
  public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, TodosVm> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper) {
      _context = context;
      _mapper = mapper;
    }

    public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken) {
      return new TodosVm {
        PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
              .Cast<PriorityLevel>()
              .Select(p => new PriorityLevelDto((int)p, p.ToString()))
              .ToList(),

        Lists = await _context.TodoLists
              .AsNoTracking()
              .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
              .OrderBy(t => t.Title)
              .ToListAsync(cancellationToken)
      };
    }
  }

  public class TodosVm {
    public IList<PriorityLevelDto> PriorityLevels { get; set; } = new List<PriorityLevelDto>();
    public IList<TodoListDto> Lists { get; set; } = new List<TodoListDto>();
  }

  public record PriorityLevelDto(int Value, string? Name);

  public record TodoListDto(int Id, string? Title, string? Colour, IList<ExportTodos.TodoItemDto> Items) : IMapFrom<ToDoList> {
    IList<ExportTodos.TodoItemDto> Items { get; set; } = new List<ExportTodos.TodoItemDto>();
  }


}
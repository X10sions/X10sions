using AutoMapper;
using CleanOnionExample.Data.DbContexts;
using Common.Interfaces;
using Common.Mappings;
using FluentValidation;
using MediatR;
namespace CleanOnionExample.Data.Entities.Services;

public static class ExportTodos {
  public class Query : IRequest<ViewModel> {
    public int ListId { get; set; }
  }

  public class QueryHandler : IRequestHandler<Query, ViewModel> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICsvFileBuilder _fileBuilder;

    public QueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder) {
      _context = context;
      _mapper = mapper;
      _fileBuilder = fileBuilder;
    }

    public async Task<ViewModel> Handle(Query request, CancellationToken cancellationToken) {
      var records = await _context.TodoItems
              .Where(t => t.ListId == request.ListId)
              .ProjectTo<ToDoItemRecord>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken);
      var vm = new ViewModel(
          "TodoItems.csv",
          "text/csv",
          _fileBuilder.BuildTodoItemsFile(records));
      return vm;
    }
  }

  public record ViewModel(string FileName, string ContentType, byte[] Content);

  public record ToDoItemRecord(string? itle, bool Done) : IMapFrom<ToDoItem>;

  public record TodoItemDto(int Id, int ListId, string? Title, bool Done, int Priority, string? Note) : IMapFrom<ToDoItem> {
    public void Mapping(Profile profile) {
      profile.CreateMap<ToDoItem, TodoItemDto>()
          .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
    }
  }

}
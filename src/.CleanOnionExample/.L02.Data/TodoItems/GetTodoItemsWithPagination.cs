using CleanOnionExample.Data.Entities;
using Common.Mappings;
using Common.Models;

namespace CleanOnionExample.Application.TodoItems;
public static class GetTodoItemsWithPagination {
  public record Query(int ListId, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<TodoItemBriefDto>>;

  public class QueryHandler : IRequestHandler<Query, PaginatedList<TodoItemBriefDto>> {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QueryHandler(IApplicationDbContext context, IMapper mapper) {
      _context = context;
      _mapper = mapper;
    }

    public async Task<PaginatedList<TodoItemBriefDto>> Handle(Query request, CancellationToken cancellationToken) {
      return await _context.TodoItems
          .Where(x => x.ListId == request.ListId)
          .OrderBy(x => x.Title)
          .ProjectTo<TodoItemBriefDto>(_mapper.ConfigurationProvider)
          .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
  }
  public record TodoItemBriefDto(int Id, int ListId, string? Title, bool Done) : IMapFrom<ToDoItem>;

  public class QueryValidator : AbstractValidator<Query> {
    public QueryValidator() {
      RuleFor(x => x.ListId).NotEmpty().WithMessage("ListId is required.");
      RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");
      RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
  }


}

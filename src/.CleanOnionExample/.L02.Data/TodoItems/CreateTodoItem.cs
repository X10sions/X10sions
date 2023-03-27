using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Application.TodoItems;

public static class CreateTodoItem {
  public record Command(int ListId, string? Title) : IRequest<int>;

  public class CommandHandler : IRequestHandler<Command, int> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<int> Handle(Command request, CancellationToken cancellationToken) {
      var entity = new ToDoItem {
        ListId = request.ListId,
        Title = request.Title,
        Done = false
      };

      entity.DomainEvents.Add(new ToDoItem.CreatedEvent(entity));

      _context.TodoItems.Add(entity);

      await _context.SaveChangesAsync(cancellationToken);

      return entity.Id;
    }
  }

  public class Validator : AbstractValidator<Command> {
    public Validator() {
      RuleFor(v => v.Title).MaximumLength(200).NotEmpty();
    }
  }

}
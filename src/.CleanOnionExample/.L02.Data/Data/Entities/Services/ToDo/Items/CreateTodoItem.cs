using CleanOnionExample.Data.DbContexts;
using FluentValidation;
using MediatR;
using X10sions.Fake.Features.ToDo.Item;

namespace CleanOnionExample.Data.Entities.Services;

public static class CreateToDoItem {
  public record Command(int ListId, string? Title) : IRequest<ToDoItemId>;

  public class CommandHandler : IRequestHandler<Command, ToDoItemId> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<ToDoItemId> Handle(Command request, CancellationToken cancellationToken) {
      var entity = new ToDoItem {
        ListId = request.ListId,
        Title = request.Title,
        IsDone = false
      };
      entity.Events.Add(new ToDoItemCreatedEvent(entity));
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
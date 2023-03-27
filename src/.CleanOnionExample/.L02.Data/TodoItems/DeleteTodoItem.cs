
using CleanOnionExample.Data.Entities;
using Common.Exceptions;

namespace CleanOnionExample.Application.TodoItems;

public static class DeleteTodoItem {
  public record Command(int Id) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    private readonly IApplicationDbContext _context;

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoItems
          .FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null) {
        throw new NotFoundException(nameof(ToDoItem), request.Id);
      }

      _context.TodoItems.Remove(entity);

      entity.DomainEvents.Add(new ToDoItem.DeletedEvent(entity));

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }
  }
}
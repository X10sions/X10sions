using CleanOnionExample.Data.DbContexts;
using Common.Exceptions;
using MediatR;
using X10sions.Fake.Features.ToDo.Item;

namespace CleanOnionExample.Data.Entities.Services;

public static class DeleteToDoItem {
  public record Command(int Id) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    private readonly IApplicationDbContext _context;

    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken);

      if (entity == null) {
        throw new NotFoundException(nameof(ToDoItem), request.Id);
      }
      _context.TodoItems.Remove(entity);
      entity.Events.Add(new ToDoItemDeletedEvent(entity));
      await _context.SaveChangesAsync(cancellationToken);
      //return Unit.Value;
    }
  }
}
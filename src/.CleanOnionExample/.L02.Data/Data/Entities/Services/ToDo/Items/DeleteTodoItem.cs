using CleanOnionExample.Data.DbContexts;
using Common.Exceptions;
using Common.Features.DummyFakeExamples.ToDo.Item;
using MediatR;

namespace CleanOnionExample.Data.Entities.Services;

public static class DeleteToDoItem {
  public record Command(int Id) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    private readonly IApplicationDbContext _context;

    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoItems.FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null) {
        throw new NotFoundException(nameof(ToDoItem), request.Id);
      }
      _context.TodoItems.Remove(entity);
      entity.Events.Add(new ToDoItem.DeletedEvent(entity));
      await _context.SaveChangesAsync(cancellationToken);
      //return Unit.Value;
    }
  }
}
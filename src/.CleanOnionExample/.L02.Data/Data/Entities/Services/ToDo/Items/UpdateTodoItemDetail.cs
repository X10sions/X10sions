using CleanOnionExample.Data.DbContexts;
using Common.Enums;
using Common.Exceptions;
using MediatR;
using X10sions.Fake.Features.ToDo.Item;

namespace CleanOnionExample.Data.Entities.Services;

public static class UpdateTodoItemDetail {
  public record Command(int Id, int ListId, PriorityLevel Priority, string Note) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken);
      if (entity == null) {
        throw new NotFoundException(nameof(ToDoItem), request.Id);
      }
      entity.ListId = request.ListId;
      entity.Priority = request.Priority;
      entity.Note = request.Note;
      await _context.SaveChangesAsync(cancellationToken);
      //return Unit.Value;
    }
  
  }
}
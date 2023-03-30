using CleanOnionExample.Data.DbContexts;
using Common.Security;
using MediatR;

namespace CleanOnionExample.Data.Entities.Services;

public static class PurgeTodoLists {
  [Authorize(Roles = "Administrator")]
  [Authorize(Policy = "CanPurge")]
  public record Command : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
      _context.TodoLists.RemoveRange(_context.TodoLists);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }

}

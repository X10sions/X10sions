using CleanOnionExample.Data.DbContexts;
using Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities.Services;

public static class DeleteTodoList {

  public record Command(int Id) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoLists
          .Where(l => l.Id == request.Id)
          .SingleOrDefaultAsync(cancellationToken);
      if (entity == null) {
        throw new NotFoundException(nameof(ToDoList), request.Id);
      }
      _context.TodoLists.Remove(entity);
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }


}

using CleanOnionExample.Data.DbContexts;
using CleanOnionExample.Data.Entities;
using Common.Exceptions;
using FluentValidation;

namespace CleanOnionExample.Application.TodoItems;

public static class UpdateTodoItem {
  public record Command(int Id, string? Title, bool Done) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoItems
          .FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null) {
        throw new NotFoundException(nameof(ToDoItem), request.Id);
      }
      entity.Title = request.Title;
      entity.Done = request.Done;
      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }

  public class CommandValidator : AbstractValidator<Command> {
    public CommandValidator() {
      RuleFor(v => v.Title).MaximumLength(200).NotEmpty();
    }
  }


}
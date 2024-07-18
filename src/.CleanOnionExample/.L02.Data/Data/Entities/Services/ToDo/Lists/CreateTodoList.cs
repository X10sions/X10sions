using CleanOnionExample.Data.DbContexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using X10sions.Fake.Features.ToDo;

namespace CleanOnionExample.Data.Entities.Services;

public static class CreateTodoList {
  public record Command(string? Title) : IRequest<int>;

  public class CommandHandler : IRequestHandler<Command, int> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task<int> Handle(Command request, CancellationToken cancellationToken) {
      var entity = new ToDoList();
      entity.Title = request.Title;
      _context.TodoLists.Add(entity);
      await _context.SaveChangesAsync(cancellationToken);
      return entity.Id;
    }
  }

  public class CommandValidator : AbstractValidator<Command> {
    private readonly IApplicationDbContext _context;

    public CommandValidator(IApplicationDbContext context) {
      _context = context;
      RuleFor(v => v.Title)
          .NotEmpty().WithMessage("Title is required.")
          .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
          .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken) {
      return await _context.TodoLists.AllAsync(l => l.Title != title, cancellationToken);
    }
  }

}

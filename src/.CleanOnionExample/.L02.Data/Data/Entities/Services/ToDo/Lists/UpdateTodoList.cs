using CleanOnionExample.Data.DbContexts;
using Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using X10sions.Fake.Features.ToDo;

namespace CleanOnionExample.Data.Entities.Services;

public static class UpdateTodoList {

  public record Command(int Id, string? Title) : IRequest;

  public class CommandHandler : IRequestHandler<Command> {
    private readonly IApplicationDbContext _context;

    public CommandHandler(IApplicationDbContext context) {
      _context = context;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var entity = await _context.TodoLists.FindAsync(new object[] { request.Id }, cancellationToken);

      if (entity == null) {
        throw new NotFoundException(nameof(ToDoList), request.Id);
      }
      entity.Title = request.Title;
      await _context.SaveChangesAsync(cancellationToken);
      //return Unit.Value;
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

    public async Task<bool> BeUniqueTitle(Command model, string title, CancellationToken cancellationToken) {
      return await _context.TodoLists
          .Where(l => l.Id != model.Id)
          .AllAsync(l => l.Title != title, cancellationToken);
    }
  }


}
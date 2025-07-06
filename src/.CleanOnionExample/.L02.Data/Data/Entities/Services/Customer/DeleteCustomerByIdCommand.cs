using CleanOnionExample.Data.DbContexts;
using Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities.Services;

public class DeleteCustomerByIdCommand : IRequest<int> {
  public int Id { get; set; }

  public class Handler : IRequestHandler<DeleteCustomerByIdCommand, int> {
    private readonly IApplicationDbContext _context;
    public Handler(IApplicationDbContext context) {
      _context = context;
    }
    public async Task<int> Handle(DeleteCustomerByIdCommand request, CancellationToken cancellationToken) {
      var customer = await _context.Customers.Where(a => a.Id == request.Id).FirstOrDefaultAsync();
      if (customer == null) return default;
      _context.Customers.Remove(customer);
      await _context.SaveChangesAsync(cancellationToken);
      return customer.Id;
    }
  }
}

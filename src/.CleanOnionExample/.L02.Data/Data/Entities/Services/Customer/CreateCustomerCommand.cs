using AutoMapper;
using CleanOnionExample.Data.DbContexts;
using MediatR;
using X10sions.Fake.Features.Customer;

namespace CleanOnionExample.Data.Entities.Services;

public class CreateCustomerCommand : IRequest<int> {
  public string CustomerName { get; set; }
  public string ContactName { get; set; }
  public string ContactTitle { get; set; }
  public string Address { get; set; }
  public string City { get; set; }
  public string Region { get; set; }
  public string PostalCode { get; set; }
  public string Country { get; set; }
  public string Phone { get; set; }
  public string Fax { get; set; }

  public class Handler : IRequestHandler<CreateCustomerCommand, int> {
    private readonly IApplicationDbContext _context;
    public Handler(IApplicationDbContext context) {
      _context = context;
    }
    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken) {
      var customer = new Customer();
      customer.CustomerName = request.CustomerName;
      customer.ContactName = request.ContactName;

      _context.Customers.Add(customer);
      await _context.SaveChangesAsync(cancellationToken);
      return customer.Id;
    }
  }
}

namespace CleanOnionExample.Data.Entities.Services;
public class GetCustomerByIdQuery : IRequest<Customer> {
  public int Id { get; set; }
  public class Handler : IRequestHandler<GetCustomerByIdQuery, Customer> {
    private readonly IApplicationDbContext _context;
    public Handler(IApplicationDbContext context) {
      _context = context;
    }
    public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken) {
      var customer = _context.Customers.Where(a => a.Id == request.Id).FirstOrDefault();
      if (customer == null) return null;
      return customer;
    }
  }
}

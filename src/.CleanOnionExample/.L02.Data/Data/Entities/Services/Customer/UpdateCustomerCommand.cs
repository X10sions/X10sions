﻿using CleanOnionExample.Data.DbContexts;
using Common.Data;
using MediatR;

namespace CleanOnionExample.Data.Entities.Services;

public class UpdateCustomerCommand : IRequest<int> {
  public int Id { get; set; }
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

  public class Handler : IRequestHandler<UpdateCustomerCommand, int> {
    private readonly IApplicationDbContext _context;
    public Handler(IApplicationDbContext context) {
      _context = context;
    }
    public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken) {
      var cust = _context.Customers.Where(a => a.Id == request.Id).FirstOrDefault();

      if (cust == null) {
        return default;
      } else {
        cust.CustomerName = request.CustomerName;
        cust.ContactName = request.ContactName;
        _context.Customers.Update(cust);
        await _context.SaveChangesAsync(cancellationToken);
        return cust.Id;
      }
    }
  }
}

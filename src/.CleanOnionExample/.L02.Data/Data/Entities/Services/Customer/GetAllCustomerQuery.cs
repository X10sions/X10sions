﻿using CleanOnionExample.Data.DbContexts;
using Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities.Services;

public class GetAllCustomerQuery : IRequest<IEnumerable<Customer>> {

  public class GetAllCustomerQueryHandler : IRequestHandler<GetAllCustomerQuery, IEnumerable<Customer>> {
    private readonly IApplicationDbContext _context;
    public GetAllCustomerQueryHandler(IApplicationDbContext context) {
      _context = context;
    }
    public async Task<IEnumerable<Customer>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken) {
      var customerList = await _context.Customers.ToListAsync();
      if (customerList == null) {
        return null;
      }
      return customerList.AsReadOnly();
    }
  }
}

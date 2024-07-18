using Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class GetAllProducts  {
  public record Response(int Id, string Name, string Barcode, string Description, decimal Rate);
  public record Query(int PageNumber, int PageSize) : IRequest<PagedListResult<Response>>;
  public class QueryHandler(IProductRepository repository) : IRequestHandler<Query, PagedListResult<Response>> {
    public async Task<PagedListResult<Response>> Handle(Query request, CancellationToken cancellationToken) {
      Expression<Func<Product, Response>> expression = e => new Response(
        e.Id,
        e.Name,
        e.Barcode,
        e.Description,
        e.Rate
      );
      var paginatedList = await repository.Products.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
      return paginatedList;
    }
  }
}

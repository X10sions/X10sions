using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class GetProductById {
  public record Response(int Id, string Name, string Barcode, byte[] Image, string Description, decimal Rate, int BrandId);
  public record Query(int Id) : IRequest<Result<Response>>;
  public class QueryHandler(IProductCacheRepository productCache, IMapper mapper) : IRequestHandler<Query, IResult<Response>> {
    public async Task<IResult<Response>> Handle(Query query, CancellationToken cancellationToken) {
      var product = await productCache.GetByIdAsync(query.Id);
      var mappedProduct = mapper.Map<Response>(product);
      return Result.Success(mappedProduct);
    }
  }
}

using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class GetAllProductsCached {
  public record Response(int Id, string Name, string Barcode, byte[] Image, string Description, decimal Rate, int BrandId);

  public record Query : IRequest<Result<List<Response>>>;

  public class QueryHandler(IProductCacheRepository productCache, IMapper mapper) : IRequestHandler<Query, IResult<List<Response>>> {

    public async Task<IResult<List<Response>>> Handle(Query request, CancellationToken cancellationToken) {
      var productList = await productCache.GetCachedListAsync();
      var mappedProducts = mapper.Map<List<Response>>(productList);
      return Result.Success(mappedProducts);
    }
  }

}

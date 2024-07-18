using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Brand;

namespace CleanOnionExample.Data.Entities.Services;

public class GetAllBrandsCached {
  public record Response(int Id, string Name, string Description, decimal Tax);

  public class Query : IRequest<Result<List<Response>>>;

  public class GetAllBrandsCachedQueryHandler(IBrandCacheRepository productCache, IMapper mapper) : IRequestHandler<Query, IResult<List<Response>>> {

    public async Task<IResult<List<Response>>> Handle(Query request, CancellationToken cancellationToken) {
      var brandList = await productCache.GetCachedListAsync();
      var mappedBrands = mapper.Map<List<Response>>(brandList);
      return Result.Success(mappedBrands);
    }
  }
}

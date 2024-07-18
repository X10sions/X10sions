using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Brand;

namespace CleanOnionExample.Data.Entities.Services;

public class GetBrandById {

  public record Response(int Id, string Name, decimal Tax, string Description);

  public record Query(int Id) : IRequest<Result<Response>>;

  public class GetProductByIdQueryHandler : IRequestHandler<Query, IResult<Response>> {
    private readonly IBrandCacheRepository _brandCache;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IBrandCacheRepository productCache, IMapper mapper) {
      _brandCache = productCache;
      _mapper = mapper;
    }

    public async Task<IResult<Response>> Handle(Query query, CancellationToken cancellationToken) {
      var product = await _brandCache.GetByIdAsync(query.Id);
      var mappedProduct = _mapper.Map<Response>(product);
      return Result<Response>.Success(mappedProduct);
    }
  }

}

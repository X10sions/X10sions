using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Brand;

namespace CleanOnionExample.Data.Entities.Services;

public partial class CreateBrand {
  public partial class Command(string Name, string Description, decimal Tax) : IRequest<Result<int>>;

  public class CommandHandler(IBrandRepository brandRepository, IMapper mapper) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken) {
      var product = mapper.Map<Brand>(request);
      await brandRepository.InsertAsync(product);
      //await _brandRepository.SaveChangesAsync(cancellationToken);
      return Result.Success(product.Id);
    }
  }
}

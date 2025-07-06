using Common.Results;
using MediatR;
using X10sions.Fake.Features.Brand;

namespace CleanOnionExample.Data.Entities.Services;

public class DeleteBrand{
  public record Command(int Id) : IRequest<Result<int>>;
    public class CommandHandler(IBrandRepository brandRepository) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command command, CancellationToken cancellationToken) {
      var product = await brandRepository.GetByIdAsync(command.Id);
      await brandRepository.DeleteAsync(product);
      //await _brandRepository.SaveChangesAsync(cancellationToken);
      return Result.Success(product.Id);
    }
  }
}

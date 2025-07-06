using Common.Exceptions;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class UpdateProductImage {
  public record Command(int Id, byte[] Image) : IRequest<IResult<int>>;
  public class CommandHandler(IProductRepository productRepository) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command command, CancellationToken cancellationToken) {
      var product = await productRepository.GetByIdAsync(command.Id);

      if (product == null) {
        throw new ApiException($"Product Not Found.");
      } else {
        product.Image = command.Image;
        await productRepository.UpdateAsync(product);
        //await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id);
      }
    }
  }
}

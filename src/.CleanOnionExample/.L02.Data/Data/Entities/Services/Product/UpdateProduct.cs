using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class UpdateProduct {
  public record Command(int Id, string Name, string Description, decimal Rate, int BrandId) : IRequest<Result<int>>;

  public class CommandHandler(IProductRepository productRepository) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command command, CancellationToken cancellationToken) {
      var product = await productRepository.GetByIdAsync(command.Id);
      if (product == null) {
        return Result.Fail<int>(Product.Errors.NotFound);
      }
      product.Name = command.Name ?? product.Name;
      product.Rate = (command.Rate == 0) ? product.Rate : command.Rate;
      product.Description = command.Description ?? product.Description;
      product.BrandId = (command.BrandId == 0) ? product.BrandId : command.BrandId;
      await productRepository.UpdateAsync(product);
      //await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result.Success(product.Id);
    }
  }
}

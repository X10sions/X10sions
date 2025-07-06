using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public class DeleteProduct{
  public record Command(int Id) : IRequest<Result<int>>;


  public class CommandHandler(IProductRepository productRepository) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command command, CancellationToken cancellationToken) {
      var product = await productRepository.GetByIdAsync(command.Id);
      await productRepository.DeleteAsync(product);
      //await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result.Success(product.Id);
    }
  }
}

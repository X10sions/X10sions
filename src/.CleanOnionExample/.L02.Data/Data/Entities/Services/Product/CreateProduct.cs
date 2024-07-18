using AutoMapper;
using Common.Results;
using MediatR;
using X10sions.Fake.Features.Product;

namespace CleanOnionExample.Data.Entities.Services;

public  class CreateProduct {
  public record Command(string Name, string Barcode, string Description, decimal Rate, int BrandId) : IRequest<Result<int>>;

  public class CommandHandler(IProductRepository productRepository, IMapper mapper) : IRequestHandler<Command, IResult<int>> {

    public async Task<IResult<int>> Handle(Command request, CancellationToken cancellationToken) {
      var product = mapper.Map<Product>(request);
      await productRepository.InsertAsync(product);
      //await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result.Success(product.Id);
    }
  }

}

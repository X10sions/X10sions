using Common.Results;
using MediatR;
using X10sions.Fake.Features.Brand;

namespace CleanOnionExample.Data.Entities.Services;

public class UpdateBrand {
  public record Command(int Id, string Name, string Description, decimal Tax) : IRequest<Result<int>> {

    public class CommandHandler(IBrandRepository brandRepository) : IRequestHandler<Command, IResult<int>> {

      public async Task<IResult<int>> Handle(Command command, CancellationToken cancellationToken) {
        var brand = await brandRepository.GetByIdAsync(command.Id);
        if (brand == null) {
          return Result.Fail<int>(Brand.Errors.NotFound);
        }
        brand.Name = command.Name ?? brand.Name;
        brand.Tax = (command.Tax == 0) ? brand.Tax : command.Tax;
        brand.Description = command.Description ?? brand.Description;
        await brandRepository.UpdateAsync(brand);
        //await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(brand.Id);

      }
    }
  }
}

using Common.Domain.Repositories;
using Common.Exceptions;
using Mapster;
using X10sions.Fake.Features.Owner;

namespace CleanOnionExample.Data.Entities.Services;

internal sealed class OwnerService(IOwnerRepository ownerRepository) : IOwnerService {

  public async Task<IEnumerable<GetOwnerQuery>> GetAllAsync(CancellationToken cancellationToken = default) {
    var owners = await ownerRepository.GetListAsync(cancellationToken);
    var ownersDto = owners.Adapt<IEnumerable<GetOwnerQuery>>();
    return ownersDto;
  }

  public async Task<GetOwnerQuery> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    return owner.Adapt<GetOwnerQuery>();
  }

  public async Task<GetOwnerQuery> CreateAsync(UpdateOwnerCommand ownerForCreationDto, CancellationToken cancellationToken = default) {
    var owner = ownerForCreationDto.Adapt<Owner>();
    await ownerRepository.InsertAsync(owner, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return owner.Adapt<GetOwnerQuery>();
  }

  public async Task UpdateAsync(Guid ownerId, UpdateOwnerCommand ownerForUpdateDto, CancellationToken cancellationToken = default) {
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    owner.Name = ownerForUpdateDto.Name;
    owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
    owner.Address = ownerForUpdateDto.Address;
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var owner = await ownerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    await ownerRepository.DeleteByIdAsync(owner.Id, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }
}

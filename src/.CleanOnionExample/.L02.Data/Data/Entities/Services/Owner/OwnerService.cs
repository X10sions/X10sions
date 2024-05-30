namespace CleanOnionExample.Data.Entities.Services;

using Common.Exceptions;
using Common.Features.DummyFakeExamples.Owner;
using Mapster;
using Task = System.Threading.Tasks.Task;

internal sealed class OwnerService : IOwnerService {
  public OwnerService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

  private readonly IRepositoryManager _repositoryManager;

  public async Task<IEnumerable<GetOwnerQuery>> GetAllAsync(CancellationToken cancellationToken = default) {
    var owners = await _repositoryManager.OwnerRepository.GetAllAsync(cancellationToken);
    var ownersDto = owners.Adapt<IEnumerable<GetOwnerQuery>>();
    return ownersDto;
  }

  public async Task<GetOwnerQuery> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    return owner.Adapt<GetOwnerQuery>();
  }

  public async Task<GetOwnerQuery> CreateAsync(UpdateOwnerCommand ownerForCreationDto, CancellationToken cancellationToken = default) {
    var owner = ownerForCreationDto.Adapt<Owner>();
    await _repositoryManager.OwnerRepository.InsertAsync(owner, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return owner.Adapt<GetOwnerQuery>();
  }

  public async Task UpdateAsync(Guid ownerId, UpdateOwnerCommand ownerForUpdateDto, CancellationToken cancellationToken = default) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    owner.Name = ownerForUpdateDto.Name;
    owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
    owner.Address = ownerForUpdateDto.Address;
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default) {
    var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);
    if (owner is null) {
      throw new OwnerNotFoundException(ownerId);
    }
    await _repositoryManager.OwnerRepository.DeleteAsync(owner.Id, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }
}

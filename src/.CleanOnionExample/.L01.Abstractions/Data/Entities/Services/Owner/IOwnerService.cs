namespace CleanOnionExample.Data.Entities.Services;

public interface IOwnerService {
  Task<IEnumerable<GetOwnerQuery>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<GetOwnerQuery> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
  Task<GetOwnerQuery> CreateAsync(UpdateOwnerCommand ownerForCreationDto, CancellationToken cancellationToken = default);
  Task UpdateAsync(Guid ownerId, UpdateOwnerCommand ownerForUpdateDto, CancellationToken cancellationToken = default);
  Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

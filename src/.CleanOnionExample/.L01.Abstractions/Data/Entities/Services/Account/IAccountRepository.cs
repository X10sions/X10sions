using Common.Data.Repositories;

namespace CleanOnionExample.Data.Entities.Services;
public interface IAccountRepository : IRepository<Account, Guid> {
  Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

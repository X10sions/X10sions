using Common.Domain.Repositories;

namespace X10sions.Fake.Features.Account;

public interface IAccountRepository : IRepositoryAsync<Account, Guid> {
  Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

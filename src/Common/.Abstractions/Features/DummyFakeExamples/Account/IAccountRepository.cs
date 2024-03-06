using Common.Data.Repositories;

namespace Common.Features.DummyFakeExamples.Account;

public interface IAccountRepository : IRepository<Account, Guid> {
  Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

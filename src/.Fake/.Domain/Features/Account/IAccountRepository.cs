
using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Account;

public interface IAccountRepository : IGraphRepository<Account>  {//IReadOnlyRepository<Account, Guid>, IWriteOnlyRepository<Account,Guid> {
  Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

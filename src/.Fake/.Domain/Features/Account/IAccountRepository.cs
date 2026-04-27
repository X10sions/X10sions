
using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Account;

public interface IAccountRepository : IGraphRepository<FakeAccount>  {//IReadOnlyRepository<Account, Guid>, IWriteOnlyRepository<Account,Guid> {
  Task<IEnumerable<FakeAccount>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
}

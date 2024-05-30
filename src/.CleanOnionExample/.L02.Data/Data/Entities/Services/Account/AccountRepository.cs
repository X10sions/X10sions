using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using Common.Features.DummyFakeExamples.Account;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class AccountRepository : EntityFrameworkCoreRepositoryBase<RepositoryDbContext, Account, Guid>, IAccountRepository {
  public AccountRepository(RepositoryDbContext dbContext) : base(dbContext) { }

  public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
      await _dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

}

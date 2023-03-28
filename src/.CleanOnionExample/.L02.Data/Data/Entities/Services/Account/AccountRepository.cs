using CleanOnionExample.Data.DbContexts;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class AccountRepository : BaseEntityFrameworkCoreRepository<RepositoryDbContext, Account, Guid>, IAccountRepository {
  public AccountRepository(RepositoryDbContext dbContext) : base(dbContext) { }

  public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
      await _dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

}

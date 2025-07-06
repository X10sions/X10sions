using CleanOnionExample.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using RCommon.Persistence.EFCore.Crud;
using X10sions.Fake.Features.Account;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class AccountRepository : EFCoreRepository<Account, Guid>, IAccountRepository {
  public AccountRepository(RepositoryDbContext dbContext) : base(dbContext) { }

  public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
      await Table.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

}

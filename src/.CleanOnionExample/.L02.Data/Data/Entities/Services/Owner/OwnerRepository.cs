using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class OwnerRepository : EntityFrameworkCoreRepositoryBase<RepositoryDbContext, Owner, Guid>, IOwnerRepository {
  public OwnerRepository(RepositoryDbContext dbContext) : base(dbContext) { }

}

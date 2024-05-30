using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using Common.Features.DummyFakeExamples.Owner;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class OwnerRepository : EntityFrameworkCoreRepositoryBase<RepositoryDbContext, Owner, Guid>, IOwnerRepository {
  public OwnerRepository(RepositoryDbContext dbContext) : base(dbContext) { }

}

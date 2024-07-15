using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using Common.Features.DummyFakeExamples.Owner;
using X10sions.Fake.Features.Owner;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class OwnerRepository : EntityFrameworkCoreRepositoryBase<RepositoryDbContext, Owner, Guid>, IOwnerRepository {
  public OwnerRepository(RepositoryDbContext dbContext) : base(dbContext) { }

}

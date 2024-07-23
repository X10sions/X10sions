using CleanOnionExample.Data.DbContexts;
using RCommon.Persistence.EFCore.Crud;
using X10sions.Fake.Features.Owner;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class OwnerRepository : EFCoreRepository<Owner, Guid>, IOwnerRepository {
  public OwnerRepository(RepositoryDbContext dbContext) : base(dbContext) { }

}

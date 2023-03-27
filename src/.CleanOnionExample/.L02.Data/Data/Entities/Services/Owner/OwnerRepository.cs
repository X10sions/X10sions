namespace CleanOnionExample.Data.Entities.Services;
internal sealed class OwnerRepository : BaseEntityFrameworkCoreRepository<RepositoryDbContext, Owner, Guid>, IOwnerRepository {
  public OwnerRepository(RepositoryDbContext dbContext) : base(dbContext) { }

}

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : BaseEntityFrameworkCoreRepository<ApplicationDbContext, Person, int>, IPersonRepository {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}

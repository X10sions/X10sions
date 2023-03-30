using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : EntityFrameworkCoreRepositoryBase<ApplicationDbContext, Person, int> {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}

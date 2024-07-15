using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using X10sions.Fake.Features.Person;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : EntityFrameworkCoreRepositoryBase<ApplicationDbContext, Person, int>, IPersonRepository {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }

  public IQueryable<Person> Person => Queryable;
}

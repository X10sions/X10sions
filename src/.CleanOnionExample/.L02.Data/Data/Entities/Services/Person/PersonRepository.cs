using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using Common.Features.DummyFakeExamples.Person;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : EntityFrameworkCoreRepositoryBase<ApplicationDbContext, Person, int>, IPersonRepository {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }

  public IQueryable<Person> Person => Queryable;
}

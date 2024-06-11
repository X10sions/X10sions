using CleanOnionExample.Data.DbContexts;
using Common.Data.Repositories;
using Common.Features.DummyFakeExamples.Person;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : EntityFrameworkCoreRepositoryBase<ApplicationDbContext, Person, int> {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}

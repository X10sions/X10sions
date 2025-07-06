using CleanOnionExample.Data.DbContexts;
using RCommon.Persistence.EFCore.Crud;
using X10sions.Fake.Features.Person;

namespace CleanOnionExample.Data.Entities.Services;
internal sealed class PersonRepository : EFCoreRepository<Person, int>, IPersonRepository {
  public PersonRepository(ApplicationDbContext dbContext) : base(dbContext) { }

  public IQueryable<Person> Person =>  Table;
}

using RCommon.Persistence.Crud;

namespace X10sions.Fake.Features.Person;

public interface IPersonRepository : IReadOnlyRepository<FakePerson, int>, IWriteOnlyRepository<FakePerson, int> {
  //IQueryable<Person> Person { get; }
  //Task<List<Product>> GetListAsync();
  //Task<Product> GetByIdAsync(int productId);
  //Task<int> InsertAsync(Product product);
  //Task UpdateAsync(Product product);
  //Task DeleteAsync(Product product);
}
namespace X10sions.Fake.Features.Product;

public interface IProductRepository {
  IQueryable<FakeProduct> Products { get; }
  Task<List<FakeProduct>> GetListAsync();
  Task<FakeProduct> GetByIdAsync(int productId);
  Task<int> InsertAsync(FakeProduct product);
  Task UpdateAsync(FakeProduct product);
  Task DeleteAsync(FakeProduct product);
}
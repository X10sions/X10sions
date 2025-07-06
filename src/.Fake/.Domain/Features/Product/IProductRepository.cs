namespace X10sions.Fake.Features.Product;

public interface IProductRepository {
  IQueryable<Product> Products { get; }
  Task<List<Product>> GetListAsync();
  Task<Product> GetByIdAsync(int productId);
  Task<int> InsertAsync(Product product);
  Task UpdateAsync(Product product);
  Task DeleteAsync(Product product);
}
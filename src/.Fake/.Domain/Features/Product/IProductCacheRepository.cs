namespace X10sions.Fake.Features.Product;

public interface IProductCacheRepository {
  Task<List<Product>> GetCachedListAsync();
  Task<Product> GetByIdAsync(int brandId);
}
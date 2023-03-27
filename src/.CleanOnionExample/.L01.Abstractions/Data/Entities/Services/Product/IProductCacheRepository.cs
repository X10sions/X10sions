namespace CleanOnionExample.Data.Entities.Services;

public interface IProductCacheRepository {
  Task<List<Product>> GetCachedListAsync();
  Task<Product> GetByIdAsync(int brandId);
}
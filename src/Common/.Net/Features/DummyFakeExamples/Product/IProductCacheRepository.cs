namespace Common.Features.DummyFakeExamples.Product;

public interface IProductCacheRepository {
  Task<List<Product>> GetCachedListAsync();
  Task<Product> GetByIdAsync(int brandId);
}
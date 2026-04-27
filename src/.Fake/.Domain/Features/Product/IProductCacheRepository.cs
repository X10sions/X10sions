namespace X10sions.Fake.Features.Product;

public interface IProductCacheRepository {
  Task<List<FakeProduct>> GetCachedListAsync();
  Task<FakeProduct> GetByIdAsync(int brandId);
}
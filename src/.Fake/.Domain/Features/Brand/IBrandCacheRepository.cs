namespace X10sions.Fake.Features.Brand;

public interface IBrandCacheRepository {
  Task<List<Brand>> GetCachedListAsync();
  Task<Brand> GetByIdAsync(int brandId);
}

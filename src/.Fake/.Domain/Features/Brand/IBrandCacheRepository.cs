namespace X10sions.Fake.Features.Brand;

public interface IBrandCacheRepository {
  Task<ICollection<Brand>> GetCachedListAsync();
  Task<Brand> GetByIdAsync(int brandId);
}

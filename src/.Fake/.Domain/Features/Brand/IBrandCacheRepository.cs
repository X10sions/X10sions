namespace X10sions.Fake.Features.Brand;

public interface IBrandCacheRepository {
  Task<ICollection<FakeBrand>> GetCachedListAsync();
  Task<FakeBrand> GetByIdAsync(int brandId);
}

namespace Common.Features.DummyFakeExamples.Brand;

public interface IBrandCacheRepository {
  Task<List<Brand>> GetCachedListAsync();
  Task<Brand> GetByIdAsync(int brandId);
}

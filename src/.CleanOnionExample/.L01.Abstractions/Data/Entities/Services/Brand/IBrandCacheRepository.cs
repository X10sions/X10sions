namespace CleanOnionExample.Data.Entities.Services;

public interface IBrandCacheRepository {
  Task<List<Brand>> GetCachedListAsync();
  Task<Brand> GetByIdAsync(int brandId);
}

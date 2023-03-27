using CleanOnionExample.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;


namespace CleanOnionExample.Data.Entities.Services;

public class BrandRepository : IBrandRepository {
  private readonly IRepositoryAsync<Brand> _repository;
  private readonly IDistributedCache _distributedCache;

  public BrandRepository(IDistributedCache distributedCache, IRepositoryAsync<Brand> repository) {
    _distributedCache = distributedCache;
    _repository = repository;
  }

  public IQueryable<Brand> Brands => _repository.Entities;

  public async System.Threading.Tasks.Task DeleteAsync(Brand brand) {
    await _repository.DeleteAsync(brand);
    await _distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    await _distributedCache.RemoveAsync(BrandCacheKeys.GetKey(brand.Id));
  }

  public async Task<Brand> GetByIdAsync(int brandId) {
    return await _repository.Entities.Where(p => p.Id == brandId).FirstOrDefaultAsync();
  }

  public async Task<List<Brand>> GetListAsync() {
    return await _repository.Entities.ToListAsync();
  }

  public async Task<int> InsertAsync(Brand brand) {
    await _repository.AddAsync(brand);
    await _distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    return brand.Id;
  }

  public async System.Threading.Tasks.Task UpdateAsync(Brand brand) {
    await _repository.UpdateAsync(brand);
    await _distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    await _distributedCache.RemoveAsync(BrandCacheKeys.GetKey(brand.Id));
  }
}

public class BrandCacheRepository : IBrandCacheRepository {
  private readonly IDistributedCache _distributedCache;
  private readonly IBrandRepository _brandRepository;

  public BrandCacheRepository(IDistributedCache distributedCache, IBrandRepository brandRepository) {
    _distributedCache = distributedCache;
    _brandRepository = brandRepository;
  }

  public async Task<Brand> GetByIdAsync(int brandId) {
    var cacheKey = BrandCacheKeys.GetKey(brandId);
    var brand = await _distributedCache.GetAsync<Brand>(cacheKey);
    if (brand == null) {
      brand = await _brandRepository.GetByIdAsync(brandId);
      Throw.IfNull(brand, "Brand", "No Brand Found");
      await _distributedCache.SetAsync(cacheKey, brand);
    }
    return brand;
  }

  public async Task<List<Brand>> GetCachedListAsync() {
    var cacheKey = BrandCacheKeys.ListKey;
    var brandList = await _distributedCache.GetAsync<List<Brand>>(cacheKey);
    if (brandList == null) {
      brandList = await _brandRepository.GetListAsync();
      await _distributedCache.SetAsync(cacheKey, brandList);
    }
    return brandList;
  }
}
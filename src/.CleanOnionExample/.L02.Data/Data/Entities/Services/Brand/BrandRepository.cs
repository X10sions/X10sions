using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RCommon.Persistence.Crud;
using X10sions.Fake.Features.Brand;
using static X10sions.Fake.Constants.CacheKeys;

namespace CleanOnionExample.Data.Entities.Services;

public class BrandRepository(IDistributedCache distributedCache, ILinqRepository<Brand> repository) : IBrandRepository {
  public IQueryable<Brand> Brands => repository.Queryable;

  public async Task DeleteAsync(Brand brand) {
    await repository.DeleteAsync(brand);
    await distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    await distributedCache.RemoveAsync(BrandCacheKeys.GetKey(brand.Id));
  }

  public async Task<Brand> GetByIdAsync(int brandId) {
    return await repository.Queryable.Where(p => p.Id == brandId).FirstOrDefaultAsync();
  }

  public async Task<ICollection<Brand>> GetListAsync() =>await repository.GetAllAsync(x=> true);

  public async Task<int> InsertAsync(Brand brand) {
    await repository.InsertAsync(brand);
    await distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    return brand.Id;
  }

  public async Task UpdateAsync(Brand brand) {
    await repository.UpdateAsync(brand);
    await distributedCache.RemoveAsync(BrandCacheKeys.ListKey);
    await distributedCache.RemoveAsync(BrandCacheKeys.GetKey(brand.Id));
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

  public async Task<ICollection<Brand>> GetCachedListAsync() {
    var cacheKey = BrandCacheKeys.ListKey;
    var brandList = await _distributedCache.GetAsync<ICollection<Brand>>(cacheKey);
    if (brandList == null) {
      brandList = await _brandRepository.GetListAsync();
      await _distributedCache.SetAsync(cacheKey, brandList);
    }
    return brandList;
  }
}
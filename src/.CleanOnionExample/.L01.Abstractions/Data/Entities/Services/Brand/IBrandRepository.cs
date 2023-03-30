namespace CleanOnionExample.Data.Entities.Services;

public interface IBrandRepository {
  IQueryable<Brand> Brands { get; }
  Task<List<Brand>> GetListAsync();
  Task<Brand> GetByIdAsync(int brandId);
  Task<int> InsertAsync(Brand brand);
  Task UpdateAsync(Brand brand);
  Task DeleteAsync(Brand brand);
}

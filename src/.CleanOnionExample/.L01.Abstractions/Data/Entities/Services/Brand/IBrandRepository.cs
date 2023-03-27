namespace CleanOnionExample.Data.Entities.Services;

public interface IBrandRepository {
  IQueryable<Brand> Brands { get; }
  System.Threading.Tasks.Task<List<Brand>> GetListAsync();
  System.Threading.Tasks.Task<Brand> GetByIdAsync(int brandId);
  System.Threading.Tasks.Task<int> InsertAsync(Brand brand);
  System.Threading.Tasks.Task UpdateAsync(Brand brand);
  System.Threading.Tasks.Task DeleteAsync(Brand brand);
}

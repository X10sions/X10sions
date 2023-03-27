namespace CleanOnionExample.Data.Entities.Services;

public interface IProductRepository {
  IQueryable<Product> Products { get; }
  Task<List<Product>> GetListAsync();
  Task<Product> GetByIdAsync(int productId);
  Task<int> InsertAsync(Product product);
  System.Threading.Tasks.Task UpdateAsync(Product product);
  System.Threading.Tasks.Task DeleteAsync(Product product);
}
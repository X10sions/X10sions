namespace X10sions.Fake.Features.Brand;

public interface IBrandRepository {
  IQueryable<FakeBrand> Brands { get; }
  Task<ICollection<FakeBrand>> GetListAsync();
  Task<FakeBrand> GetByIdAsync(int brandId);
  Task<int> InsertAsync(FakeBrand brand);
  Task UpdateAsync(FakeBrand brand);
  Task DeleteAsync(FakeBrand brand);
}

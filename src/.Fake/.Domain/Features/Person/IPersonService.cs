namespace X10sions.Fake.Features.Person;

  public interface IPersonService {
  Task<GetPersonQuery> InsertAsync(UpdatePersonCommand person, CancellationToken cancellationToken = default);
  Task UpdateAsync(int id, UpdatePersonCommand person, CancellationToken cancellationToken = default);
  Task DeleteAsync(int id, CancellationToken cancellationToken = default);
  Task<IEnumerable<GetPersonQuery>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<GetPersonQuery> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
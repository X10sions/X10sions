using Mapster;
using X10sions.Fake.Features.Person;

namespace CleanOnionExample.Data.Entities.Services;
public class PersonService : IPersonService {
  public PersonService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

  private readonly IRepositoryManager _repositoryManager;


  //public PersonService(IBaseRepository<Person, int> person) {
  //  _person = person;
  //}

  //private readonly IBaseRepository<Person, int> _person;

  public async Task<GetPersonQuery> InsertAsync( UpdatePersonCommand personForCreationDto, CancellationToken cancellationToken = default) {
    var person = personForCreationDto.Adapt<Person>();
    await _repositoryManager.PersonRepository .InsertAsync(person, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return person.Adapt<GetPersonQuery>();
  }

  public async System.Threading.Tasks.Task UpdateAsync(int id, UpdatePersonCommand person, CancellationToken cancellationToken = default) {
    var dbRecord = await _repositoryManager.PersonRepository.GetByIdAsync(id, cancellationToken);
    if (dbRecord is null) {
      throw new Exception($"Not found id: {id}");
    }
    dbRecord.FirstName = person.FirstName;
    dbRecord.LastName = person.LastName;
    dbRecord.Email = person.Email;
    dbRecord.MobileNo = person.MobileNo;
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async System.Threading.Tasks.Task DeleteAsync(int id, CancellationToken cancellationToken = default) {
    var person = await _repositoryManager.PersonRepository.GetByIdAsync(id, cancellationToken);
    if (person is null) {
      throw new Exception($"Not found id: {id}");
    }
    await _repositoryManager.PersonRepository.DeleteAsync(id, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task<IEnumerable<GetPersonQuery>> GetAllAsync(CancellationToken cancellationToken = default) {
    var list = await _repositoryManager.PersonRepository.GetAllAsync(cancellationToken);
    return list.Adapt<IEnumerable<GetPersonQuery>>();
  }

  public async Task<GetPersonQuery> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
    var person = await _repositoryManager.PersonRepository.GetByIdAsync(id, cancellationToken);
    if (person is null) {
      throw new Exception($"Not found id: {id}");
    }
    var dto = person.Adapt<GetPersonQuery>();
    return dto;
  }

}

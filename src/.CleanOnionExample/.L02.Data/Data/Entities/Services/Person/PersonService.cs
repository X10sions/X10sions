namespace CleanOnionExample.Data.Entities.Services;
public class PersonService : IPersonService {
  public PersonService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

  private readonly IRepositoryManager _repositoryManager;


  //public PersonService(IBaseRepository<Person, int> person) {
  //  _person = person;
  //}

  //private readonly IBaseRepository<Person, int> _person;

  public async Task<Person.GetQuery> InsertAsync(Person.UpdateCommand personForCreationDto, CancellationToken cancellationToken = default) {
    var person = personForCreationDto.Adapt<Person>();
    await _repositoryManager.PersonRepository.InsertAsync(person, cancellationToken);
    await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return person.Adapt<Person.GetQuery>();
  }

  public async System.Threading.Tasks.Task UpdateAsync(int id, Person.UpdateCommand person, CancellationToken cancellationToken = default) {
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

  public async Task<IEnumerable<Person.GetQuery>> GetAllAsync(CancellationToken cancellationToken = default) {
    var list = await _repositoryManager.PersonRepository.GetAllAsync(cancellationToken);
    return list.Adapt<IEnumerable<Person.GetQuery>>();
  }

  public async Task<Person.GetQuery> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
    var person = await _repositoryManager.PersonRepository.GetByIdAsync(id, cancellationToken);
    if (person is null) {
      throw new Exception($"Not found id: {id}");
    }
    var dto = person.Adapt<Person.GetQuery>();
    return dto;
  }

}

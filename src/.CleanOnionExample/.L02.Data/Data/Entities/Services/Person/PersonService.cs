using Common.Domain.Repositories;
using Mapster;
using X10sions.Fake.Features.Person;

namespace CleanOnionExample.Data.Entities.Services;
public class PersonService(IPersonRepository personRepository) : IPersonService {
  public async Task<GetPersonQuery> InsertAsync( UpdatePersonCommand personForCreationDto, CancellationToken cancellationToken = default) {
    var person = personForCreationDto.Adapt<Person>();
    await personRepository.InsertAsync(person, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    return person.Adapt<GetPersonQuery>();
  }

  public async Task UpdateAsync(int id, UpdatePersonCommand person, CancellationToken cancellationToken = default) {
    var dbRecord = await personRepository.GetByIdAsync(id, cancellationToken);
    if (dbRecord is null) {
      throw new Exception($"Not found id: {id}");
    }
    dbRecord.FirstName = person.FirstName;
    dbRecord.LastName = person.LastName;
    dbRecord.Email = person.Email;
    dbRecord.MobileNo = person.MobileNo;
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(int id, CancellationToken cancellationToken = default) {
    var person = await personRepository.GetByIdAsync(id, cancellationToken);
    if (person is null) {
      throw new Exception($"Not found id: {id}");
    }
    await personRepository.DeleteByIdAsync(id, cancellationToken);
    //await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
  }

  public async Task<IEnumerable<GetPersonQuery>> GetListAsync(CancellationToken cancellationToken = default) {
    var list = await personRepository.GetListAsync(cancellationToken);
    return list.Adapt<IEnumerable<GetPersonQuery>>();
  }

  public async Task<GetPersonQuery> GetByIdAsync(int id, CancellationToken cancellationToken = default) {
    var person = await personRepository.GetByIdAsync(id, cancellationToken);
    if (person is null) {
      throw new Exception($"Not found id: {id}");
    }
    var dto = person.Adapt<GetPersonQuery>();
    return dto;
  }

}

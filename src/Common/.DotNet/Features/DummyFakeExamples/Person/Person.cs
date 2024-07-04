using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Features.DummyFakeExamples.Person;

[Table("Person", Schema = "dbo")]
public partial class Person : EntityBase<int> {
  [Required][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
  [Required] public string FirstName { get; set; }
  [Required] public string LastName { get; set; }
  [Required] public string Email { get; set; }
  [Required] public string MobileNo { get; set; }
}

public partial class Person {

  public class GetQuery {
    public int Id { get; set; }
    [Display(Name = "First Name")] public string FirstName { get; set; }
    [Display(Name = "Last Name")] public string LastName { get; set; }
    [Display(Name = "Email")] public string Email { get; set; }
    [Display(Name = "Mobile Number")] public string MobileNo { get; set; }
  }
  public class UpdateCommand {
    public int Id { get; set; }
    [Required][Display(Name = "First Name")] public string FirstName { get; set; }
    [Required][Display(Name = "Last Name")] public string LastName { get; set; }
    [Required][Display(Name = "Email")] public string Email { get; set; }
    [Required][Display(Name = "Mobile Number")] public string MobileNo { get; set; }
  }
}

public interface IPersonService {
  Task<Person.GetQuery> InsertAsync(Person.UpdateCommand person, CancellationToken cancellationToken = default);
  Task UpdateAsync(int id, Person.UpdateCommand person, CancellationToken cancellationToken = default);
  Task DeleteAsync(int id, CancellationToken cancellationToken = default);
  Task<IEnumerable<Person.GetQuery>> GetAllAsync(CancellationToken cancellationToken = default);
  Task<Person.GetQuery> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

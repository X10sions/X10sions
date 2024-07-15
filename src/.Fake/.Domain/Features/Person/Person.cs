using Common.Domain;
using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Person;

[Table("Person")]
public class Person : EntityBase<int> {
  private Person(string firstName, Option<string> lastName) {
    (FirstName, LastNames) = (firstName, lastName);
  }

  public static Person Create(string firstName, string lastName) => new(firstName, Option<string>.Some(lastName));
  public static Person Create(string firstName) => new(firstName, Option<string>.None);

  [Required][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
  [Required] public string FirstName { get; set; }
  [Required] public string LastName { get; set; }
  [Required] public string Email { get; set; }
  [Required] public string MobileNo { get; set; }
  public Option<string> LastNames { get; }

  public static string GetLabel(Person person) => person.LastNames.Map(lastName => $"{person.FirstName} {lastName}").Reduce(person.FirstName);

  public static class Examples {
    public static Person Mann = Create("Thmomas", "Mann");
    public static Person Asristotle = Create("Asristotle");
  }
}

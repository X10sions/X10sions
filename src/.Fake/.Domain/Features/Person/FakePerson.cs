using Common.Domain;
using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X10sions.Fake.Features.Project.Item;

namespace X10sions.Fake.Features.Person;


[Table("FakePerson")]
public class FakePerson : EntityBase<int> {
  private FakePerson(string firstName, Option<string> lastName) {
    (PreferredFirstName, LastNames) = (firstName, lastName);
  }

  public static FakePerson Create(string firstName, string lastName) => new(firstName, Option<string>.Some(lastName));
  public static FakePerson Create(string firstName) => new(firstName, Option<string>.None);

  [Column, Required][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
  public string FirstName => PreferredFirstName ?? ActualFirstName;
  [Column, Required] public string? PreferredFirstName { get; set; }
  [Column] public string ActualFirstName { get; set; } = string.Empty;
  public string FullName => FirstName + " " + LastName;
  [Column, Required] public string LastName { get; set; } = string.Empty;
  [Required] public string Email { get; set; }
  [Required] public string MobileNo { get; set; }
  public Option<string> LastNames { get; }

  public static string GetLabel(FakePerson person) => person.LastNames.Map(lastName => $"{person.FirstName} {lastName}").Reduce(person.FirstName);

  public static class Examples {
    public static FakePerson Mann = Create("Thmomas", "Mann");
    public static FakePerson Asristotle = Create("Asristotle");
  }

  [Column] public int? FatherId { get; set; }
  [Column] public int? MotherId { get; set; }
  //[Column] public DateOnly? BirthDate { get; set; }
  //[Column] public TimeOnly? BirthTime { get; set; }
  [Column] public DateTime? BirthDateTime { get; set; }
  //[Column] public DateOnly? DeathDate { get; set; }
  //[Column] public TimeOnly? DeathTime{ get; set; }
  [Column] public DateTime? DeathDateTime { get; set; }

  public int? Age => BirthDateTime.GetWholeYearsBetween(DeathDateTime ?? DateTime.UtcNow);

  public FakePerson? Father { get; set; }
  public FakePerson? Mother { get; set; }
  public IEnumerable<FakeProjectItem> AssignedProjectItems { get; set; } = null!;
  public IEnumerable<FakeProjectItem> CreatedProjectItems { get; set; } = null!;
  public IEnumerable<FakePerson> FatherOf { get; set; } = null!;
  public IEnumerable<FakePerson> MotherOf { get; set; } = null!;
  public IEnumerable<FakePerson> Children => FatherOf.Union(MotherOf);
}

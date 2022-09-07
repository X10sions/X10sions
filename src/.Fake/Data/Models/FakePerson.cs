using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {

  [Table("FakePerson")]
  public class FakePerson {
    [Column] public int Id { get; set; }
    [Column] public int? FatherId { get; set; }
    [Column] public int? MotherId { get; set; }
    [Column] public string ActualFirstName { get; set; } = string.Empty;
    [Column] public string? PreferredFirstName { get; set; }
    [Column] public string LastName { get; set; } = string.Empty;
    //[Column] public DateOnly? BirthDate { get; set; }
    //[Column] public TimeOnly? BirthTime { get; set; }
    [Column] public DateTime? BirthDateTime { get; set; }
    //[Column] public DateOnly? DeathDate { get; set; }
    //[Column] public TimeOnly? DeathTime{ get; set; }
    [Column] public DateTime? DeathDateTime { get; set; }

    public int? Age => BirthDateTime.GetWholeYearsBetween(DeathDateTime ?? DateTime.Now);
    public string FirstName => PreferredFirstName ?? ActualFirstName;
    public string FullName => FirstName + " " + LastName;

    public FakePerson? Father { get; set; }
    public FakePerson? Mother { get; set; }
    public IEnumerable<FakeProjectItem> AssignedProjectItems { get; set; } = null!;
    public IEnumerable<FakeProjectItem> CreatedProjectItems { get; set; } = null!;
    public IEnumerable<FakePerson> FatherOf { get; set; } = null!;
    public IEnumerable<FakePerson> MotherOf { get; set; } = null!;
    public IEnumerable<FakePerson> Children => FatherOf.Union(MotherOf);
  }


}

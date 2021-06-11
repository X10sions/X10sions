using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {

  [Table("Person", IsColumnAttributeRequired = false)]
  [Column("FirstName", "Name.FirstName")]
  [Column("MiddleName", "Name.MiddleName")]
  [Column("LastName", "Name.LastName")]
  public class ComplexPerson : IPerson {

    [Identity] [SequenceName(ProviderName.Firebird, "PersonID")] [Column("PersonID", IsPrimaryKey = true)] public int ID { get; set; }
    public Gender Gender { get; set; }
    public FullName Name { get; set; } = null!;

    [NotColumn] int IPerson.ID { get => ID; set => ID = value; }
    [NotColumn] Gender IPerson.Gender { get => Gender; set => Gender = value; }
    [NotColumn] string IPerson.FirstName { get => Name.FirstName; set => Name.FirstName = value; }
    [NotColumn] string? IPerson.MiddleName { get => Name.MiddleName; set => Name.MiddleName = value; }
    [NotColumn] string IPerson.LastName { get => Name.LastName; set => Name.LastName = value; }
  }
}

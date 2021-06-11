using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  [Table("Person", IsColumnAttributeRequired = false)]
  [Column("FirstName", "Name.FirstName")]
  [Column("MiddleName", "Name.MiddleName")]
  [Column("LastName", "Name.LastName")]
  public class ComplexPerson2 {

    [Identity]
    [SequenceName(ProviderName.Firebird, "PersonID")]
    [Column("PersonID", IsPrimaryKey = true)]
    public int ID { get; set; }
    public Gender Gender { get; set; }
    public FullName Name { get; set; } = null!;
  }
}

namespace LinqToDB.Tests.Model {
  public class ComplexPerson3 {
    public int ID { get; set; }
    public Gender Gender { get; set; }
    public FullName Name { get; set; } = null!;
  }
}

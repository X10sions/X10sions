using System.Linq.Expressions;

namespace X10sions.ERP.Domain.Models;

public record Person(int Id, string FirstName, string LastName, string? PreferredFirstName = null, DateTime? DateOfBirth = null, DateTime? DateOfDeath = null) {

  public string FullName => Expressions.FullNameFunc(this);
  public string PreferredName => Expressions.PreferredNameFunc(this);


  public int? Age => Expressions.AgeFunc(this);

  private static class Expressions {
    // 1. Building Blocks (Base logic)
    static readonly Expression<Func<Person, DateTime>> AgeMaxDate = x => x.DateOfDeath ?? DateTime.Today;
    // 2. Base Compiled Delegate (Needed for the Age expression below)
    static readonly Func<Person, DateTime> AgeMaxDateFunc = AgeMaxDate.Compile();
    // 3. Composite Expressions (Depend on building blocks)
    static readonly Expression<Func<Person, string>> FirstLastName = x => x.FirstName + " " + x.LastName;
    static readonly Expression<Func<Person, string>> PreferredName = x => $"{(string.IsNullOrWhiteSpace(x.PreferredFirstName) ? x.FirstName : x.PreferredFirstName)} {x.LastName}";
    static readonly Expression<Func<Person, int?>> Age = x => x.DateOfBirth == null ? null : AgeMaxDateFunc(x).Year - x.DateOfBirth.Value.Year - (AgeMaxDateFunc(x).DayOfYear < x.DateOfBirth.Value.DayOfYear ? 1 : 0);
    // 4. Final Public Delegates
    public static readonly Func<Person, string> FullNameFunc = FirstLastName.Compile();
    public static readonly Func<Person, string> PreferredNameFunc = PreferredName.Compile();
    public static readonly Func<Person, int?> AgeFunc = Age.Compile();
  }
}

using Common.Domain;

namespace X10sions.Fake.Domain;

public class Person {
  //Zoran Horvat
  private Person(string firstName, Option<string> lastName) {
    (FirstName, LastName) = (firstName, lastName);
  }

  public static Person Create(string firstName, string lastName) => new(firstName, Option<string>.Some(lastName));
  public static Person Create(string firstName) => new(firstName, Option<string>.None);

  public string FirstName { get; }
  public Option<string> LastName { get; }

  public static string GetLabel(Person person) => person.LastName.Map(lastName => $"{person.FirstName} {lastName}").Reduce(person.FirstName);

  public static class Examples {
    public static Person Mann = Create("Thmomas", "Mann");
    public static Person Asristotle = Create("Asristotle");
  }
}




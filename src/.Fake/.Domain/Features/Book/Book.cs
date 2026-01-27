using Common.Domain;

namespace X10sions.Fake.Features.Book;

public class FakeBook {
  public string Title { get; }
  public Option<Person.FakePerson> Author { get; }

  private FakeBook(string title, Option<Person.FakePerson> author) => (Title, Author) = (title, author);

  public static string GetLabel(FakeBook book) => book.Author.Map(Person.FakePerson.GetLabel).Map(author => $"{book.Title} by {author}").Reduce(book.Title);

  public static FakeBook Create(string title, Person.FakePerson author) => new(title, Option<Person.FakePerson>.Some(author));
  public static FakeBook Create(string title) => new(title, Option<Person.FakePerson>.None);


  public static class Examples {
    public static FakeBook Faustus = Create("Doctor Faustus", Person.FakePerson.Examples.Mann);
    public static FakeBook Rhetoric = Create("Rhetoric", Person.FakePerson.Examples.Asristotle);
    public static FakeBook Nighhts = Create("One Thousand and one Nights");
  }

}




using Common.Domain;

namespace X10sions.Fake.Features.Book;

public class Book {
  public string Title { get; }
  public Option<Person.Person> Author { get; }

  private Book(string title, Option<Person.Person> author) => (Title, Author) = (title, author);

  public static string GetLabel(Book book) => book.Author.Map(Person.Person.GetLabel).Map(author => $"{book.Title} by {author}").Reduce(book.Title);

  public static Book Create(string title, Person.Person author) => new(title, Option<Person.Person>.Some(author));
  public static Book Create(string title) => new(title, Option<Person.Person>.None);


  public static class Examples {
    public static Book Faustus = Create("Doctor Faustus", Person.Person.Examples.Mann);
    public static Book Rhetoric = Create("Rhetoric", Person.Person.Examples.Asristotle);
    public static Book Nighhts = Create("One Thousand and one Nights");
  }

}




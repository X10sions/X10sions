using Common.Domain;

namespace X10sions.Fake.Domain;

public class Book {
  //Zoran Horvat
  public string Title { get; }
  public Option<Person> Author { get; }

  private Book(string title, Option<Person> author) => (Title, Author) = (title, author);

  public static string GetLabel(Book book) => book.Author.Map(Person.GetLabel).Map(author => $"{book.Title} by {author}").Reduce(book.Title);

  public static Book Create(string title, Person author) => new(title, Option<Person>.Some(author));
  public static Book Create(string title) => new(title, Option<Person>.None);


  public static class Examples {
    public static Book Faustus = Create("Doctor Faustus", Person.Examples.Mann);
    public static Book Rhetoric = Create("Rhetoric", Person.Examples.Asristotle);
    public static Book Nighhts = Create("One Thousand and one Nights");
  }

}




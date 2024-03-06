using Xunit;

namespace Common {
  public class Example_ReplacingMemberDataUsingTheoryData {
    public bool IsAboveFourteen(Person person) => person.Age > 14;

    public class Person {
      public string Name { get; set; }
      public int Age { get; set; }
    }

    public static TheoryData<Person> PersonTheoryData => new TheoryData<Person> {
      new Person {Name = "Tribbiani", Age = 56},
      new Person {Name = "Gotti", Age = 16},
      new Person {Name = "Sopranos", Age = 15},
      new Person {Name = "Corleone", Age = 27},
      new Person {Name = "Mancini", Age = 79},
      new Person {Name = "Vivaldi", Age = 16},
      new Person {Name = "Serpico", Age = 19},
      new Person {Name = "Salieri", Age = 20}
    };

    [Theory, MemberData(nameof(PersonTheoryData), MemberType = typeof(Example_ReplacingMemberDataUsingTheoryData))]
    public void AllPersons_AreAbove14_WithTheoryData_FromDataGenerator(Person a) => Assert.True(IsAboveFourteen(a));
  }


}
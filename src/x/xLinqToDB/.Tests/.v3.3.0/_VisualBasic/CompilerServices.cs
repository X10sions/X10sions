using Tests.Model;
using System.Collections.Generic;
using System.Linq;

namespace VisualBasic {
  public static class CompilerServices {
    public static IEnumerable<Person> CompareString(ITestDataContext db) {
      return from p in db.Person
             where p.FirstName == "John"
             select p;
    }
  }

}

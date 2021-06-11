using IQToolkit.Data.Ado;
using IQToolkit.Data.Mapping;
using IQToolkit.Data.SQLite;

namespace Test {
  public class Program {
    public static void Main(string[] args) => new TestRunner(args, System.Reflection.Assembly.GetEntryAssembly()).RunTests();

    private static DbEntityProvider CreateNorthwindProvider() => new SQLiteQueryProvider("Northwind.db3", new AttributeMapping(typeof(Test.NorthwindWithAttributes)));

    public class NorthwindMappingTests : Test.NorthwindMappingTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider();
    }

    public class NorthwindTranslationTests : Test.NorthwindTranslationTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider();
    }

    public class NorthwindExecutionTests : Test.NorthwindExecutionTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider();
    }

    public class NorthwindCUDTests : Test.NorthwindCUDTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider();
    }
  }
}
using IQToolkit.Data.Ado;
using IQToolkit.Data.Mapping;
using IQToolkit.Data.SqlClient;
using System;

namespace Test {
  public static class Program {
    public static void Main(string[] args) => new TestRunner(args, System.Reflection.Assembly.GetEntryAssembly()).RunTests();

    private static DbEntityProvider CreateNorthwindProvider(Type contextType = null) => new SqlQueryProvider("Northwind.mdf", new AttributeMapping(contextType ?? typeof(Test.NorthwindWithAttributes)));

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

    public class MultiTableTests : Test.MultiTableTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider(typeof(Test.MultiTableContext));
    }

#if !DEBUG
    public class NorthwindPerfTests : Test.NorthwindPerfTests {
      protected override DbEntityProvider CreateProvider() => CreateNorthwindProvider();
    }
#endif
  }
}

//using System;
//using System.Data.Odbc;
//using Xunit;

//namespace LinqToDB.Tests {
//  public class zDatabaseFixture : IDisposable {
//    public zDatabaseFixture() {
//      Odbc = new OdbcConnection("MyConnectionString");
//      Odbc.Open();
//    }

//    public void Dispose() {
//      Odbc.Close();
//      Odbc.Dispose();
//    }

//    public OdbcConnection Odbc { get; private set; }
//  }

//  [CollectionDefinition("Database collection")]
//  public class DatabaseCollection : ICollectionFixture<zDatabaseFixture> {
//    // This class has no code, and is never created. Its purpose is simply
//    // to be the place to apply [CollectionDefinition] and all the
//    // ICollectionFixture<> interfaces.
//  }

//}

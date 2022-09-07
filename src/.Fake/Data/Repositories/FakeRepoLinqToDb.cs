using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System.Data;
using System.Data.Common;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoLinqToDb : FakeRepo {
    public FakeRepoLinqToDb(  DbConnection db) : base(db) {
      DataConnection = new DataConnection(db.GetDataProvider(), db);
   }


    public DataConnection DataConnection { get; }
    public override void CreateTable<T>() =>  DataConnection.CreateTable<T>();
    public override void DropTable<T>() => DataConnection.DropTable<T>();

  }

  public static class LinqToDBExtensions {

    public static IDataProvider GetDataProvider(this IDbConnection db) {
    }

  }

}

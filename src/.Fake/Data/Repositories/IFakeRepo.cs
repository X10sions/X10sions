using System.Data;

namespace X10sions.Fake.Data.Repositories {
  public interface IFakeRepo {
    IDbConnection DbConnection { get; }
    void DropTable<T>();
    void CreateTable<T>();
  }

  public static class IFakeRepoExtensions {
  }


  public abstract class FakeRepo : IFakeRepo {
    public FakeRepo(IDbConnection db) {
      DbConnection = db;
    }

    public IDbConnection DbConnection { get; }
    public abstract void DropTable<T>();
    public abstract void CreateTable<T>();

  }

}

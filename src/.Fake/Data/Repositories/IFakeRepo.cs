using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public interface IFakeRepo {
    IDbConnection DbConnection { get; }
    void DropTable<T>();
    void CreateTable<T>();
  }

  public static class IFakeRepoExtensions { }

  public abstract class FakeRepo : IFakeRepo {
    public FakeRepo(DbConnection dbConnection) {
      DbConnection = dbConnection;
    }

    public FakeRepo(ConnectionStringName name, IConfiguration configuration) : this(name.GetDbConnection(configuration) ?? throw new ArgumentNullException(nameof(DbConnection))) { }

    public IDbConnection DbConnection { get; }
    public abstract void DropTable<T>();
    public abstract void CreateTable<T>();

  }

}
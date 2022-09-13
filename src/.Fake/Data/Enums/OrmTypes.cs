
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Data.Repositories;

namespace X10sions.Fake.Data.Enums {
  public enum OrmType {
    Chloe,
    Dapper,
    EFCore,
    FreeSql,
    Linq2Db,
    NHibernate,
    OrmLite,
    PetaPoco,
    RepoDb,
    SmartSql,
    SqlSugar
  }

  public static class OrmTypeExtensions{

    public static IFakeRepo? GetFakeRepo(this OrmType ormType, ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) => ormType switch {
      //OrmType.Chloe => throw new NotImplementedException(),
      //OrmType.Dapper => throw new NotImplementedException(),
      OrmType.EFCore => new FakeRepoEFCore(name,configuration,loggerFactory),
      //OrmType.FreeSql => throw new NotImplementedException(),
      OrmType.Linq2Db => new FakeRepoLinqToDb(name, configuration, loggerFactory),
      //OrmType.NHibernate => throw new NotImplementedException(),
      OrmType.OrmLite => new FakeRepoOrmLite(name,configuration),
      //OrmType.PetaPoco => throw new NotImplementedException(),
      //OrmType.RepoDb => throw new NotImplementedException(),
      //OrmType.SmartSql => throw new NotImplementedException(),
      //OrmType.SqlSugar => throw new NotImplementedException(),
      _ => null
    };

  }

}

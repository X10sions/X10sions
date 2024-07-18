namespace CleanOnionExample.Data.DbContexts;

public class DbContextManager {
  public DbContextManager(EFCoreDbContexts.Access access, EFCoreDbContexts.DB2 db2, EFCoreDbContexts.DB2iSeries db2i, EFCoreDbContexts.MariaDb mariaDb, EFCoreDbContexts.MySql mySql, EFCoreDbContexts.PostgreSql postgreSql, EFCoreDbContexts.Oracle oracle, EFCoreDbContexts.SqlServer sqlServer, EFCoreDbContexts.Sqlite sqlite) {
    Access = access;
    DB2 = db2;
    DB2iSeries = db2i;
    MariaDb = mariaDb;
    MySql = mySql;
    PostgreSql = postgreSql;
    Oracle = oracle;
    SqlServer = sqlServer;
    Sqlite = sqlite;
  }
  public EFCoreDbContexts.Access Access { get; }
  public EFCoreDbContexts.DB2 DB2 { get; }
  public EFCoreDbContexts.DB2iSeries DB2iSeries { get; }
  public EFCoreDbContexts.MariaDb MariaDb { get; }
  public EFCoreDbContexts.MySql MySql { get; }
  public EFCoreDbContexts.PostgreSql PostgreSql { get; }
  public EFCoreDbContexts.Oracle Oracle { get; }
  public EFCoreDbContexts.SqlServer SqlServer { get; }
  public EFCoreDbContexts.Sqlite Sqlite { get; }

}

/*

add-migration v1

update-database


 */
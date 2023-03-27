using IBM.EntityFrameworkCore;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider.Access;
using LinqToDB.DataProvider.MySql;
using LinqToDB.DataProvider.Oracle;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Odbc;
using System.Data.OleDb;

namespace CleanOnionExample.Data.DbContexts;

public static class IDatabaseTableExtensions {
  public static DbSet<T> GetDbSet<T, TDatabase>(this IDatabaseTable<T, TDatabase> table) where T : class where TDatabase : IDatabase, IHaveDbContext => table.Database.DbContext.Set<T>();
  public static ITable<T> GetTable<T, TDatabase>(this IDatabaseTable<T, TDatabase> table) where T : class where TDatabase : IDatabase, IHaveDataContext => table.Database.DataContext.GetTable<T>();
}

public interface IHaveDbContext {
  DbContext DbContext { get; }
}

public interface IHaveDataContext {
  DataContext DataContext { get; }
}

public interface IHaveSession {
  ISession Session { get; }
}

public class CleanOnionExampleErpDatabase : IDatabase, IHaveDataContext, IHaveDbContext, IHaveSession {
  public CleanOnionExampleErpDatabase(DbContext dbContext, DataContext dataContext, ISession session) {
    DbContext = dbContext;
    DataContext = dataContext;
    Session = session;
  }
  public DbContext DbContext { get; }
  public DataContext DataContext { get; }
  public ISession Session { get; }

  public IDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase> ToDoItem => new DatabaseTable<ToDoItem, CleanOnionExampleErpDatabase>(this);
  //public IDatabaseTable<ToDoList, ICleanOnionExampleErpDatabase> ToDoList => new DatabaseTable<ToDoList>(this);
  //public IDatabaseTable<Project, ICleanOnionExampleErpDatabase> Project => new DatabaseTable<Project>(this);
  //public IDatabaseTable<WeatherForecast, ICleanOnionExampleErpDatabase> WeatherForecast => new CleanOnionExampleErpDatabaseTable<WeatherForecast>(this);
}

public static class ICleanOnionExampleErpDatabaseExtensions {
  public static IDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase> ToDoItem(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<ToDoItem, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<ToDoList, CleanOnionExampleErpDatabase> ToDoList(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<ToDoList, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<Project, CleanOnionExampleErpDatabase> Project(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<Project, CleanOnionExampleErpDatabase>();
  public static IDatabaseTable<WeatherForecast, CleanOnionExampleErpDatabase> WeatherForecast(this CleanOnionExampleErpDatabase db) => db.GetDatabaseTable<WeatherForecast, CleanOnionExampleErpDatabase>();
}

public class EFCoreDbContexts {
  public abstract class BaseDbContext : DbContext {
    public BaseDbContext(DbContextOptions options) : base(options) { }
  }

  public class Access : BaseDbContext { public Access(DbContextOptions<Access> options) : base(options) { } }
  public class DB2 : BaseDbContext { public DB2(DbContextOptions<DB2> options) : base(options) { } }
  public class DB2iSeries : BaseDbContext { public DB2iSeries(DbContextOptions<DB2iSeries> options) : base(options) { } }
  public class MariaDb : BaseDbContext { public MariaDb(DbContextOptions<MariaDb> options) : base(options) { } }
  public class MySql : BaseDbContext { public MySql(DbContextOptions<MySql> options) : base(options) { } }
  public class PostgreSql : BaseDbContext { public PostgreSql(DbContextOptions<PostgreSql> options) : base(options) { } }
  public class Oracle : BaseDbContext { public Oracle(DbContextOptions<Oracle> options) : base(options) { } }
  public class SqlServer : BaseDbContext { public SqlServer(DbContextOptions<SqlServer> options) : base(options) { } }
  public class Sqlite : BaseDbContext { public Sqlite(DbContextOptions<Sqlite> options) : base(options) { } }

}

public class LinqToDbDataConnections {
  public abstract class BaseDataConnection : DataConnection {
    public BaseDataConnection(LinqToDBConnectionOptions options) : base(options) { }
  }

  public class Access : BaseDataConnection { public Access(LinqToDBConnectionOptions<Access> options) : base(options) { } }
  public class DB2 : BaseDataConnection { public DB2(LinqToDBConnectionOptions<DB2> options) : base(options) { } }
  public class DB2iSeries_Odbc : BaseDataConnection { public DB2iSeries_Odbc(LinqToDBConnectionOptions<DB2iSeries_Odbc> options) : base(options) { } }
  public class DB2iSeries_OleDb : BaseDataConnection { public DB2iSeries_OleDb(LinqToDBConnectionOptions<DB2iSeries_OleDb> options) : base(options) { } }
  public class MariaDb : BaseDataConnection { public MariaDb(LinqToDBConnectionOptions<MariaDb> options) : base(options) { } }
  public class MySql : BaseDataConnection { public MySql(LinqToDBConnectionOptions<MySql> options) : base(options) { } }
  public class PostgreSql : BaseDataConnection { public PostgreSql(LinqToDBConnectionOptions<PostgreSql> options) : base(options) { } }
  public class Oracle : BaseDataConnection { public Oracle(LinqToDBConnectionOptions<Oracle> options) : base(options) { } }
  public class SqlServer : BaseDataConnection { public SqlServer(LinqToDBConnectionOptions<SqlServer> options) : base(options) { } }
  public class Sqlite : BaseDataConnection { public Sqlite(LinqToDBConnectionOptions<Sqlite> options) : base(options) { } }

}

public class RepositoryManager<T> where T : class {
  public RepositoryManager(IEFCoreRepository<T> efCore, ILinq2DBRepository<T> linqToDb, INHibernateRepository<T> nHibernate) {
    EFCore = efCore;
    LinqToDb = linqToDb;
    NHibernate = nHibernate;
  }

  public IEFCoreRepository<T> EFCore { get; }
  public ILinq2DBRepository<T> LinqToDb { get; }
  public INHibernateRepository<T> NHibernate { get; }

}

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

public static class Extensions {

  public static IServiceCollection AddScopedRepository_CleanOnionExample(this IServiceCollection services) {
    services.AddScoped(typeof(IEFCoreRepository<>), typeof(EFCoreRepository<>));
    services.AddScoped(typeof(IHttpClientRepository<>), typeof(HttpClientRepository<>));
    services.AddScoped(typeof(ILinq2DBRepository<>), typeof(Linq2DbRepository<>));
    services.AddScoped(typeof(INHibernateRepository<>), typeof(NHibernateRepository<>));
    services.AddScoped(typeof(RepositoryManager<>));
    return services;
  }

  public static IServiceCollection AddEFCore_CleanOnionExample(this IServiceCollection services, IAppSettings appSettings, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory) {
    services.AddDbContext<EFCoreDbContexts.Access>(options => { options.UseLoggerFactory(loggerFactory).UseJet(appSettings.ConnectionStrings.Access_OleDb()); });
    services.AddDbContext<EFCoreDbContexts.DB2>(options => {
      options.UseLoggerFactory(loggerFactory).UseDb2(appSettings.ConnectionStrings.DB2iSeries_IBM(), p => {
        p.SetServerInfo(IBMDBServerType.LUW);
        p.UseRowNumberForPaging();
      });
    });
    services.AddDbContext<EFCoreDbContexts.DB2iSeries>(options => {
      options.UseLoggerFactory(loggerFactory).UseDb2(appSettings.ConnectionStrings.DB2iSeries_IBM(), p => {
        p.SetServerInfo(IBMDBServerType.AS400);
        p.UseRowNumberForPaging();
      });
    });
    var mysql = appSettings.ConnectionStrings.MySql();
    var maria = appSettings.ConnectionStrings.MariaDb();
    services.AddDbContext<EFCoreDbContexts.MariaDb>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(maria, ServerVersion.AutoDetect(maria)); });
    services.AddDbContext<EFCoreDbContexts.MySql>(options => { options.UseLoggerFactory(loggerFactory).UseMySql(mysql, ServerVersion.AutoDetect(mysql)); });
    services.AddDbContext<EFCoreDbContexts.PostgreSql>(options => { options.UseLoggerFactory(loggerFactory).UseNpgsql(appSettings.ConnectionStrings.PostgreSql()); });
    services.AddDbContext<EFCoreDbContexts.Oracle>(options => { options.UseLoggerFactory(loggerFactory).UseOracle(appSettings.ConnectionStrings.Oracle()); });
    services.AddDbContext<EFCoreDbContexts.SqlServer>(options => { options.UseLoggerFactory(loggerFactory).UseSqlServer(appSettings.ConnectionStrings.SqlServer()); });
    services.AddDbContext<EFCoreDbContexts.Sqlite>(options => { options.UseLoggerFactory(loggerFactory).UseSqlite(appSettings.ConnectionStrings.Sqlite()); });
    services.AddScoped<DbContextManager>();
    return services;
  }

  public static IServiceCollection AddLinqToDb_CleanOnionExample(this IServiceCollection services, IAppSettings appSettings, ILogger logger) {
    services.AddLinqToDBContext<LinqToDbDataConnections.Access, OleDbConnection>(AccessTools.GetDataProvider(ProviderName.Access), appSettings.ConnectionStrings.Access_OleDb(), logger);
    //services.AddLinqToDbContext<LinqToDbDataConnections.DB2, DB2Connection>(new DB2DataProvider(nameof(IBM.Data.Db2), DB2Version.LUW), appSettings.ConnectionStrings.DB2(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_Odbc, OdbcConnection, OdbcDataReader>(appSettings.ConnectionStrings.DB2iSeries_Odbc(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.DB2iSeries_OleDb, OleDbConnection, OleDbDataReader>(appSettings.ConnectionStrings.DB2iSeries_OleDb(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.MariaDb, MySql.Data.MySqlClient.MySqlConnection>(MySqlTools.GetDataProvider(ProviderName.MySql), appSettings.ConnectionStrings.MariaDb(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.MySql, MySqlConnector.MySqlConnection>(MySqlTools.GetDataProvider(ProviderName.MySqlConnector), appSettings.ConnectionStrings.MySql(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.PostgreSql, NpgsqlConnection>(PostgreSQLTools.GetDataProvider(), appSettings.ConnectionStrings.PostgreSql(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.Oracle, OracleConnection>(OracleTools.GetDataProvider(OracleVersion.v12, OracleProvider.Managed), appSettings.ConnectionStrings.Oracle(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.SqlServer, SqlConnection>(SqlServerTools.GetDataProvider(SqlServerVersion.v2012, SqlServerProvider.MicrosoftDataSqlClient), appSettings.ConnectionStrings.SqlServer(), logger);
    services.AddLinqToDBContext<LinqToDbDataConnections.Sqlite, SqliteConnection>(SQLiteTools.GetDataProvider(ProviderName.SQLiteMS), appSettings.ConnectionStrings.Sqlite(), logger);
    return services;
  }
}

/*

add-migration v1

update-database


 */
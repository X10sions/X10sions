using EntityFrameworkCore.Jet.Data;
using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  //public class FakeRepoEFCore<T> : FakeRepoEFCore where T:DbContext  {
  //  public FakeRepoEFCore(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) :base(name,configuration,loggerFactory){ }
  //}

  public class FakeRepoEFCore : FakeRepo {
    public FakeRepoEFCore(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) : base(name, configuration) {
      DbContext = new EFCoreDbContexts.BaseDbContext(name.GetEFCoreDbContextOptions(configuration, loggerFactory));
    }
    public DbContext DbContext { get; }

    public override IQueryable<T> GetQueryable<T>() => DbContext.Set<T>();

    public override void CreateTable<T>() => throw new NotImplementedException();
    public override void DropTable<T>() => DbContext.DropTable<T>();
    public override long Delete<T>(IEnumerable<T> rows) {
      DbContext.Set<T>().RemoveRange(rows);
      return DbContext.SaveChanges();
    }

    public override long Insert<T>(IEnumerable<T> rows) {
      DbContext.Set<T>().AddRange(rows);
      return DbContext.SaveChanges();
    }

    public override T InsertWithIdentity<T>(T row) {
      DbContext.Set<T>().Add(row);
      DbContext.SaveChanges();
      return row;
    }

    public override long Update<T>(IEnumerable<T> rows) {
      DbContext.Set<T>().AttachRange(rows);
      return DbContext.SaveChanges();
    }

  }

  public static class EFCoreExtensions {

    public static int DropTable<T>(this DbContext dbContext) => dbContext.Database.ExecuteSqlInterpolated($"DROP TABLE {dbContext.GetSchemaQualifiedTableName<T>()}");
    public static string? GetTableName<T>(this DbContext dbContext) => dbContext.Model?.FindEntityType(typeof(T))?.GetTableName();
    public static string? GetSchemaQualifiedTableName<T>(this DbContext dbContext) => dbContext.Model?.FindEntityType(typeof(T))?.GetSchemaQualifiedTableName();
    public static string? GetSchemaName<T>(this DbContext dbContext) => dbContext.Model?.FindEntityType(typeof(T))?.GetSchema();
    public static string? GetSchemaQualifiedViewName<T>(this DbContext dbContext) => dbContext.Model?.FindEntityType(typeof(T))?.GetSchemaQualifiedViewName();

    public static DbContextOptions<T> GetEFCoreDbContextOptions<T>(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) where T : DbContext => name.GetEFCoreDbContextOptionsBuilder<T>(configuration, loggerFactory).Options;
    public static DbContextOptions GetEFCoreDbContextOptions(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) => name.GetEFCoreDbContextOptionsBuilder(configuration, loggerFactory).Options;

    static DbContextOptionsBuilder<T> GetEFCoreDbContextOptionsBuilder<T>(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) where T : DbContext => new DbContextOptionsBuilder<T>().SetOptions(name, configuration, loggerFactory);
    static DbContextOptionsBuilder GetEFCoreDbContextOptionsBuilder(this ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) => new DbContextOptionsBuilder().SetOptions(name, configuration, loggerFactory);

    static T SetOptions<T>(this T builder, ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) where T : DbContextOptionsBuilder {
      var connectionString = name.GetConnectionString(configuration);
      //var factory = name.GetDbProviderFactory();
      builder.UseLoggerFactory(loggerFactory);
      switch (name) {
#if windows
        case ConnectionStringName.Access_Odbc: builder.UseJet(connectionString, DataAccessProviderType.Odbc); break;
        case ConnectionStringName.Access_OleDb: builder.UseJet(connectionString, DataAccessProviderType.OleDb); break;
#endif
        case ConnectionStringName.DB2_IBM: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.LUW); }); break;
        case ConnectionStringName.DB2_Odbc: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.LUW); }); break;
        case ConnectionStringName.DB2_OleDb: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.LUW); }); break;
        case ConnectionStringName.DB2iSeries_IBM: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.DB2iSeries_Odbc: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.DB2iSeries_OleDb: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.Firebird: builder.UseFirebird(connectionString); break;
        case ConnectionStringName.MariaDb: builder.UseMySql(connectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString))); break;
        case ConnectionStringName.MySql_Client: builder.UseMySQL(connectionString); break;
        case ConnectionStringName.MySql_Connector: builder.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))); break;
        case ConnectionStringName.PostgreSql: builder.UseNpgsql(connectionString); break;
        case ConnectionStringName.Oracle: builder.UseOracle(connectionString); break;
        case ConnectionStringName.Sqlite_Microsoft: builder.UseSqlite(connectionString); break;
        case ConnectionStringName.Sqlite_System: builder.UseSqlite(connectionString); break;
        case ConnectionStringName.SqlServer_Microsoft: builder.UseSqlServer(connectionString); break;
        case ConnectionStringName.SqlServer_System: builder.UseSqlServer(connectionString); break;
        case ConnectionStringName.SqlServer_Odbc: builder.UseSqlServer(connectionString); break;
        case ConnectionStringName.SqlServer_OleDb: builder.UseSqlServer(connectionString); break;
        default: throw new NotImplementedException(name.ToString());
      }
      return builder;
    }

    public static IServiceCollection AddEFCore_Fake(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory) {

      services.AddTransient<EFCoreDbContexts._Resolver>(serviceProvider => name => {
        return name switch {
          ConnectionStringName.Access_Odbc => serviceProvider.GetRequiredService<EFCoreDbContexts.Access_Odbc>(),
          ConnectionStringName.Access_OleDb => serviceProvider.GetRequiredService<EFCoreDbContexts.Access_OleDb>(),
          ConnectionStringName.DB2_IBM => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2_IBM>(),
          ConnectionStringName.DB2_Odbc => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2_Odbc>(),
          ConnectionStringName.DB2_OleDb => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2_OleDb>(),
          ConnectionStringName.DB2iSeries_IBM => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_IBM>(),
          ConnectionStringName.DB2iSeries_Odbc => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_Odbc>(),
          ConnectionStringName.DB2iSeries_OleDb => serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_OleDb>(),
          ConnectionStringName.Firebird => serviceProvider.GetRequiredService<EFCoreDbContexts.Firebird>(),
          ConnectionStringName.MariaDb => serviceProvider.GetRequiredService<EFCoreDbContexts.MariaDb>(),
          ConnectionStringName.MySql_Client => serviceProvider.GetRequiredService<EFCoreDbContexts.MySql_Client>(),
          ConnectionStringName.MySql_Connector => serviceProvider.GetRequiredService<EFCoreDbContexts.MySql_Connector>(),
          ConnectionStringName.Oracle => serviceProvider.GetRequiredService<EFCoreDbContexts.Oracle>(),
          ConnectionStringName.PostgreSql => serviceProvider.GetRequiredService<EFCoreDbContexts.PostgreSql>(),
          ConnectionStringName.Sqlite_Microsoft => serviceProvider.GetRequiredService<EFCoreDbContexts.Sqlite_Microsoft>(),
          ConnectionStringName.Sqlite_System => serviceProvider.GetRequiredService<EFCoreDbContexts.Sqlite_System>(),
          ConnectionStringName.SqlServer_Microsoft => serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_Microsoft>(),
          ConnectionStringName.SqlServer_System => serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_System>(),
          ConnectionStringName.SqlServer_Odbc => serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_Odbc>(),
          ConnectionStringName.SqlServer_OleDb => serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_OleDb>(),
          _ => throw new NotSupportedException($"RepositoryResolver, key: {name}"),
        };
      });

      services.AddScoped(x => new EFCoreDbContexts.Access_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Access_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2_IBM(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2iSeries_IBM(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2iSeries_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2iSeries_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Firebird(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.MariaDb(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.MySql_Client(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.MySql_Connector(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Oracle(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.PostgreSql(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Sqlite_Microsoft(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Sqlite_System(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.SqlServer_Microsoft(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.SqlServer_System(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.SqlServer_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.SqlServer_OleDb(configuration, loggerFactory));

      return services;
    }

  }
}

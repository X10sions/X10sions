using IBM.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Data.Enums;

namespace X10sions.Fake.Data.Repositories {
  public class FakeRepoEFCore<T> : FakeRepoEFCore where T:DbContext  {
    public FakeRepoEFCore(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) :base(name,configuration,loggerFactory){ }
  }

  public class FakeRepoEFCore : FakeRepo {
    public FakeRepoEFCore(ConnectionStringName name, IConfiguration configuration, ILoggerFactory loggerFactory) : base(name, configuration) {
      DbContext = new EFCoreDbContexts.BaseDbContext(name.GetEFCoreDbContextOptions(configuration, loggerFactory));
    }

    public DbContext DbContext { get; }
    public override void CreateTable<T>() => throw new NotImplementedException();
    public override void DropTable<T>() => DbContext.DropTable<T>();

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
        case ConnectionStringName.Access_Odbc: builder.UseJet(connectionString, EntityFrameworkCore.Jet.Data.DataAccessProviderType.Odbc); break;
        case ConnectionStringName.Access_OleDb: builder.UseJet(connectionString, EntityFrameworkCore.Jet.Data.DataAccessProviderType.OleDb); break;
        case ConnectionStringName.DB2_IBM: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.LUW); }); break;
        case ConnectionStringName.DB2_Odbc: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.LUW); }); break;
        case ConnectionStringName.DB2iSeries_IBM: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.DB2iSeries_Odbc: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.DB2iSeries_OleDb: builder.UseDb2(connectionString, x => { x.SetServerInfo(IBMDBServerType.AS400); x.UseRowNumberForPaging(); }); break;
        case ConnectionStringName.Firebird: builder.UseFirebird(connectionString); break;
        case ConnectionStringName.MariaDb: builder.UseMySql(connectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString))); break;
        case ConnectionStringName.MySql_Client: builder.UseMySQL(connectionString); break;
        case ConnectionStringName.MySql_Connector: builder.UseMySql(connectionString, new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))); break;
        case ConnectionStringName.PostgreSql: builder.UseNpgsql(connectionString); break;
        case ConnectionStringName.Oracle: builder.UseOracle(connectionString); break;
        case ConnectionStringName.SqlServer_Microsoft: builder.UseSqlServer(connectionString); break;
        case ConnectionStringName.SqlServer_System: builder.UseSqlServer(connectionString); break;
        case ConnectionStringName.Sqlite_Microsoft: builder.UseSqlite(connectionString); break;
        case ConnectionStringName.Sqlite_System: builder.UseSqlite(connectionString); break;
        default: throw new NotImplementedException(name.ToString());
      }
      return builder;
    }

    public static IServiceCollection AddEFCore_Fake(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory) {

      services.AddTransient<EFCoreDbContexts._Resolver>(serviceProvider => name => {
        switch (name) {
          case ConnectionStringName.Access_Odbc: return serviceProvider.GetRequiredService<EFCoreDbContexts.Access_Odbc>();
          case ConnectionStringName.Access_OleDb: return serviceProvider.GetRequiredService<EFCoreDbContexts.Access_OleDb>();
          case ConnectionStringName.DB2_IBM: return serviceProvider.GetRequiredService<EFCoreDbContexts.DB2_IBM>();
          case ConnectionStringName.DB2_Odbc: return serviceProvider.GetRequiredService<EFCoreDbContexts.DB2_Odbc>();
          case ConnectionStringName.DB2iSeries_IBM: return serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_IBM>();
          case ConnectionStringName.DB2iSeries_Odbc: return serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_Odbc>();
          case ConnectionStringName.DB2iSeries_OleDb: return serviceProvider.GetRequiredService<EFCoreDbContexts.DB2iSeries_OleDb>();
          case ConnectionStringName.Firebird: return serviceProvider.GetRequiredService<EFCoreDbContexts.Firebird>();
          case ConnectionStringName.MariaDb: return serviceProvider.GetRequiredService<EFCoreDbContexts.MariaDb>();
          case ConnectionStringName.MySql_Client: return serviceProvider.GetRequiredService<EFCoreDbContexts.MySql_Client>();
          case ConnectionStringName.MySql_Connector: return serviceProvider.GetRequiredService<EFCoreDbContexts.MySql_Connector>();
          case ConnectionStringName.Oracle: return serviceProvider.GetRequiredService<EFCoreDbContexts.Oracle>();
          case ConnectionStringName.PostgreSql: return serviceProvider.GetRequiredService<EFCoreDbContexts.PostgreSql>();
          case ConnectionStringName.Sqlite_Microsoft: return serviceProvider.GetRequiredService<EFCoreDbContexts.Sqlite_Microsoft>();
          case ConnectionStringName.Sqlite_System: return serviceProvider.GetRequiredService<EFCoreDbContexts.Sqlite_System>();
          case ConnectionStringName.SqlServer_Microsoft: return serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_Microsoft>();
          case ConnectionStringName.SqlServer_System: return serviceProvider.GetRequiredService<EFCoreDbContexts.SqlServer_System>();
          default: throw new NotSupportedException($"RepositoryResolver, key: {name}");
        }
      });

      services.AddScoped(x => new EFCoreDbContexts.Access_Odbc(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.Access_OleDb(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2_IBM(configuration, loggerFactory));
      services.AddScoped(x => new EFCoreDbContexts.DB2_Odbc(configuration, loggerFactory));
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

      return services;
    }

  }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Data.Enums;
using X10sions.Fake.Data.Models;
using X10sions.Fake.Data.Repositories;

namespace X10sions.Fake.Data {

  public interface IFakeDbContext { }

  public class EFCoreDbContexts {
    public class BaseDbContext : DbContext, IFakeDbContext {
      public BaseDbContext(DbContextOptions options) : base(options) { }

      //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {    }

      //    protected override void OnModelCreating(ModelBuilder modelBuilder) {    }

      public DbSet<FakePerson> Person { get; set; }
      public DbSet<FakeProject> Project { get; set; }
      public DbSet<FakeProjectItem> ProjectItem { get; set; }
      public DbSet<FakePriority> Priority { get; set; }
    }

    public delegate IFakeDbContext _Resolver(ConnectionStringName name);

    public class Access_Odbc : BaseDbContext { public Access_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Access_Odbc.GetEFCoreDbContextOptions<Access_Odbc>(configuration, loggerFactory)) { } }
    public class Access_OleDb : BaseDbContext { public Access_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Access_OleDb.GetEFCoreDbContextOptions<Access_OleDb>(configuration, loggerFactory)) { } }
    public class DB2_IBM : BaseDbContext { public DB2_IBM(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2_IBM.GetEFCoreDbContextOptions<DB2_IBM>(configuration, loggerFactory)) { } }
    public class DB2_Odbc : BaseDbContext { public DB2_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2_Odbc.GetEFCoreDbContextOptions<DB2_Odbc>(configuration, loggerFactory)) { } }
    public class DB2iSeries_IBM : BaseDbContext { public DB2iSeries_IBM(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_IBM.GetEFCoreDbContextOptions<DB2iSeries_IBM>(configuration, loggerFactory)) { } }
    public class DB2iSeries_Odbc : BaseDbContext { public DB2iSeries_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_Odbc.GetEFCoreDbContextOptions<DB2iSeries_Odbc>(configuration, loggerFactory)) { } }
    public class DB2iSeries_OleDb : BaseDbContext { public DB2iSeries_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_OleDb.GetEFCoreDbContextOptions<DB2iSeries_OleDb>(configuration, loggerFactory)) { } }
    public class Firebird : BaseDbContext { public Firebird(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Firebird.GetEFCoreDbContextOptions<Firebird>(configuration, loggerFactory)) { } }
    public class MariaDb : BaseDbContext { public MariaDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MariaDb.GetEFCoreDbContextOptions<MariaDb>(configuration, loggerFactory)) { } }
    public class MySql_Client : BaseDbContext { public MySql_Client(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MySql_Client.GetEFCoreDbContextOptions<MySql_Client>(configuration, loggerFactory)) { } }
    public class MySql_Connector : BaseDbContext { public MySql_Connector(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MySql_Connector.GetEFCoreDbContextOptions<MySql_Connector>(configuration, loggerFactory)) { } }
    public class PostgreSql : BaseDbContext { public PostgreSql(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.PostgreSql.GetEFCoreDbContextOptions<PostgreSql>(configuration, loggerFactory)) { } }
    public class Oracle : BaseDbContext { public Oracle(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Oracle.GetEFCoreDbContextOptions<Oracle>(configuration, loggerFactory)) { } }
    public class Sqlite_Microsoft : BaseDbContext { public Sqlite_Microsoft(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Sqlite_Microsoft.GetEFCoreDbContextOptions<Sqlite_Microsoft>(configuration, loggerFactory)) { } }
    public class Sqlite_System : BaseDbContext { public Sqlite_System(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Sqlite_System.GetEFCoreDbContextOptions<Sqlite_System>(configuration, loggerFactory)) { } }
    public class SqlServer_Microsoft : BaseDbContext { public SqlServer_Microsoft(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_Microsoft.GetEFCoreDbContextOptions<SqlServer_Microsoft>(configuration, loggerFactory)) { } }
    public class SqlServer_System : BaseDbContext { public SqlServer_System(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_System.GetEFCoreDbContextOptions<SqlServer_System>(configuration, loggerFactory)) { } }

  }

}

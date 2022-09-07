using Microsoft.EntityFrameworkCore;

namespace X10sions.Fake.Data {
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
}

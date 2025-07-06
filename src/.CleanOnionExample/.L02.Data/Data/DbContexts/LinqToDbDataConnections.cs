using LinqToDB;
using LinqToDB.Data;

namespace CleanOnionExample.Data.DbContexts;

public class LinqToDbDataConnections {
  public abstract class BaseDataConnection(DataOptions options) : DataConnection(options) {  }

  public class Access(DataOptions<Access> options) : BaseDataConnection(options.Options) {  }
  public class DB2(DataOptions<DB2> options) : BaseDataConnection(options.Options) {  }
  public class DB2iSeries_Odbc(DataOptions<DB2iSeries_Odbc> options) : BaseDataConnection(options.Options) {  }
  public class DB2iSeries_OleDb(DataOptions<DB2iSeries_OleDb> options) : BaseDataConnection(options.Options) {  }
  public class MariaDb(DataOptions<MariaDb> options) : BaseDataConnection(options.Options) {  }
  public class MySql(DataOptions<MySql> options) : BaseDataConnection(options.Options) {  }
  public class PostgreSql(DataOptions<PostgreSql> options) : BaseDataConnection(options.Options) {  }
  public class Oracle(DataOptions<Oracle> options) : BaseDataConnection(options.Options) {  }
  public class SqlServer(DataOptions<SqlServer> options) : BaseDataConnection(options.Options) {  }
  public class Sqlite(DataOptions<Sqlite> options) : BaseDataConnection(options.Options) {  }

}

/*

add-migration v1

update-database


 */
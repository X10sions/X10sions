using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using X10sions.Fake.Data.Models;
using X10sions.Fake.Data.Enums;
using X10sions.Fake.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace X10sions.Fake.Data {
  public interface IFakeDataConnection { }

  //public delegate IFakeDataConnection FakeDataConnectionResolver(ConnectionStringName name);
  public class LinqToDbDataConnections {

    public abstract class BaseDataConnection<TConnection> : BaseDataConnection where TConnection : DbConnection {
      public BaseDataConnection(LinqToDBConnectionOptions options) : base(options) { }
      public TConnection TypedConnection => (TConnection)Connection;
    }

    public class BaseDataConnection : DataConnection, IFakeDataConnection {
      public BaseDataConnection(LinqToDBConnectionOptions options) : base(options) {
        //public _BaseDataConnection(IDataProvider dataProvider, DbConnection dbConnection, bool useMiniProfiler) : base(dataProvider, dbConnection, useMiniProfiler) {
        //MappingSchema.AddMetadataReader(new CommonAttributesMetadataReader());
        //AddMapMemberExpressions();
        //SetLinqExpressions();
        //SetConvertExpressions();
        //AddFluentMappings();
        AddAssocations();
        //this.ConfigureMiniProfiler();
      }

      void AddAssocations() {
        this.AddAssociation01<FakePerson, FakePerson?>(x => x.Father, y => y.FatherOf, (x, y) => x.FatherId == y.Id);
        this.AddAssociation01<FakePerson, FakePerson?>(x => x.Mother, y => y.MotherOf, (x, y) => x.MotherId == y.Id);

        this.AddAssociation01<FakeProjectItem, FakePerson?>(x => x.AssignedToPerson, y => y.AssignedProjectItems, (x, y) => x.PriorityId == y.Id);
        this.AddAssociation00<FakeProjectItem, FakePerson>(x => x.CreatedByPerson, y => y.CreatedProjectItems, (x, y) => x.PriorityId == y.Id);
        this.AddAssociation00<FakeProjectItem, FakePriority>(x => x.Priority, y => y.ProjectItems, (x, y) => x.PriorityId == y.Id);
        this.AddAssociation00<FakeProjectItem, FakeProject>(x => x.Project, y => y.Items, (x, y) => x.PriorityId == y.Id);
      }

      public ITable<FakePerson> Person => this.GetTable<FakePerson>();
      public ITable<FakeProject> Project => this.GetTable<FakeProject>();
      public ITable<FakeProjectItem> ProjectItem => this.GetTable<FakeProjectItem>();
      public ITable<FakePriority> Priority => this.GetTable<FakePriority>();

    }

    public delegate IFakeDataConnection _Resolver(ConnectionStringName name);

    public class Access_Odbc : BaseDataConnection { public Access_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Access_Odbc.GetLinqToDBConnectionOptions<Access_Odbc>(configuration, loggerFactory)) { } }
    public class Access_OleDb : BaseDataConnection { public Access_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Access_OleDb.GetLinqToDBConnectionOptions<Access_OleDb>(configuration, loggerFactory)) { } }
    public class DB2_IBM : BaseDataConnection { public DB2_IBM(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2_IBM.GetLinqToDBConnectionOptions<DB2_IBM>(configuration, loggerFactory)) { } }
    public class DB2_Odbc : BaseDataConnection { public DB2_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2_Odbc.GetLinqToDBConnectionOptions<DB2_Odbc>(configuration, loggerFactory)) { } }
    public class DB2_OleDb : BaseDataConnection { public DB2_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2_OleDb.GetLinqToDBConnectionOptions<DB2_OleDb>(configuration, loggerFactory)) { } }
    public class DB2iSeries_IBM: BaseDataConnection { public DB2iSeries_IBM(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_IBM.GetLinqToDBConnectionOptions<DB2iSeries_IBM>(configuration, loggerFactory)) { } }
    public class DB2iSeries_Odbc : BaseDataConnection { public DB2iSeries_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_Odbc.GetLinqToDBConnectionOptions<DB2iSeries_Odbc>(configuration, loggerFactory)) { } }
    public class DB2iSeries_OleDb : BaseDataConnection { public DB2iSeries_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.DB2iSeries_OleDb.GetLinqToDBConnectionOptions<DB2iSeries_OleDb>(configuration, loggerFactory)) { } }
    public class Firebird : BaseDataConnection { public Firebird(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Firebird.GetLinqToDBConnectionOptions<Firebird>(configuration, loggerFactory)) { } }
    public class MariaDb : BaseDataConnection { public MariaDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MariaDb.GetLinqToDBConnectionOptions<MariaDb>(configuration, loggerFactory)) { } }
    public class MySql_Client : BaseDataConnection { public MySql_Client(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MySql_Client.GetLinqToDBConnectionOptions<MySql_Client>(configuration, loggerFactory)) { } }
    public class MySql_Connector : BaseDataConnection { public MySql_Connector(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.MySql_Connector.GetLinqToDBConnectionOptions<MySql_Connector>(configuration, loggerFactory)) { } }
    public class PostgreSql : BaseDataConnection { public PostgreSql(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.PostgreSql.GetLinqToDBConnectionOptions<PostgreSql>(configuration, loggerFactory)) { } }
    public class Oracle : BaseDataConnection { public Oracle(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Oracle.GetLinqToDBConnectionOptions<Oracle>(configuration, loggerFactory)) { } }
    public class Sqlite_Microsoft : BaseDataConnection { public Sqlite_Microsoft(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Sqlite_Microsoft.GetLinqToDBConnectionOptions<Sqlite_Microsoft>(configuration, loggerFactory)) { } }
    public class Sqlite_System : BaseDataConnection { public Sqlite_System(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.Sqlite_System.GetLinqToDBConnectionOptions<Sqlite_System>(configuration, loggerFactory)) { } }
    public class SqlServer_Microsoft : BaseDataConnection { public SqlServer_Microsoft(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_Microsoft.GetLinqToDBConnectionOptions<SqlServer_Microsoft>(configuration, loggerFactory)) { } }
    public class SqlServer_System : BaseDataConnection { public SqlServer_System(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_System.GetLinqToDBConnectionOptions<SqlServer_System>(configuration, loggerFactory)) { } }
    public class SqlServer_Odbc : BaseDataConnection { public SqlServer_Odbc(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_Odbc.GetLinqToDBConnectionOptions<SqlServer_Odbc>(configuration, loggerFactory)) { } }
    public class SqlServer_OleDb : BaseDataConnection { public SqlServer_OleDb(IConfiguration configuration, ILoggerFactory loggerFactory) : base(ConnectionStringName.SqlServer_OleDb.GetLinqToDBConnectionOptions<SqlServer_OleDb>(configuration, loggerFactory)) { } }

  }
}
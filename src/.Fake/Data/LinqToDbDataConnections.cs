using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using X10sions.Fake.Data.Models;
using System.Data.Common;
using X10sions.Fake.Data.Enums;
using X10sions.Fake.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace X10sions.Fake.Data {
  public class LinqToDbDataConnections {

    public abstract class BaseDataConnection<TConnection> : BaseDataConnection where TConnection : DbConnection {
      public BaseDataConnection(LinqToDBConnectionOptions options) : base(options) { }
      public TConnection TypedConnection => (TConnection)Connection;
    }

    public class BaseDataConnection : DataConnection {
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

    public class Access_Odbc : BaseDataConnection { public Access_Odbc(IConfiguration configuration) : base(  ConnectionStringName.Access_Odbc.GetLinqToDBConnectionOptions<Access_Odbc>(configuration)) { } }
    public class Access_OleDb : BaseDataConnection { public Access_OleDb(IConfiguration configuration) : base(ConnectionStringName.Access_OleDb.GetLinqToDBConnectionOptions<Access_OleDb>(configuration)) { } }
    public class DB2_IBM : BaseDataConnection { public DB2_IBM(IConfiguration configuration) : base(ConnectionStringName.DB2_IBM.GetLinqToDBConnectionOptions<DB2_IBM>(configuration)) { } }
    public class DB2_Odbc : BaseDataConnection { public DB2_Odbc(IConfiguration configuration) : base(ConnectionStringName.DB2_Odbc.GetLinqToDBConnectionOptions<DB2_Odbc>(configuration)) { } }
    public class DB2iSeries_Odbc : BaseDataConnection { public DB2iSeries_Odbc(IConfiguration configuration) : base(ConnectionStringName.DB2iSeries_Odbc.GetLinqToDBConnectionOptions<DB2iSeries_Odbc>(configuration)) { } }
    public class DB2iSeries_OleDb : BaseDataConnection { public DB2iSeries_OleDb(IConfiguration configuration) : base(ConnectionStringName.DB2iSeries_OleDb.GetLinqToDBConnectionOptions<DB2iSeries_OleDb>(configuration)) { } }
    public class MariaDb : BaseDataConnection { public MariaDb(IConfiguration configuration) : base(ConnectionStringName.MariaDb.GetLinqToDBConnectionOptions<MariaDb>(configuration)) { } }
    public class MySql_Client : BaseDataConnection { public MySql_Client(IConfiguration configuration) : base(ConnectionStringName.MySql_Client.GetLinqToDBConnectionOptions<MySql_Client>(configuration)) { } }
    public class MySql_Connector : BaseDataConnection { public MySql_Connector(IConfiguration configuration) : base(ConnectionStringName.MySql_Connector.GetLinqToDBConnectionOptions<MySql_Connector>(configuration)) { } }
    public class PostgreSql : BaseDataConnection { public PostgreSql(IConfiguration configuration) : base(ConnectionStringName.PostgreSql.GetLinqToDBConnectionOptions<PostgreSql>(configuration)) { } }
    public class Oracle : BaseDataConnection { public Oracle(IConfiguration configuration) : base(ConnectionStringName.Oracle.GetLinqToDBConnectionOptions<Oracle>(configuration)) { } }
    public class Sqlite_Microsoft : BaseDataConnection { public Sqlite_Microsoft(IConfiguration configuration) : base(ConnectionStringName.Sqlite_Microsoft.GetLinqToDBConnectionOptions<Sqlite_Microsoft>(configuration)) { } }
    public class SqlServer_Microsoft : BaseDataConnection { public SqlServer_Microsoft(IConfiguration configuration) : base(ConnectionStringName.SqlServer_Microsoft.GetLinqToDBConnectionOptions<SqlServer_Microsoft>(configuration)) { } }
  }
}
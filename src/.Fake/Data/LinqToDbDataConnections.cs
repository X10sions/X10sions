using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using X10sions.Fake.Data.Models;
using System.Data.Common;

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

    public class Access_OleDb : BaseDataConnection { public Access_OleDb(LinqToDBConnectionOptions<Access_OleDb> options) : base(options) { } }
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
}

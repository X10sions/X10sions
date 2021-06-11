using IBM.Data.DB2.iSeries; // System.Data.SqlClient;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400RelationalConnection : RelationalConnection, IAS400RelationalConnection {

    public AS400RelationalConnection([NotNull] RelationalConnectionDependencies dependencies)
        : base(dependencies) {
    }

    internal const int DefaultMasterConnectionCommandTimeout = 60;

    protected override DbConnection CreateDbConnection() => new iDB2Connection(ConnectionString);

    public virtual IAS400RelationalConnection CreateMasterConnection() {
      var csb = new iDB2ConnectionStringBuilder(ConnectionString);
      //TODO  csb.ReadOnly = true;
      var contextOptions = new DbContextOptionsBuilder().UseAS400x(
        csb.ConnectionString,
        b => b.CommandTimeout(CommandTimeout ?? DefaultMasterConnectionCommandTimeout)
        ).Options;
      return new AS400RelationalConnection(Dependencies.With(contextOptions));
    }

    iDB2NamingConvention? _namingConvention;
    public iDB2NamingConvention NamingConvention
      => (iDB2NamingConvention)(_namingConvention
          ?? (_namingConvention = new iDB2ConnectionStringBuilder(ConnectionString).Naming));


    //bool? _multipleActiveResultSetsEnabled;
    //public override bool IsMultipleActiveResultSetsEnabled
    //    => (bool)(_multipleActiveResultSetsEnabled
    //              ?? (_multipleActiveResultSetsEnabled
    //                  = new iDB2ConnectionStringBuilder(ConnectionString).MultipleActiveResultSets));

  }
}

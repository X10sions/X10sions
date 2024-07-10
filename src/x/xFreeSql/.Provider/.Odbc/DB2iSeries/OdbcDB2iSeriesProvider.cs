using FreeSql.Internal.CommonProvider;
using System.Data.Common;

namespace FreeSql.Odbc.DB2iSeries;

public class OdbcDB2iSeriesProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {
  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new OdbcDB2iSeriesSelect<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new OdbcDB2iSeriesInsert<T1>(this, InternalCommonUtils, InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new OdbcDB2iSeriesUpdate<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new OdbcDB2iSeriesDelete<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new OdbcDB2iSeriesInsertOrUpdate<T1>(this, InternalCommonUtils, InternalCommonExpression);

  public OdbcDB2iSeriesProvider(string masterConnectionString, string[] slaveConnectionString, Func<DbConnection> connectionFactory = null) {
    InternalCommonUtils = new OdbcDB2iSeriesUtils(this);
    InternalCommonExpression = new OdbcDB2iSeriesExpression(InternalCommonUtils);

    Ado = new OdbcDB2iSeriesAdo(InternalCommonUtils, masterConnectionString, slaveConnectionString, connectionFactory);
    Aop = new AopProvider();

    DbFirst = new OdbcDB2iSeriesDbFirst(this, InternalCommonUtils, InternalCommonExpression);
    CodeFirst = new OdbcDB2iSeriesCodeFirst(this, InternalCommonUtils, InternalCommonExpression);
    CodeFirst = new OdbcDB2iSeriesCodeFirst(this, InternalCommonUtils, InternalCommonExpression);

    if (Ado.MasterPool != null)
      try {
        using (var conn = Ado.MasterPool.Get()) {
          (InternalCommonUtils as OdbcDB2iSeriesUtils).ServerVersion = int.Parse(conn.Value.ServerVersion.Split('.')[0]);
        }
      } catch {
      }
  }

  ~OdbcDB2iSeriesProvider() => Dispose();
  int _disposeCounter;
  public override void Dispose() {
    if (Interlocked.Increment(ref _disposeCounter) != 1) return;
    (Ado as AdoProvider)?.Dispose();
  }
}

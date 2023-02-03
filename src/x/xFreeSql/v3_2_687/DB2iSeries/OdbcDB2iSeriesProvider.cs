using FreeSql.Internal.CommonProvider;
using System.Data.Common;

namespace FreeSql.DB2iSeries;

public class OdbcDB2iSeriesProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {
  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new OdbcDB2iSeriesSelect<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new OdbcDB2iSeriesInsert<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new OdbcDB2iSeriesUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new OdbcDB2iSeriesDelete<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new OdbcDB2iSeriesInsertOrUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);

  public OdbcDB2iSeriesProvider(string masterConnectionString, string[] slaveConnectionString, Func<DbConnection> connectionFactory = null) {
    this.InternalCommonUtils = new OdbcDB2iSeriesUtils(this);
    this.InternalCommonExpression = new OdbcDB2iSeriesExpression(this.InternalCommonUtils);

    this.Ado = new OdbcDB2iSeriesAdo(this.InternalCommonUtils, masterConnectionString, slaveConnectionString, connectionFactory);
    this.Aop = new AopProvider();

    this.DbFirst = new OdbcDB2iSeriesDbFirst(this, this.InternalCommonUtils, this.InternalCommonExpression);
    this.CodeFirst = new OdbcDB2iSeriesCodeFirst(this, this.InternalCommonUtils, this.InternalCommonExpression);

    if (this.Ado.MasterPool != null)
      try {
        using (var conn = this.Ado.MasterPool.Get()) {
          (this.InternalCommonUtils as OdbcDB2iSeriesUtils).ServerVersion = int.Parse(conn.Value.ServerVersion.Split('.')[0]);
        }
      } catch {
      }
  }

  ~OdbcDB2iSeriesProvider() => this.Dispose();
  int _disposeCounter;
  public override void Dispose() {
    if (Interlocked.Increment(ref _disposeCounter) != 1) return;
    (this.Ado as AdoProvider)?.Dispose();
  }
}

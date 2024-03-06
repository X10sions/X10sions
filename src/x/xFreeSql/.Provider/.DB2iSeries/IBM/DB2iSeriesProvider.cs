using FreeSql.DB2iSeries.Curd;
using FreeSql.Internal.CommonProvider;
using System.Data.Common;

namespace FreeSql.DB2iSeries;

public class DB2iSeriesProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {
  static DB2iSeriesProvider() {
    Select0Provider._dicMethodDataReaderGetValue[typeof(Guid)] = typeof(DbDataReader).GetMethod("GetGuid", new Type[] { typeof(int) });
  }

  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new DB2iSeriesSelect<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new DB2iSeriesInsert<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new DB2iSeriesUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new DB2iSeriesDelete<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new DB2iSeriesInsertOrUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);

  public DB2iSeriesProvider(string masterConnectionString, string[] slaveConnectionString, Func<DbConnection> connectionFactory = null) {
    this.InternalCommonUtils = new DB2iSeriesUtils(this);
    this.InternalCommonExpression = new DB2iSeriesExpression(this.InternalCommonUtils);

    this.Ado = new DB2iSeriesAdo(this.InternalCommonUtils, masterConnectionString, slaveConnectionString, connectionFactory);
    this.Aop = new AopProvider();

    this.CodeFirst = new DB2iSeriesCodeFirst(this, this.InternalCommonUtils, this.InternalCommonExpression);
    this.DbFirst = new DB2iSeriesDbFirst(this, this.InternalCommonUtils, this.InternalCommonExpression);
    if (connectionFactory != null) this.CodeFirst.IsNoneCommandParameter = true;
  }

  ~DB2iSeriesProvider() => this.Dispose();
  int _disposeCounter;
  public override void Dispose() {
    if (Interlocked.Increment(ref _disposeCounter) != 1) return;
    (this.Ado as AdoProvider)?.Dispose();
  }
}

using FreeSql.Internal.CommonProvider;
using FreeSql.Odbc.DB2iSeries;

namespace FreeSql.DB2iSeries;

public class DB2iOleDbProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {

  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new OdbcDB2iSeriesSelect<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new OdbcDB2iSeriesInsert<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new OdbcDB2iSeriesUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new OdbcDB2iSeriesDelete<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new OdbcDB2iSeriesInsertOrUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);



  public override void Dispose() {
    throw new NotImplementedException();
  }
}

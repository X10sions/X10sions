using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using FreeSql.Odbc.DB2iSeries;

namespace FreeSql.DB2iSeries;

public class DB2iOdbcProvider<TMark> : BaseDbProvider, IFreeSql<TMark> {

  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new OdbcDB2iSeriesSelect<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new OdbcDB2iSeriesInsert<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new OdbcDB2iSeriesUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new OdbcDB2iSeriesDelete<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression, dywhere);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new OdbcDB2iSeriesInsertOrUpdate<T1>(this, this.InternalCommonUtils, this.InternalCommonExpression);


  public override void Dispose() {
    throw new NotImplementedException();
  }

  #region CURD

  class DB2iOdbcDelete<T1> : DeleteProvider<T1> {
    public DB2iOdbcDelete(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) {
    }
  }

  class DB2iOdbcInsert<T1> : InsertProvider<T1> where T1 : class {
    public DB2iOdbcInsert(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression) : base(orm, commonUtils, commonExpression) {
    }
  }

  class DB2iOdbcInsertOrUpdate<T1> : InsertOrUpdateProvider<T1> where T1 : class {
    public DB2iOdbcInsertOrUpdate(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression) : base(orm, commonUtils, commonExpression) {
    }
  }

  class DB2iOdbcSelect<T1> : Select1Provider<T1> {
    public DB2iOdbcSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  }

  class DB2iOdbcUpdate<T1> : UpdateProvider<T1> {

    public DB2iOdbcUpdate(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) {
    }
  }

  #endregion
}


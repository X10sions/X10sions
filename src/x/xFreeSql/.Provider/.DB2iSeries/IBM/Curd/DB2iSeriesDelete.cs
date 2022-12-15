using FreeSql.Internal;

namespace FreeSql.DB2iSeries.Curd;

class DB2iSeriesDelete<T1> : Internal.CommonProvider.DeleteProvider<T1> {
  public DB2iSeriesDelete(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
      : base(orm, commonUtils, commonExpression, dywhere) {
  }

  public override List<T1> ExecuteDeleted() => throw new NotImplementedException($"FreeSql.Provider.DB2iSeries {CoreStrings.S_Not_Implemented_Feature}");

#if net40
#else
  public override Task<List<T1>> ExecuteDeletedAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException($"FreeSql.Provider.DB2iSeries {CoreStrings.S_Not_Implemented_Feature}");
#endif
}

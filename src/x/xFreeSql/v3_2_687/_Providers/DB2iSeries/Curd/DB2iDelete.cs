using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;

namespace xFreeSql.v3_2_687.DB2iSeries.Curd;
class DB2iDelete<T1> : DeleteProvider<T1> {
  public DB2iDelete(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
    : base(orm, commonUtils, commonExpression, dywhere) { }

  public override List<T1> ExecuteDeleted() => throw new NotImplementedException($"FreeSql.Provider.DB2i {CoreStrings.S_Not_Implemented_Feature}");
  public override Task<List<T1>> ExecuteDeletedAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException($"FreeSql.Provider.DB2i {CoreStrings.S_Not_Implemented_Feature}");

}
using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using FreeSql.Internal.Model;
using System.Text;

namespace xFreeSql.v3_2_687.DB2iSeries.Curd;

class DB2iUpdate<T1> : UpdateProvider<T1> {

  public DB2iUpdate(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
    : base(orm, commonUtils, commonExpression, dywhere) { }

  public override int ExecuteAffrows() => base.SplitExecuteAffrows(_batchRowsLimit > 0 ? _batchRowsLimit : 200, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override Task<int> ExecuteAffrowsAsync(CancellationToken cancellationToken = default) => base.SplitExecuteAffrowsAsync(_batchRowsLimit > 0 ? _batchRowsLimit : 200, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  public override List<T1> ExecuteUpdated() => base.SplitExecuteUpdated(_batchRowsLimit > 0 ? _batchRowsLimit : 200, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override Task<List<T1>> ExecuteUpdatedAsync(CancellationToken cancellationToken = default) => base.SplitExecuteUpdatedAsync(_batchRowsLimit > 0 ? _batchRowsLimit : 200, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  protected override List<T1> RawExecuteUpdated() => throw new NotImplementedException($"FreeSql.Provider.DB2i {CoreStrings.S_Not_Implemented_Feature}");
  protected override Task<List<T1>> RawExecuteUpdatedAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException($"FreeSql.Provider.DB2i {CoreStrings.S_Not_Implemented_Feature}");

  protected override void ToSqlCase(StringBuilder caseWhen, ColumnInfo[] primarys) {
    if (primarys.Length == 1) {
      var pk = primarys.First();
      caseWhen.Append(_commonUtils.RereadColumn(pk, _commonUtils.QuoteSqlName(pk.Attribute.Name)));
      return;
    }
    caseWhen.Append("(");
    var pkidx = 0;
    foreach (var pk in primarys) {
      if (pkidx > 0) caseWhen.Append(" || '+' || ");
      caseWhen.Append(_commonUtils.RereadColumn(pk, _commonUtils.QuoteSqlName(pk.Attribute.Name)));
      ++pkidx;
    }
    caseWhen.Append(")");
  }

  protected override void ToSqlWhen(StringBuilder sb, ColumnInfo[] primarys, object d) {
    if (primarys.Length == 1) {
      if (primarys[0].Attribute.DbType.Contains("xNVARCHAR2"))
        sb.Append("N");
      sb.Append(_commonUtils.FormatSql("{0}", primarys[0].GetDbValue(d)));
      return;
    }
    sb.Append("(");
    var pkidx = 0;
    foreach (var pk in primarys) {
      if (pkidx > 0) sb.Append(" || '+' || ");
      sb.Append(_commonUtils.FormatSql("{0}", pk.GetDbValue(d)));
      ++pkidx;
    }
    sb.Append(")");
  }

}
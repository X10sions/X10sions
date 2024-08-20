using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using FreeSql.Internal.Model;
using System.Linq.Expressions;
using System.Text;

namespace FreeSql.DB2i;
public enum DB2iProviderType { IBM, Odbc, OleDb }

public class DB2iProvider_v3_2_833<TMark>(DB2iProviderType providerType = DB2iProviderType.Odbc) : BaseDbProvider, IFreeSql<TMark> {
  public override IDelete<T1> CreateDeleteProvider<T1>(object dywhere) => new DB2iDelete<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);
  public override IInsert<T1> CreateInsertProvider<T1>() => new DB2iInsert<T1>(this, InternalCommonUtils, InternalCommonExpression);
  public override IInsertOrUpdate<T1> CreateInsertOrUpdateProvider<T1>() => new DB2iInsertOrUpdate<T1>(this, InternalCommonUtils, InternalCommonExpression);
  public override ISelect<T1> CreateSelectProvider<T1>(object dywhere) => new DB2iSelect<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);
  public override IUpdate<T1> CreateUpdateProvider<T1>(object dywhere) => new DB2iUpdate<T1>(this, InternalCommonUtils, InternalCommonExpression, dywhere);

  ~DB2iProvider_v3_2_833() => Dispose();
  int _disposeCounter;

  public override void Dispose() {
    if (Interlocked.Increment(ref _disposeCounter) != 1) return;
    (Ado as AdoProvider)?.Dispose();
  }

}

class DB2iDelete<T1>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : DeleteProvider<T1>(orm, commonUtils, commonExpression, dywhere) {
  public override List<T1> ExecuteDeleted() => throw new NotImplementedException($"FreeSql.DB2i {CoreStrings.S_Not_Implemented_Feature}");
  public override Task<List<T1>> ExecuteDeletedAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException($"FreeSql.DB2i {CoreStrings.S_Not_Implemented_Feature}");
}

class DB2iInsert<T1>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression) : InsertProvider<T1>(orm, commonUtils, commonExpression) where T1 : class {
  public const int DefaultValuesLimit = 500;
  public const int DefaultParameterLimit = 999;
  public override int ExecuteAffrows() => base.SplitExecuteAffrows(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit);
  public override Task<int> ExecuteAffrowsAsync(CancellationToken cancellationToken = default) => SplitExecuteAffrowsAsync(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, cancellationToken);
  public override long ExecuteIdentity() => SplitExecuteIdentity(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit);
  public override Task<long> ExecuteIdentityAsync(CancellationToken cancellationToken = default) => SplitExecuteIdentityAsync(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, cancellationToken);
  public override List<T1> ExecuteInserted() => SplitExecuteInserted(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit);
  public override Task<List<T1>> ExecuteInsertedAsync(CancellationToken cancellationToken = default) => SplitExecuteInsertedAsync(_batchValuesLimit > 0 ? _batchValuesLimit : DefaultValuesLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, cancellationToken);

  protected override long RawExecuteIdentity() => throw new NotImplementedException();
  protected override Task<long> RawExecuteIdentityAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
  protected override List<T1> RawExecuteInserted() => throw new NotImplementedException();
  protected override Task<List<T1>> RawExecuteInsertedAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}

class DB2iInsertOrUpdate<T1>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression) : InsertOrUpdateProvider<T1>(orm, commonUtils, commonExpression) where T1 : class {
  public override string ToSql() {
    throw new NotImplementedException();
  }
}

class DB2iSelect<T1>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : Select1Provider<T1>(orm, commonUtils, commonExpression, dywhere) {

  public override ISelect<T1, T2> From<T2>(Expression<Func<ISelectFromExpression<T1>, T2, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3> From<T2, T3>(Expression<Func<ISelectFromExpression<T1>, T2, T3, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4> From<T2, T3, T4>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5> From<T2, T3, T4, T5>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6> From<T2, T3, T4, T5, T6>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7> From<T2, T3, T4, T5, T6, T7>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8> From<T2, T3, T4, T5, T6, T7, T8>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9> From<T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> From<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(_orm, _commonUtils, _commonExpression, null); CopyData(this, ret, exp?.Parameters); return ret; }
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select2Provider<T1, T2>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select3Provider<T1, T2, T3>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select4Provider<T1, T2, T3, T4>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select5Provider<T1, T2, T3, T4, T5>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select6Provider<T1, T2, T3, T4, T5, T6>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select7Provider<T1, T2, T3, T4, T5, T6, T7>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select8Provider<T1, T2, T3, T4, T5, T6, T7, T8>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select9Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select10Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select11Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select12Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select13Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select14Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select15Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere)
  : Select16Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(orm, commonUtils, commonExpression, dywhere)
  where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class where T16 : class {
  public override string ToSql(string? field = null) => _commonUtils.ToDB2iSqlSelect(_commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}


class DB2iUpdate<T1>(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : UpdateProvider<T1>(orm, commonUtils, commonExpression, dywhere) {
  public const int DefaultRowLimit = 200;
  public const int DefaultParameterLimit = 999;

  public override int ExecuteAffrows() => SplitExecuteAffrows(_batchRowsLimit > 0 ? _batchRowsLimit : DefaultRowLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit);
  public override Task<int> ExecuteAffrowsAsync(CancellationToken cancellationToken = default) => SplitExecuteAffrowsAsync(_batchRowsLimit > 0 ? _batchRowsLimit : DefaultRowLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, cancellationToken);
  protected override List<TReturn> ExecuteUpdated<TReturn>(IEnumerable<ColumnInfo> columns) => SplitExecuteUpdated<TReturn>(_batchRowsLimit > 0 ? _batchRowsLimit : DefaultRowLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, columns);
  protected override Task<List<TReturn>> ExecuteUpdatedAsync<TReturn>(IEnumerable<ColumnInfo> columns, CancellationToken cancellationToken = default) => SplitExecuteUpdatedAsync<TReturn>(_batchRowsLimit > 0 ? _batchRowsLimit : DefaultRowLimit, _batchParameterLimit > 0 ? _batchParameterLimit : DefaultParameterLimit, columns, cancellationToken);

  protected override List<TReturn> RawExecuteUpdated<TReturn>(IEnumerable<ColumnInfo> columns) => throw new NotImplementedException();
  protected override Task<List<TReturn>> RawExecuteUpdatedAsync<TReturn>(IEnumerable<ColumnInfo> columns, CancellationToken cancellationToken = default) => throw new NotImplementedException();
  protected override void ToSqlCase(StringBuilder caseWhen, ColumnInfo[] primarys) => throw new NotImplementedException();
  protected override void ToSqlWhen(StringBuilder sb, ColumnInfo[] primarys, object d) => throw new NotImplementedException();
}

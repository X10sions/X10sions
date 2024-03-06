﻿using FreeSql.Internal;
using System.Data;
using System.Text;

namespace FreeSql.DB2iSeries.Curd;

class DB2iSeriesInsert<T1> : Internal.CommonProvider.InsertProvider<T1> where T1 : class {
  public DB2iSeriesInsert(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression)
      : base(orm, commonUtils, commonExpression) {
  }

  public override int ExecuteAffrows() => base.SplitExecuteAffrows(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override long ExecuteIdentity() => base.SplitExecuteIdentity(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override List<T1> ExecuteInserted() => base.SplitExecuteInserted(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999);

  protected override long RawExecuteIdentity() {
    var sql = this.ToSql();
    if (string.IsNullOrEmpty(sql)) return 0;

    sql = string.Concat(sql, "; SELECT last_insert_rowid();");
    var before = new Aop.CurdBeforeEventArgs(_table.Type, _table, Aop.CurdType.Insert, sql, _params);
    _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
    long ret = 0;
    Exception exception = null;
    try {
      long.TryParse(string.Concat(_orm.Ado.ExecuteScalar(_connection, _transaction, CommandType.Text, sql, _commandTimeout, _params)), out ret);
    } catch (Exception ex) {
      exception = ex;
      throw;
    } finally {
      var after = new Aop.CurdAfterEventArgs(before, exception, ret);
      _orm.Aop.CurdAfterHandler?.Invoke(this, after);
    }
    return ret;
  }

  protected override List<T1> RawExecuteInserted() {
    var sql = this.ToSql();
    if (string.IsNullOrEmpty(sql)) return new List<T1>();

    var ret = _source.ToList();
    this.RawExecuteAffrows();
    return ret;
  }

  public override string ToSql() {
    if (_table.Columns.Count == 1 && _table.ColumnsByPosition[0].Attribute.IsIdentity) {
      var sb = new StringBuilder();
      var didx = 0;
      foreach (var d in _source) {
        if (didx++ > 0) sb.Append(";\r\n");
        sb.Append("INSERT INTO ").Append(_commonUtils.QuoteSqlName(TableRuleInvoke())).Append(" DEFAULT VALUES");
      }
      return sb.ToString();
    }
    return base.ToSql();
  }

#if net40
#else
  public override Task<int> ExecuteAffrowsAsync(CancellationToken cancellationToken = default) => base.SplitExecuteAffrowsAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  public override Task<long> ExecuteIdentityAsync(CancellationToken cancellationToken = default) => base.SplitExecuteIdentityAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  public override Task<List<T1>> ExecuteInsertedAsync(CancellationToken cancellationToken = default) => base.SplitExecuteInsertedAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 5000, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);

  async protected override Task<long> RawExecuteIdentityAsync(CancellationToken cancellationToken = default) {
    var sql = this.ToSql();
    if (string.IsNullOrEmpty(sql)) return 0;

    sql = string.Concat(sql, "; SELECT last_insert_rowid();");
    var before = new Aop.CurdBeforeEventArgs(_table.Type, _table, Aop.CurdType.Insert, sql, _params);
    _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
    long ret = 0;
    Exception exception = null;
    try {
      long.TryParse(string.Concat(await _orm.Ado.ExecuteScalarAsync(_connection, _transaction, CommandType.Text, sql, _commandTimeout, _params, cancellationToken)), out ret);
    } catch (Exception ex) {
      exception = ex;
      throw;
    } finally {
      var after = new Aop.CurdAfterEventArgs(before, exception, ret);
      _orm.Aop.CurdAfterHandler?.Invoke(this, after);
    }
    return ret;
  }
  async protected override Task<List<T1>> RawExecuteInsertedAsync(CancellationToken cancellationToken = default) {
    var sql = this.ToSql();
    if (string.IsNullOrEmpty(sql)) return new List<T1>();

    var ret = _source.ToList();
    await this.RawExecuteAffrowsAsync(cancellationToken);
    return ret;
  }
#endif
}

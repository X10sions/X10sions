﻿using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using FreeSql.Internal.Model;
//#if oledb
//using System.Data.OleDb;
//#endif
using System.Data;
using System.Data.Common;
using System.Text;

namespace xFreeSql.v3_2_687.DB2iSeries.Curd;

class DB2iInsert<T1> : InsertProvider<T1> where T1 : class {
  public DB2iInsert(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression)
      : base(orm, commonUtils, commonExpression) {
  }
  ColumnInfo? _identCol;
  internal IFreeSql InternalOrm => _orm;
  internal DbConnection InternalConnection => _connection;
  internal DbTransaction InternalTransaction => _transaction;

  public override int ExecuteAffrows() => base.SplitExecuteAffrows(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override Task<int> ExecuteAffrowsAsync(CancellationToken cancellationToken = default) => base.SplitExecuteAffrowsAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  public override long ExecuteIdentity() => base.SplitExecuteIdentity(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override Task<long> ExecuteIdentityAsync(CancellationToken cancellationToken = default) => base.SplitExecuteIdentityAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  public override List<T1> ExecuteInserted() => base.SplitExecuteInserted(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999);
  public override Task<List<T1>> ExecuteInsertedAsync(CancellationToken cancellationToken = default) => base.SplitExecuteInsertedAsync(_batchValuesLimit > 0 ? _batchValuesLimit : 500, _batchParameterLimit > 0 ? _batchParameterLimit : 999, cancellationToken);
  protected override long RawExecuteIdentity() {
    var sql = ToSql();
    if (string.IsNullOrEmpty(sql)) return 0;

    long ret = 0;
    Exception? exception = null;
    FreeSql.Aop.CurdBeforeEventArgs? before = null;

    if (_identCol == null || _source.Count > 1) {
      before = new FreeSql.Aop.CurdBeforeEventArgs(_table.Type, _table, FreeSql.Aop.CurdType.Insert, sql, _params);
      _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
      try {
        ret = _orm.Ado.ExecuteNonQuery(_connection, _transaction, CommandType.Text, sql, _commandTimeout, _params);
      } catch (Exception ex) {
        exception = ex;
        throw;
      } finally {
        var after = new FreeSql.Aop.CurdAfterEventArgs(before, exception, ret);
        _orm.Aop.CurdAfterHandler?.Invoke(this, after);
      }
      return 0;
    }
    var identColName = _commonUtils.QuoteSqlName(_identCol.Attribute.Name);
    var identParam = _commonUtils.AppendParamter(null, $"{_identCol.CsName}99", _identCol, _identCol.Attribute.MapType, 0);
    identParam.Direction = ParameterDirection.Output;
    sql = $"{sql} RETURNING {identColName} INTO {identParam.ParameterName}";
    var dbParms = _params.Concat(new[] { identParam }).ToArray();
    before = new FreeSql.Aop.CurdBeforeEventArgs(_table.Type, _table, FreeSql.Aop.CurdType.Insert, sql, dbParms);
    _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
    try {
      _orm.Ado.ExecuteNonQuery(_connection, _transaction, CommandType.Text, sql, _commandTimeout, dbParms);
      long.TryParse(string.Concat(identParam.Value), out ret);
    } catch (Exception ex) {
      exception = ex;
      throw;
    } finally {
      var after = new FreeSql.Aop.CurdAfterEventArgs(before, exception, ret);
      _orm.Aop.CurdAfterHandler?.Invoke(this, after);
    }
    return ret;
  }
  async protected override Task<long> RawExecuteIdentityAsync(CancellationToken cancellationToken = default) {
    var sql = ToSql();
    if (string.IsNullOrEmpty(sql)) return 0;

    long ret = 0;
    Exception? exception = null;
    FreeSql.Aop.CurdBeforeEventArgs? before = null;

    if (_identCol == null || _source.Count > 1) {
      before = new FreeSql.Aop.CurdBeforeEventArgs(_table.Type, _table, FreeSql.Aop.CurdType.Insert, sql, _params);
      _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
      try {
        ret = await _orm.Ado.ExecuteNonQueryAsync(_connection, _transaction, CommandType.Text, sql, _commandTimeout, _params, cancellationToken);
      } catch (Exception ex) {
        exception = ex;
        throw;
      } finally {
        var after = new FreeSql.Aop.CurdAfterEventArgs(before, exception, ret);
        _orm.Aop.CurdAfterHandler?.Invoke(this, after);
      }
      return 0;
    }
    var identColName = _commonUtils.QuoteSqlName(_identCol.Attribute.Name);
    var identParam = _commonUtils.AppendParamter(null, $"{_identCol.CsName}99", _identCol, _identCol.Attribute.MapType, 0);
    identParam.Direction = ParameterDirection.Output;
    sql = $"{sql} RETURNING {identColName} INTO {identParam.ParameterName}";
    var dbParms = _params.Concat(new[] { identParam }).ToArray();
    before = new FreeSql.Aop.CurdBeforeEventArgs(_table.Type, _table, FreeSql.Aop.CurdType.Insert, sql, dbParms);
    _orm.Aop.CurdBeforeHandler?.Invoke(this, before);
    try {
      await _orm.Ado.ExecuteNonQueryAsync(_connection, _transaction, CommandType.Text, sql, _commandTimeout, dbParms, cancellationToken);
      long.TryParse(string.Concat(identParam.Value), out ret);
    } catch (Exception ex) {
      exception = ex;
      throw;
    } finally {
      var after = new FreeSql.Aop.CurdAfterEventArgs(before, exception, ret);
      _orm.Aop.CurdAfterHandler?.Invoke(this, after);
    }
    return ret;
  }

  protected override List<T1> RawExecuteInserted() {
    var sql = ToSql();
    if (string.IsNullOrEmpty(sql)) return new List<T1>();

    var ret = _source.ToList();
    RawExecuteAffrows();
    return ret;
  }
  async protected override Task<List<T1>> RawExecuteInsertedAsync(CancellationToken cancellationToken = default) {
    var sql = ToSql();
    if (string.IsNullOrEmpty(sql)) return new List<T1>();

    var ret = _source.ToList();
    await RawExecuteAffrowsAsync(cancellationToken);
    return ret;
  }

  public string? ToSqlBatchIdentityColumn() {
    if (_source == null || _source.Any() == false) return null;
    var cols = new List<ColumnInfo>();
    foreach (var col in _table.Columns.Values) {
      if (col.Attribute.IsIdentity && _insertIdentity == false && string.IsNullOrEmpty(col.DbInsertValue)) continue;
      if (col.Attribute.IsIdentity == false && _ignore.ContainsKey(col.Attribute.Name)) continue;
      cols.Add(col);
    }
    _identCol = null;
    var sb = new StringBuilder();
    var tmpsb = new StringBuilder();
    sb.Append("INSERT INTO ").Append(_commonUtils.QuoteSqlName(TableRuleInvoke())).Append("(");
    var colidx = 0;
    foreach (var col in cols) {
      if (col.Attribute.IsIdentity) _identCol = col;
      if (colidx > 0) {
        sb.Append(", ");
        tmpsb.Append(", ");
      }
      var colname = _commonUtils.QuoteSqlName(col.Attribute.Name);
      sb.Append(colname);
      tmpsb.Append(col.Attribute.IsIdentity && !string.IsNullOrEmpty(col.DbInsertValue) ? col.DbInsertValue : colname);
      ++colidx;
    }
    sb.Append(") ").Append("\r\nSELECT ").Append(tmpsb.ToString()).Append(" FROM \r\n(\r\n");
    tmpsb.Clear();

    _params = _noneParameter ? new DbParameter[0] : new DbParameter[colidx * _source.Count];
    var specialParams = new List<DbParameter>();
    var didx = 0;
    foreach (var d in _source) {
      if (_source.Count > 1) sb.Append("\r\n  UNION ALL\r\n ");
      sb.Append("  SELECT ");
      var colidx2 = 0;
      foreach (var col in cols) {
        if (colidx2 > 0) sb.Append(", ");
        if (string.IsNullOrEmpty(col.DbInsertValue) == false)
          sb.Append(col.DbInsertValue);
        else {
          var val = col.GetDbValue(d);
          if (val == null && col.Attribute.IsNullable == false) val = col.CsType == typeof(string) ? "" : Utils.GetDataReaderValue(col.CsType.NullableTypeOrThis(), null);//#384

          var colsql = _noneParameter ? _commonUtils.GetNoneParamaterSqlValue(specialParams, _noneParameterFlag, col, col.Attribute.MapType, val) :
              _commonUtils.QuoteWriteParamterAdapter(col.Attribute.MapType, _commonUtils.QuoteParamterName($"{col.CsName}_{didx}"));
          sb.Append(_commonUtils.RewriteColumn(col, colsql));
          if (_noneParameter == false)
            _params[didx * colidx + colidx2] = _commonUtils.AppendParamter(null, $"{col.CsName}_{didx}", col, col.Attribute.MapType, val);
        }
        if (didx == 0) sb.Append(" as ").Append(col.Attribute.Name);
        ++colidx2;
      }
      sb.Append(" FROM dual ");
      ++didx;
    }
    sb.Append(")");
    if (_noneParameter && specialParams.Any()) _params = specialParams.ToArray();
    return sb.ToString();
  }
  public override string? ToSql() {
    if (_source == null || _source.Any() == false) return null;
    var sb = new StringBuilder();
    sb.Append("INSERT ");
    if (_source.Count > 1) sb.Append("ALL");

    _identCol = null;
    var sbtb = new StringBuilder();
    sbtb.Append("INTO ");
    sbtb.Append(_commonUtils.QuoteSqlName(TableRuleInvoke())).Append("(");
    var colidx = 0;
    foreach (var col in _table.Columns.Values) {
      if (col.Attribute.IsIdentity) _identCol = col;
      if (col.Attribute.IsIdentity && _insertIdentity == false && string.IsNullOrEmpty(col.DbInsertValue)) continue;
      if (col.Attribute.IsIdentity == false && _ignore.ContainsKey(col.Attribute.Name)) continue;

      if (colidx > 0) sbtb.Append(", ");
      sbtb.Append(_commonUtils.QuoteSqlName(col.Attribute.Name));
      ++colidx;
    }
    sbtb.Append(") ");

    _params = _noneParameter ? new DbParameter[0] : new DbParameter[colidx * _source.Count];
    var specialParams = new List<DbParameter>();
    var didx = 0;
    foreach (var d in _source) {
      if (_source.Count > 1) sb.Append("\r\n");
      sb.Append(sbtb);
      sb.Append("VALUES");
      sb.Append("(");
      var colidx2 = 0;
      foreach (var col in _table.Columns.Values) {
        if (col.Attribute.IsIdentity && _insertIdentity == false && string.IsNullOrEmpty(col.DbInsertValue)) continue;
        if (col.Attribute.IsIdentity == false && _ignore.ContainsKey(col.Attribute.Name)) continue;

        if (colidx2 > 0) sb.Append(", ");
        if (string.IsNullOrEmpty(col.DbInsertValue) == false)
          sb.Append(col.DbInsertValue);
        else {
          var val = col.GetDbValue(d);
          if (val == null && col.Attribute.IsNullable == false) val = col.CsType == typeof(string) ? "" : Utils.GetDataReaderValue(col.CsType.NullableTypeOrThis(), null);//#384

          var colsql = _noneParameter ? _commonUtils.GetNoneParamaterSqlValue(specialParams, _noneParameterFlag, col, col.Attribute.MapType, val) :
              _commonUtils.QuoteWriteParamterAdapter(col.Attribute.MapType, _commonUtils.QuoteParamterName($"{col.CsName}_{didx}"));
          sb.Append(_commonUtils.RewriteColumn(col, colsql));
          if (_noneParameter == false)
            _params[didx * colidx + colidx2] = _commonUtils.AppendParamter(null, $"{col.CsName}_{didx}", col, col.Attribute.MapType, val);
        }
        ++colidx2;
      }
      sb.Append(")");
      ++didx;
    }
    if (_noneParameter && specialParams.Any()) _params = specialParams.ToArray();
    if (_source.Count > 1) sb.Append("\r\n SELECT 1 FROM DUAL");
    return sb.ToString();
  }
}
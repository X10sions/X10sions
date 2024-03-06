﻿using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using FreeSql.Internal.Model;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
namespace FreeSql.Odbc.DB2iSeries;

class DB2iSeriesSelect<T1> : Select1Provider<T1> {

  internal static string ToSqlStatic(CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm) {
    if (_orm.CodeFirst.IsAutoSyncStructure)
      _orm.CodeFirst.SyncStructure(_tables.Select(a => a.Table.Type).ToArray());

    if (_whereGlobalFilter.Any())
      foreach (var tb in _tables.Where(a => a.Type != SelectTableInfoType.Parent))
        tb.Cascade = _commonExpression.GetWhereCascadeSql(tb, _whereGlobalFilter, true);

    var sb = new StringBuilder();
    var sbunion = new StringBuilder();
    var sbnav = new StringBuilder();
    var tbUnionsGt0 = tbUnions.Count > 1;
    for (var tbUnionsIdx = 0; tbUnionsIdx < tbUnions.Count; tbUnionsIdx++) {
      if (tbUnionsIdx > 0) sb.Append("\r\n \r\nUNION ALL\r\n \r\n");
      var tbUnion = tbUnions[tbUnionsIdx];

      sbunion.Append(_select);
      if (_distinct) sbunion.Append("DISTINCT ");
      sbunion.Append(field);
      if (string.IsNullOrEmpty(_orderby) && _skip > 0) sbunion.Append(", ROWNUM AS \"__rownum__\"");
      sbunion.Append(" \r\nFROM ");
      var tbsjoin = _tables.Where(a => a.Type != SelectTableInfoType.From).ToArray();
      var tbsfrom = _tables.Where(a => a.Type == SelectTableInfoType.From).ToArray();
      for (var a = 0; a < tbsfrom.Length; a++) {
        sbunion.Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[a].Table.Type])).Append(" ").Append(_aliasRule?.Invoke(tbsfrom[a].Table.Type, tbsfrom[a].Alias) ?? tbsfrom[a].Alias);
        if (tbsjoin.Length > 0) {
          //如果存在 join 查询，则处理 from t1, t2 改为 from t1 inner join t2 on 1 = 1
          for (var b = 1; b < tbsfrom.Length; b++) {
            sbunion.Append(" \r\nLEFT JOIN ").Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[b].Table.Type])).Append(" ").Append(_aliasRule?.Invoke(tbsfrom[b].Table.Type, tbsfrom[b].Alias) ?? tbsfrom[b].Alias);

            if (string.IsNullOrEmpty(tbsfrom[b].NavigateCondition) && string.IsNullOrEmpty(tbsfrom[b].On) && string.IsNullOrEmpty(tbsfrom[b].Cascade)) sbunion.Append(" ON 1 = 1");
            else {
              var onSql = tbsfrom[b].NavigateCondition ?? tbsfrom[b].On;
              sbunion.Append(" ON ").Append(onSql);
              if (string.IsNullOrEmpty(tbsfrom[b].Cascade) == false) {
                if (string.IsNullOrEmpty(onSql)) sbunion.Append(tbsfrom[b].Cascade);
                else sbunion.Append(" AND ").Append(tbsfrom[b].Cascade);
              }
            }
          }
          break;
        } else {
          if (!string.IsNullOrEmpty(tbsfrom[a].NavigateCondition)) sbnav.Append(" AND (").Append(tbsfrom[a].NavigateCondition).Append(")");
          if (!string.IsNullOrEmpty(tbsfrom[a].On)) sbnav.Append(" AND (").Append(tbsfrom[a].On).Append(")");
          if (a > 0 && !string.IsNullOrEmpty(tbsfrom[a].Cascade)) sbnav.Append(" AND ").Append(tbsfrom[a].Cascade);
        }
        if (a < tbsfrom.Length - 1) sbunion.Append(", ");
      }
      foreach (var tb in tbsjoin) {
        switch (tb.Type) {
          case SelectTableInfoType.Parent:
          case SelectTableInfoType.RawJoin:
            continue;
          case SelectTableInfoType.LeftJoin:
            sbunion.Append(" \r\nLEFT JOIN ");
            break;
          case SelectTableInfoType.InnerJoin:
            sbunion.Append(" \r\nINNER JOIN ");
            break;
          case SelectTableInfoType.RightJoin:
            sbunion.Append(" \r\nRIGHT JOIN ");
            break;
        }
        sbunion.Append(_commonUtils.QuoteSqlName(tbUnion[tb.Table.Type])).Append(" ").Append(_aliasRule?.Invoke(tb.Table.Type, tb.Alias) ?? tb.Alias).Append(" ON ").Append(tb.On ?? tb.NavigateCondition);
        if (!string.IsNullOrEmpty(tb.Cascade)) sbunion.Append(" AND ").Append(tb.Cascade);
        if (!string.IsNullOrEmpty(tb.On) && !string.IsNullOrEmpty(tb.NavigateCondition)) sbnav.Append(" AND (").Append(tb.NavigateCondition).Append(")");
      }
      if (_join.Length > 0) sbunion.Append(_join);

      sbnav.Append(_where);
      if (!string.IsNullOrEmpty(_tables[0].Cascade))
        sbnav.Append(" AND ").Append(_tables[0].Cascade);

      if (string.IsNullOrEmpty(_orderby) && (_skip > 0 || _limit > 0))
        sbnav.Append(" AND ROWNUM < ").Append(_skip + _limit + 1);
      if (sbnav.Length > 0)
        sbunion.Append(" \r\nWHERE ").Append(sbnav.Remove(0, 5));
      if (string.IsNullOrEmpty(_groupby) == false) {
        sbunion.Append(_groupby);
        if (string.IsNullOrEmpty(_having) == false)
          sbunion.Append(" \r\nHAVING ").Append(_having.Substring(5));
      }
      sbunion.Append(_orderby);

      if (string.IsNullOrEmpty(_orderby)) {
        if (_skip > 0)
          sbunion.Insert(0, $"{_select}t.* FROM (").Append(") t WHERE t.\"__rownum__\" > ").Append(_skip);
      } else {
        if (_skip > 0 && _limit > 0) sbunion.Insert(0, $"{_select}t.* FROM (SELECT rt.*, ROWNUM AS \"__rownum__\" FROM (").Append(") rt WHERE ROWNUM < ").Append(_skip + _limit + 1).Append(") t WHERE t.\"__rownum__\" > ").Append(_skip);
        else if (_skip > 0) sbunion.Insert(0, $"{_select}t.* FROM (").Append(") t WHERE ROWNUM > ").Append(_skip);
        else if (_limit > 0) sbunion.Insert(0, $"{_select}t.* FROM (").Append(") t WHERE ROWNUM < ").Append(_limit + 1);
      }

      if (tbUnionsGt0) sbunion.Insert(0, $"{_select}* from (").Append(") ftb");
      sb.Append(sbunion);
      sbnav.Clear();
      sbunion.Clear();
    }
    var sql = sb.Append(_tosqlAppendContent).ToString();

    var aliasGreater30 = 0;
    foreach (var tb in _tables)
      if (tb.Alias.Length > 30) sql = sql.Replace(tb.Alias, $"than30_{aliasGreater30++}");

    return sql;
  }

  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override ISelect<T1, T2> From<T2>(Expression<Func<ISelectFromExpression<T1>, T2, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3> From<T2, T3>(Expression<Func<ISelectFromExpression<T1>, T2, T3, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4> From<T2, T3, T4>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5> From<T2, T3, T4, T5>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6> From<T2, T3, T4, T5, T6>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7> From<T2, T3, T4, T5, T6, T7>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8> From<T2, T3, T4, T5, T6, T7, T8>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9> From<T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> From<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }

  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(_orm, _commonUtils, _commonExpression, null); DB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override string ToSql(string field = null) => ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2> : FreeSql.Internal.CommonProvider.Select2Provider<T1, T2> where T2 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3> : FreeSql.Internal.CommonProvider.Select3Provider<T1, T2, T3> where T2 : class where T3 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4> : FreeSql.Internal.CommonProvider.Select4Provider<T1, T2, T3, T4> where T2 : class where T3 : class where T4 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5> : FreeSql.Internal.CommonProvider.Select5Provider<T1, T2, T3, T4, T5> where T2 : class where T3 : class where T4 : class where T5 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6> : FreeSql.Internal.CommonProvider.Select6Provider<T1, T2, T3, T4, T5, T6> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7> : FreeSql.Internal.CommonProvider.Select7Provider<T1, T2, T3, T4, T5, T6, T7> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8> : FreeSql.Internal.CommonProvider.Select8Provider<T1, T2, T3, T4, T5, T6, T7, T8> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9> : FreeSql.Internal.CommonProvider.Select9Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : FreeSql.Internal.CommonProvider.Select10Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : FreeSql.Internal.CommonProvider.Select11Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : FreeSql.Internal.CommonProvider.Select12Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : FreeSql.Internal.CommonProvider.Select13Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : FreeSql.Internal.CommonProvider.Select14Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : FreeSql.Internal.CommonProvider.Select15Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class DB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : FreeSql.Internal.CommonProvider.Select16Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class where T16 : class {
  public DB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string field = null) => DB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class OdbcDB2iSeriesSelect<T1> : FreeSql.Internal.CommonProvider.Select1Provider<T1> {

  internal static string ToSqlStatic(CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm)
      => (_commonUtils as OdbcDB2iSeriesUtils).IsSelectRowNumber ?
      ToSqlStaticRowNumber(_commonUtils, _commonExpression, _select, _distinct, field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, tbUnions, _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm) :
      ToSqlStaticOffsetFetchNext(_commonUtils, _commonExpression, _select, _distinct, field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, tbUnions, _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);

  #region DB2iSeries 2005 row_number
  internal static string ToSqlStaticRowNumber(CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm) {
    if (_orm.CodeFirst.IsAutoSyncStructure)
      _orm.CodeFirst.SyncStructure(_tables.Select(a => a.Table.Type).ToArray());

    if (_whereGlobalFilter.Any())
      foreach (var tb in _tables.Where(a => a.Type != SelectTableInfoType.Parent))
        tb.Cascade = _commonExpression.GetWhereCascadeSql(tb, _whereGlobalFilter, true);

    var sb = new StringBuilder();
    var tbUnionsGt0 = tbUnions.Count > 1;
    for (var tbUnionsIdx = 0; tbUnionsIdx < tbUnions.Count; tbUnionsIdx++) {
      if (tbUnionsIdx > 0) sb.Append(" \r\n\r\nUNION ALL\r\n\r\n");
      if (tbUnionsGt0) sb.Append(_select).Append(" * from (");
      var tbUnion = tbUnions[tbUnionsIdx];

      var sbnav = new StringBuilder();
      sb.Append(_select);
      if (_distinct) sb.Append("DISTINCT ");
      //if (_limit > 0) sb.Append("TOP ").Append(_skip + _limit).Append(" "); //TOP 会引发 __rownum__ 无序的问题
      if (_skip <= 0 && _limit > 0) sb.Append("TOP ").Append(_limit).Append(" ");
      sb.Append(field);

      if (_limit > 0 || _skip > 0) {
        if (string.IsNullOrEmpty(_orderby) && (_limit > 1 || _skip > 0))  //TOP 1 不自动 order by
        {
          if (string.IsNullOrEmpty(_groupby)) {
            var pktb = _tables.Where(a => a.Table.Primarys.Any()).FirstOrDefault();
            if (pktb != null) _orderby = string.Concat(" \r\nORDER BY ", pktb.Alias, ".", _commonUtils.QuoteSqlName(pktb?.Table.Primarys.First().Attribute.Name));
            else _orderby = string.Concat(" \r\nORDER BY ", _tables.First().Alias, ".", _commonUtils.QuoteSqlName(_tables.First().Table.Columns.First().Value.Attribute.Name));
          } else
            _orderby = _groupby.Replace("GROUP BY ", "ORDER BY ");
        }
        if (_skip > 0) // 注意这个判断，大于 0 才使用 ROW_NUMBER ，否则属于第一页直接使用 TOP
          sb.Append(", ROW_NUMBER() OVER(").Append(_orderby.Trim('\r', '\n', ' ')).Append(") AS __rownum__");
      }
      sb.Append(" \r\nFROM ");
      var tbsjoin = _tables.Where(a => a.Type != SelectTableInfoType.From).ToArray();
      var tbsfrom = _tables.Where(a => a.Type == SelectTableInfoType.From).ToArray();
      for (var a = 0; a < tbsfrom.Length; a++) {
        var alias = LocalGetTableAlias(tbsfrom[a].Table.Type, tbUnion[tbsfrom[a].Table.Type], tbsfrom[a].Alias, _aliasRule);
        sb.Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[a].Table.Type])).Append(" ").Append(alias);
        if (tbsjoin.Length > 0) {
          //如果存在 join 查询，则处理 from t1, t2 改为 from t1 inner join t2 on 1 = 1
          for (var b = 1; b < tbsfrom.Length; b++) {
            alias = LocalGetTableAlias(tbsfrom[b].Table.Type, tbUnion[tbsfrom[b].Table.Type], tbsfrom[b].Alias, _aliasRule);
            sb.Append(" \r\nLEFT JOIN ").Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[b].Table.Type])).Append(" ").Append(alias);

            if (string.IsNullOrEmpty(tbsfrom[b].NavigateCondition) && string.IsNullOrEmpty(tbsfrom[b].On) && string.IsNullOrEmpty(tbsfrom[b].Cascade)) sb.Append(" ON 1 = 1");
            else {
              var onSql = tbsfrom[b].NavigateCondition ?? tbsfrom[b].On;
              sb.Append(" ON ").Append(onSql);
              if (string.IsNullOrEmpty(tbsfrom[b].Cascade) == false) {
                if (string.IsNullOrEmpty(onSql)) sb.Append(tbsfrom[b].Cascade);
                else sb.Append(" AND ").Append(tbsfrom[b].Cascade);
              }
            }
          }
          break;
        } else {
          if (!string.IsNullOrEmpty(tbsfrom[a].NavigateCondition)) sbnav.Append(" AND (").Append(tbsfrom[a].NavigateCondition).Append(")");
          if (!string.IsNullOrEmpty(tbsfrom[a].On)) sbnav.Append(" AND (").Append(tbsfrom[a].On).Append(")");
          if (a > 0 && !string.IsNullOrEmpty(tbsfrom[a].Cascade)) sbnav.Append(" AND ").Append(tbsfrom[a].Cascade);
        }
        if (a < tbsfrom.Length - 1) sb.Append(", ");
      }
      foreach (var tb in tbsjoin) {
        switch (tb.Type) {
          case SelectTableInfoType.Parent:
          case SelectTableInfoType.RawJoin:
            continue;
          case SelectTableInfoType.LeftJoin:
            sb.Append(" \r\nLEFT JOIN ");
            break;
          case SelectTableInfoType.InnerJoin:
            sb.Append(" \r\nINNER JOIN ");
            break;
          case SelectTableInfoType.RightJoin:
            sb.Append(" \r\nRIGHT JOIN ");
            break;
        }
        var alias = LocalGetTableAlias(tb.Table.Type, tbUnion[tb.Table.Type], tb.Alias, _aliasRule);
        sb.Append(_commonUtils.QuoteSqlName(tbUnion[tb.Table.Type])).Append(" ").Append(alias).Append(" ON ").Append(tb.On ?? tb.NavigateCondition);
        if (!string.IsNullOrEmpty(tb.Cascade)) sb.Append(" AND ").Append(tb.Cascade);
        if (!string.IsNullOrEmpty(tb.On) && !string.IsNullOrEmpty(tb.NavigateCondition)) sbnav.Append(" AND (").Append(tb.NavigateCondition).Append(")");
      }
      if (_join.Length > 0) sb.Append(_join);

      sbnav.Append(_where);
      if (!string.IsNullOrEmpty(_tables[0].Cascade))
        sbnav.Append(" AND ").Append(_tables[0].Cascade);

      if (sbnav.Length > 0) {
        sb.Append(" \r\nWHERE ").Append(sbnav.Remove(0, 5));
      }
      if (string.IsNullOrEmpty(_groupby) == false) {
        sb.Append(_groupby);
        if (string.IsNullOrEmpty(_having) == false)
          sb.Append(" \r\nHAVING ").Append(_having.Substring(5));
      }
      if (_skip <= 0)
        sb.Append(_orderby);
      else {
        sb.Insert(0, "WITH t AS ( ").Append(" ) SELECT t.* FROM t where __rownum__");
        if (_limit > 0)
          sb.Append(" between ").Append(_skip + 1).Append(" and ").Append(_skip + _limit);
        else
          sb.Append(" > ").Append(_skip);
      }

      sbnav.Clear();
      if (tbUnionsGt0) sb.Append(") ftb");
    }
    return sb.Append(_tosqlAppendContent).ToString();
  }
  #endregion

  #region DB2iSeries 2012+ offset feach next
  internal static string ToSqlStaticOffsetFetchNext(CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm) {
    if (_orm.CodeFirst.IsAutoSyncStructure)
      _orm.CodeFirst.SyncStructure(_tables.Select(a => a.Table.Type).ToArray());

    if (_whereGlobalFilter.Any())
      foreach (var tb in _tables.Where(a => a.Type != SelectTableInfoType.Parent))
        tb.Cascade = _commonExpression.GetWhereCascadeSql(tb, _whereGlobalFilter, true);

    var sb = new StringBuilder();
    var tbUnionsGt0 = tbUnions.Count > 1;
    for (var tbUnionsIdx = 0; tbUnionsIdx < tbUnions.Count; tbUnionsIdx++) {
      if (tbUnionsIdx > 0) sb.Append("\r\n \r\nUNION ALL\r\n \r\n");
      if (tbUnionsGt0) sb.Append(_select).Append(" * from (");
      var tbUnion = tbUnions[tbUnionsIdx];

      var sbnav = new StringBuilder();
      sb.Append(_select);
      if (_distinct) sb.Append("DISTINCT ");
      if (_skip <= 0 && _limit > 0) sb.Append("TOP ").Append(_limit).Append(" ");
      sb.Append(field);
      sb.Append(" \r\nFROM ");
      var tbsjoin = _tables.Where(a => a.Type != SelectTableInfoType.From).ToArray();
      var tbsfrom = _tables.Where(a => a.Type == SelectTableInfoType.From).ToArray();
      for (var a = 0; a < tbsfrom.Length; a++) {
        var alias = LocalGetTableAlias(tbsfrom[a].Table.Type, tbUnion[tbsfrom[a].Table.Type], tbsfrom[a].Alias, _aliasRule);
        sb.Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[a].Table.Type])).Append(" ").Append(alias);
        if (tbsjoin.Length > 0) {
          //如果存在 join 查询，则处理 from t1, t2 改为 from t1 inner join t2 on 1 = 1
          for (var b = 1; b < tbsfrom.Length; b++) {
            alias = LocalGetTableAlias(tbsfrom[b].Table.Type, tbUnion[tbsfrom[b].Table.Type], tbsfrom[b].Alias, _aliasRule);
            sb.Append(" \r\nLEFT JOIN ").Append(_commonUtils.QuoteSqlName(tbUnion[tbsfrom[b].Table.Type])).Append(" ").Append(alias);

            if (string.IsNullOrEmpty(tbsfrom[b].NavigateCondition) && string.IsNullOrEmpty(tbsfrom[b].On) && string.IsNullOrEmpty(tbsfrom[b].Cascade)) sb.Append(" ON 1 = 1");
            else {
              var onSql = tbsfrom[b].NavigateCondition ?? tbsfrom[b].On;
              sb.Append(" ON ").Append(onSql);
              if (string.IsNullOrEmpty(tbsfrom[b].Cascade) == false) {
                if (string.IsNullOrEmpty(onSql)) sb.Append(tbsfrom[b].Cascade);
                else sb.Append(" AND ").Append(tbsfrom[b].Cascade);
              }
            }
          }
          break;
        } else {
          if (!string.IsNullOrEmpty(tbsfrom[a].NavigateCondition)) sbnav.Append(" AND (").Append(tbsfrom[a].NavigateCondition).Append(")");
          if (!string.IsNullOrEmpty(tbsfrom[a].On)) sbnav.Append(" AND (").Append(tbsfrom[a].On).Append(")");
          if (a > 0 && !string.IsNullOrEmpty(tbsfrom[a].Cascade)) sbnav.Append(" AND ").Append(tbsfrom[a].Cascade);
        }
        if (a < tbsfrom.Length - 1) sb.Append(", ");
      }
      foreach (var tb in tbsjoin) {
        switch (tb.Type) {
          case SelectTableInfoType.Parent:
          case SelectTableInfoType.RawJoin:
            continue;
          case SelectTableInfoType.LeftJoin:
            sb.Append(" \r\nLEFT JOIN ");
            break;
          case SelectTableInfoType.InnerJoin:
            sb.Append(" \r\nINNER JOIN ");
            break;
          case SelectTableInfoType.RightJoin:
            sb.Append(" \r\nRIGHT JOIN ");
            break;
        }
        var alias = LocalGetTableAlias(tb.Table.Type, tbUnion[tb.Table.Type], tb.Alias, _aliasRule);
        sb.Append(_commonUtils.QuoteSqlName(tbUnion[tb.Table.Type])).Append(" ").Append(alias).Append(" ON ").Append(tb.On ?? tb.NavigateCondition);
        if (!string.IsNullOrEmpty(tb.Cascade)) sb.Append(" AND ").Append(tb.Cascade);
        if (!string.IsNullOrEmpty(tb.On) && !string.IsNullOrEmpty(tb.NavigateCondition)) sbnav.Append(" AND (").Append(tb.NavigateCondition).Append(")");
      }
      if (_join.Length > 0) sb.Append(_join);

      sbnav.Append(_where);
      if (!string.IsNullOrEmpty(_tables[0].Cascade))
        sbnav.Append(" AND ").Append(_tables[0].Cascade);

      if (sbnav.Length > 0) {
        sb.Append(" \r\nWHERE ").Append(sbnav.Remove(0, 5));
      }
      if (string.IsNullOrEmpty(_groupby) == false) {
        sb.Append(_groupby);
        if (string.IsNullOrEmpty(_having) == false)
          sb.Append(" \r\nHAVING ").Append(_having.Substring(5));
      }
      if (_skip > 0) {
        if (string.IsNullOrEmpty(_orderby)) {
          if (string.IsNullOrEmpty(_groupby)) {
            var pktb = _tables.Where(a => a.Table.Primarys.Any()).FirstOrDefault();
            if (pktb != null) _orderby = string.Concat(" \r\nORDER BY ", pktb.Alias, ".", _commonUtils.QuoteSqlName(pktb?.Table.Primarys.First().Attribute.Name));
            else _orderby = string.Concat(" \r\nORDER BY ", _tables.First().Alias, ".", _commonUtils.QuoteSqlName(_tables.First().Table.Columns.First().Value.Attribute.Name));
          } else
            _orderby = _groupby.Replace("GROUP BY ", "ORDER BY ");
        }
        sb.Append(_orderby).Append($" \r\nOFFSET {_skip} ROW");
        if (_limit > 0) sb.Append($" \r\nFETCH NEXT {_limit} ROW ONLY");
      } else {
        sb.Append(_orderby);
      }

      sbnav.Clear();
      if (tbUnionsGt0) sb.Append(") ftb");
    }
    return sb.Append(_tosqlAppendContent).ToString();
  }
  #endregion

  static string LocalGetTableAlias(Type entityType, string tbname, string alias, Func<Type, string, string> aliasRule) {
    if (aliasRule != null) {
      alias = aliasRule(entityType, alias);
      if (tbname.IndexOf(' ') != -1) //还可以这样：select.AsTable((a, b) => "(select * from tb_topic where clicks > 10)").Page(1, 10).ToList()
        alias = Regex.Replace(alias, @" With\([^\)]+\)", ""); //替换 WithLock、WithIndex
    }
    return alias;
  }

  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override ISelect<T1, T2> From<T2>(Expression<Func<ISelectFromExpression<T1>, T2, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3> From<T2, T3>(Expression<Func<ISelectFromExpression<T1>, T2, T3, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4> From<T2, T3, T4>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5> From<T2, T3, T4, T5>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6> From<T2, T3, T4, T5, T6>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7> From<T2, T3, T4, T5, T6, T7>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8> From<T2, T3, T4, T5, T6, T7, T8>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9> From<T2, T3, T4, T5, T6, T7, T8, T9>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> From<T2, T3, T4, T5, T6, T7, T8, T9, T10>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, ISelectFromExpression<T1>>> exp) { InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }

  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override ISelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> From<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Expression<Func<ISelectFromExpression<T1>, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ISelectFromExpression<T1>>> exp) { this.InternalFrom(exp); var ret = new OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(_orm, _commonUtils, _commonExpression, null); OdbcDB2iSeriesSelect<T1>.CopyData(this, ret, exp?.Parameters); return ret; }
  public override string ToSql(string? field = null) => ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2> : Internal.CommonProvider.Select2Provider<T1, T2> where T2 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3> : Internal.CommonProvider.Select3Provider<T1, T2, T3> where T2 : class where T3 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4> : Internal.CommonProvider.Select4Provider<T1, T2, T3, T4> where T2 : class where T3 : class where T4 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5> : Internal.CommonProvider.Select5Provider<T1, T2, T3, T4, T5> where T2 : class where T3 : class where T4 : class where T5 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6> : FreeSql.Internal.CommonProvider.Select6Provider<T1, T2, T3, T4, T5, T6> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7> : FreeSql.Internal.CommonProvider.Select7Provider<T1, T2, T3, T4, T5, T6, T7> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8> : FreeSql.Internal.CommonProvider.Select8Provider<T1, T2, T3, T4, T5, T6, T7, T8> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9> : FreeSql.Internal.CommonProvider.Select9Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : FreeSql.Internal.CommonProvider.Select10Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : FreeSql.Internal.CommonProvider.Select11Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : FreeSql.Internal.CommonProvider.Select12Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : FreeSql.Internal.CommonProvider.Select13Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : FreeSql.Internal.CommonProvider.Select14Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string? ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : FreeSql.Internal.CommonProvider.Select15Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}
class OdbcDB2iSeriesSelect<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : FreeSql.Internal.CommonProvider.Select16Provider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> where T2 : class where T3 : class where T4 : class where T5 : class where T6 : class where T7 : class where T8 : class where T9 : class where T10 : class where T11 : class where T12 : class where T13 : class where T14 : class where T15 : class where T16 : class {
  public OdbcDB2iSeriesSelect(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression, object dywhere) : base(orm, commonUtils, commonExpression, dywhere) { }
  public override string ToSql(string? field = null) => OdbcDB2iSeriesSelect<T1>.ToSqlStatic(_commonUtils, _commonExpression, _select, _distinct, field ?? this.GetAllFieldExpressionTreeLevel2().Field, _join, _where, _groupby, _having, _orderby, _skip, _limit, _tables, this.GetTableRuleUnions(), _aliasRule, _tosqlAppendContent, _whereGlobalFilter, _orm);
}

using FreeSql;
using FreeSql.DB2i;
using FreeSql.Internal;
using FreeSql.Internal.Model;
using System.Text;
using System.Text.RegularExpressions;

namespace xFreeSql.v3_2_687.DB2iSeries;

public static class Extensions {


  internal static string ToSqlStatic(CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm)
      => (_commonUtils as DB2iUtils).IsSelectRowNumber ?
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

}

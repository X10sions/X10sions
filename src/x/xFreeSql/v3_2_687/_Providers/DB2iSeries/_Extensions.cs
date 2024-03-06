using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.Model;
using System.Text;

namespace xFreeSql.v3_2_687.DB2iSeries;

public static class Extensions {

  public static string ToDB2iSqlSelect(this CommonUtils _commonUtils, CommonExpression _commonExpression, string _select, bool _distinct, string field, StringBuilder _join, StringBuilder _where, string _groupby, string _having, string _orderby, int _skip, int _limit, List<SelectTableInfo> _tables, List<Dictionary<Type, string>> tbUnions, Func<Type, string, string> _aliasRule, string _tosqlAppendContent, List<GlobalFilter.Item> _whereGlobalFilter, IFreeSql _orm) {
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
          case SelectTableInfoType.LeftJoin: sbunion.Append(" \r\nLEFT JOIN "); break;
          case SelectTableInfoType.InnerJoin: sbunion.Append(" \r\nINNER JOIN "); break;
          case SelectTableInfoType.RightJoin: sbunion.Append(" \r\nRIGHT JOIN "); break;
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
}

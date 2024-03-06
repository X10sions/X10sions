using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.CommonProvider;
using System.Data.Common;
using System.Text;

namespace xFreeSql.v3_2_687.DB2iSeries.Curd;

class DB2iInsertOrUpdate<T1> : InsertOrUpdateProvider<T1> where T1 : class {
  public DB2iInsertOrUpdate(IFreeSql orm, CommonUtils commonUtils, CommonExpression commonExpression)
    : base(orm, commonUtils, commonExpression) { }

  public override string? ToSql() {
    var dbParams = new List<DbParameter>();
    if (_sourceSql != null) return getMergeSql(null, dbParams);
    if (_source?.Any() != true) return null;
    var sqls = new string[2];
    var ds = SplitSourceByIdentityValueIsNull(_source);
    if (ds.Item1.Any()) sqls[0] = string.Join("\r\n\r\n;\r\n\r\n", ds.Item1.Select(a => getMergeSql(a, dbParams)));
    if (ds.Item2.Any()) sqls[1] = string.Join("\r\n\r\n;\r\n\r\n", ds.Item2.Select(a => getInsertSql(a, dbParams)));
    _params = dbParams.ToArray();
    if (ds.Item2.Any() == false) return sqls[0];
    if (ds.Item1.Any() == false) return sqls[1];
    return string.Join("\r\n\r\n;\r\n\r\n", sqls);
  }

  string getMergeSql(List<T1>? data, List<DbParameter> dbParams) {
    if (_tempPrimarys.Any() == false) throw new Exception(CoreStrings.InsertOrUpdate_Must_Primary_Key(_table.CsName));

    var sb = new StringBuilder().Append("xMERGE INTO ").Append(_commonUtils.QuoteSqlName(TableRuleInvoke())).Append(" t1 \r\nUSING (");
    WriteSourceSelectUnionAll(data, sb, dbParams);
    sb.Append(" ) t2 ON (").Append(string.Join(" AND ", _tempPrimarys.Select(a => $"t1.{_commonUtils.QuoteSqlName(a.Attribute.Name)} = t2.{_commonUtils.QuoteSqlName(a.Attribute.Name)}"))).Append(") \r\n");

    var cols = _table.Columns.Values.Where(a => _tempPrimarys.Contains(a) == false && a.Attribute.CanUpdate == true && _updateIgnore.ContainsKey(a.Attribute.Name) == false);
    if (_doNothing == false && cols.Any())
      sb.Append("WHEN MATCHED THEN \r\n")
          .Append("  update set ").Append(string.Join(", ", cols.Select(a =>
              a.Attribute.IsVersion && a.Attribute.MapType != typeof(byte[]) ?
              $"{_commonUtils.QuoteSqlName(a.Attribute.Name)} = t1.{_commonUtils.QuoteSqlName(a.Attribute.Name)} + 1" :
              $"{_commonUtils.QuoteSqlName(a.Attribute.Name)} = t2.{_commonUtils.QuoteSqlName(a.Attribute.Name)}"
              ))).Append(" \r\n");

    cols = _table.Columns.Values.Where(a => a.Attribute.CanInsert == true);
    if (cols.Any())
      sb.Append("WHEN NOT MATCHED THEN \r\n")
          .Append("  insert (").Append(string.Join(", ", cols.Select(a => _commonUtils.QuoteSqlName(a.Attribute.Name)))).Append(") \r\n")
          .Append("  values (").Append(string.Join(", ", cols.Select(a => $"t2.{_commonUtils.QuoteSqlName(a.Attribute.Name)}"))).Append(")");

    return sb.ToString();
  }

  string? getInsertSql(List<T1> data, List<DbParameter> dbParams) {
    var insert = _orm.Insert<T1>().AsTable(_tableRule).AsType(_table.Type)
      .WithConnection(_connection).WithTransaction(_transaction).NoneParameter(true) as InsertProvider<T1>;
    if (insert is not null) {
      insert._source = data;
      insert._table = _table;
      var sql = insert.ToSql();
      if (string.IsNullOrEmpty(sql)) return null;
      if (insert._params?.Any() == true) dbParams.AddRange(insert._params);
      return sql;
    }
    return null;
  }

}

using System.Data;

namespace Common.Sql {
  public class SqlColumn<T> : ISqlColumn {

    public SqlColumn(string sqlExpression, string name = null, string heading = null, T defaultValue = default(T)) {
      SqlExpression = sqlExpression;
      Name = name ?? sqlExpression;
      Heading = heading ?? Name;
      Value = defaultValue;
    }

    public string Name { get; set; }
    public string SqlExpression { get; set; }
    public string Heading { get; set; }
    public T Value { get; set; }
    public string SqlTableAlias { get; set; }

    public string Sql => (string.IsNullOrWhiteSpace(SqlTableAlias) ? "" : SqlTableAlias + ".") + SqlExpression;
    public string SqlSelect => $"{Sql} As {Name}";
    public DataColumn ToDataColumn() => new DataColumn(Name, typeof(T), Sql);

  }

}

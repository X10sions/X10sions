namespace LinqToDB.Internal.SqlQuery;

public static class ISqlExpressionExtensions {
  public static SqlTable? QueryHelper_ExtractSqlTable(this ISqlExpression? expression) {
    return expression switch {
      SqlTable t => t,
      SqlField f when f.Table is SqlTable t => t,
      //SqlField f when f.Table is SelectQuery { From.Tables: [{ Source: var s }] } => QueryHelper_ExtractSqlTable(s),
      //SqlField f when f.Table is SelectQuery sq => QueryHelper_ExtractSqlTable((from s in sq.From.Tables where !string.IsNullOrWhiteSpace(s.Alias) select s.Source).FirstOrDefault()),
      SqlField f when f.Table is SelectQuery sq => QueryHelper_ExtractSqlTable((from s in sq.From.Tables where s.Source == sq select s.Source).FirstOrDefault()),

      //SqlColumn column => QueryHelper.GetUnderlyingField(column)?.Table as SqlTable,

      SqlColumn c => QueryHelper_ExtractSqlTable(QueryHelper.ExtractField(c)),

      //SqlColumn c => QueryHelper_ExtractSqlTable(c.Parent),
      _ => throw new NotImplementedException($"{expression.GetType()}") //null,
    };
  }

}
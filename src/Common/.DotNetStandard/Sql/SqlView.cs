namespace Common.Sql {
  public class SqlView<T> {

    public SqlView(string sqlAlias, T sqlTable) {
      //', sqlColumns As IEnumerable(Of SqlColumn(Of Object)))
      SqlAlias = sqlAlias;
      SqlTable = sqlTable;
      //'Me.SqlColumns = SqlColumns

    }
    // Public SqlColumns As IEnumerable(Of SqlColumn(Of Object))
    public string SqlAlias { get; set; }
    public T SqlTable { get; set; }

    //Public Function SqlSelect(sqlcolumnSelector As Func(Of T, ISqlColumn)) As String
    //  Dim x = TryCast(Function() sqlcolumnSelector.Invoke, ISqlColumn)
    //  Return x.SqlSelect(SqlAlias)
    //End Function

  }
}
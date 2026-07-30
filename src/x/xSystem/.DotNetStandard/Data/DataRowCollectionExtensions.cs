namespace System.Data;

public static class DataRowCollectionExtensions {
  public static IEnumerable<DataRow> AsEnumerable(this DataRowCollection rows) => rows.Cast<DataRow>();

  public static List<T> ToList<T>(this DataRowCollection rows, Func<DataRow, T> map) {
    var list = new List<T>();
    foreach (DataRow row in rows) {
      list.Add(map(row));
    }
    return list;
  }

}

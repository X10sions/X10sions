namespace System.Data {
  public static class DataRowExtensions {
    [Obsolete("Fix to stop ambiguous method calls caused by LinqToDB.")]

    public static T xField<T>(this DataRow row, string columnName) => row.Field<T>(columnName);

  }
}

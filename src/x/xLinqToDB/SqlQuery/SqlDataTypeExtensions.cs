namespace LinqToDB.SqlQuery {
  public static class SqlDataTypeExtensions {
    public static string AppendLength(this SqlDataType type, string typeName, int maxLength) {
      var length = (type.Type.Length == null || type.Type.Length > maxLength || type.Type.Length < 1) ? maxLength : type.Type.Length.Value;
      return typeName + $"({length})";
    }

  }
}
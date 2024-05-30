namespace System.ComponentModel;
public static class DataObjectMethodTypeExtensions {
  public static string Name(this DataObjectMethodType auditType) {
    switch (auditType) {
      case DataObjectMethodType.Delete: return nameof(DataObjectMethodType.Delete);
      case DataObjectMethodType.Fill: return nameof(DataObjectMethodType.Fill);
      case DataObjectMethodType.Insert: return nameof(DataObjectMethodType.Insert);
      case DataObjectMethodType.Select: return nameof(DataObjectMethodType.Select);
      case DataObjectMethodType.Update: return nameof(DataObjectMethodType.Update);
      default: throw new ArgumentOutOfRangeException(nameof(auditType));
    }
  }

  public static string Code(this DataObjectMethodType auditType) => auditType.Name().Substring(0, 1);

  public static bool IsDelete(this DataObjectMethodType? o) => o == DataObjectMethodType.Delete;
  public static bool IsInsert(this DataObjectMethodType? o) => o == DataObjectMethodType.Insert;
  public static bool IsUpdate(this DataObjectMethodType? o) => o == DataObjectMethodType.Update;
  public static bool IsSelect(this DataObjectMethodType? o) => o == DataObjectMethodType.Select;

  public static DataObjectMethodType? GetSqlDmlAction(this bool isDirty, bool isNewRecord, bool doDelete)
    => isDirty
    ? doDelete
       ? isNewRecord ? null : DataObjectMethodType.Delete
       : isNewRecord ? DataObjectMethodType.Insert : DataObjectMethodType.Update
    : null;

}
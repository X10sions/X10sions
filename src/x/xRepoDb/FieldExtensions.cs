using RepoDb.Interfaces;

namespace RepoDb {
  public static class FieldExtensions {

    public static string AsJoinQualifier(this Field field, string leftAlias, string rightAlias, IDbSetting dbSetting) => field.Name.AsJoinQualifier(leftAlias, rightAlias, dbSetting);

  }
}

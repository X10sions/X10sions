using RepoDb.Extensions;
using RepoDb.Interfaces;

namespace RepoDb.Resolvers {
  public class DB2iSeriesConvertFieldResolver : IResolver<Field, IDbSetting, string> {
    private static ClientTypeToDbTypeResolver DbTypeResolver => new ClientTypeToDbTypeResolver();
    private static DbTypeToDB2iSeriesStringNameResolver StringNameResolver => new DbTypeToDB2iSeriesStringNameResolver();

    public string Resolve(Field field, IDbSetting dbSetting) {
      if (field != null && field.Type != null) {
        var dbType = DbTypeResolver.Resolve(field.Type);
        if (dbType != null) {
          var dbTypeName = StringNameResolver.Resolve(dbType.Value).ToUpper().AsQuoted(dbSetting);
          return $"CAST({field.Name.AsQuoted(true, true, dbSetting)} AS { dbTypeName})";
        }
      }
      return field?.Name?.AsQuoted(true, true, dbSetting);
      //return field?.Name?.AsField(dbSetting);
    }

  }
}

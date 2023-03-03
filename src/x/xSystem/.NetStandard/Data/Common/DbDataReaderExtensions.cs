using System.Reflection;

namespace System.Data.Common;
public static class DbDataReaderExtensions {

  public static T GetFieldValue<T>(this DbDataReader reader, string name, T defaultValue) {
    var idx = reader.GetOrdinal(name);
    return reader.IsDBNull(idx) ? defaultValue : reader.GetFieldValue<T>(idx);
  }

  public async static Task<T> GetFieldValueAsync<T>(this DbDataReader reader, string name, T defaultValue, CancellationToken cancellationToken = default) {
    var idx = reader.GetOrdinal(name);
    return reader.IsDBNull(idx) ? defaultValue : await reader.GetFieldValueAsync<T>(idx, cancellationToken);
  }

  public static DateTime GetDateTime(this DbDataReader reader, string name) => reader.GetDateTime(reader.GetOrdinal(name));
  public static short GetInt16(this DbDataReader reader, string name) => reader.GetInt16(reader.GetOrdinal(name));
  public static int GetInt32(this DbDataReader reader, string name) => reader.GetInt32(reader.GetOrdinal(name));
  public static long GetInt64(this DbDataReader reader, string name) => reader.GetInt64(reader.GetOrdinal(name));
  public static string GetString(this DbDataReader reader, string name) => reader.GetString(reader.GetOrdinal(name));

  public static List<T>? MapToList<T>(this DbDataReader dr) {
    if (dr != null && dr.HasRows) {
      var entity = typeof(T);
      var entities = new List<T>();
      var propDict = new Dictionary<string, PropertyInfo>();
      var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
      var newObject = default(T);
      while (dr.Read()) {
        newObject = Activator.CreateInstance<T>();
        for (var index = 0; index < dr.FieldCount; index++) {
          if (propDict.ContainsKey(dr.GetName(index).ToUpper())) {
            var info = propDict[dr.GetName(index).ToUpper()];
            if ((info != null) && info.CanWrite) {
              var val = dr.GetValue(index);
              info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
            }
          }
        }
        entities.Add(newObject);
      }
      return entities;
    }
    return null;
  }

}

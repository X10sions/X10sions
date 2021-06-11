using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Common {
  public static class DbDataReaderExtensions {

    public static T GetValueOrDefault<T>(this DbDataReader reader, string name) {
      var idx = reader.GetOrdinal(name);
      return reader.IsDBNull(idx) ? default : reader.GetFieldValue<T>(idx);
    }

    public static List<T> MapToList<T>(this DbDataReader dr) {
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
}
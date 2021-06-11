using LinqToDB.Data;
using System.Collections.Generic;
using System.Linq;

namespace LinqToDB.Tests.Base {
  static class DataCache<T>
    where T : class {
    static readonly Dictionary<string, List<T>> _dic = new Dictionary<string, List<T>>();
    public static List<T> Get(string context) {
      lock (_dic) {
        context = context.Replace(".LinqService", "");

        if (!_dic.TryGetValue(context, out var list)) {
          using (new DisableLogging())
          using (var db = new DataConnection(context)) {
            list = db.GetTable<T>().ToList();
            _dic.Add(context, list);
          }
        }

        return list;
      }
    }

    public static void Clear() => _dic.Clear();
  }
}
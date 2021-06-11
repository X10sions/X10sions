using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Linq.Provider {

  internal static class BindingList {
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static IBindingList Create<T>(DataContext context, IEnumerable<T> sequence) {
      var list = sequence.ToList();
      var table = context.Services.Model.GetTable(typeof(T));
      if (table != null) {
        var table2 = context.GetTable(table.RowType.Type);
        var type = typeof(DataBindingList<>).MakeGenericType(table.RowType.Type);
        return (IBindingList)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new object[2]
        {
        list,
        table2
        }, null);
      }
      return new SortableBindingList<T>(list);
    }
  }
}
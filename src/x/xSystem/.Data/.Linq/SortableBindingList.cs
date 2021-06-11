using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace System.Data.Linq {
  internal class SortableBindingList<T> : BindingList<T> {
    internal class PropertyComparer : Comparer<T> {
      private PropertyDescriptor prop;

      private IComparer comparer;

      private ListSortDirection direction;

      private bool useToString;

      internal PropertyComparer(PropertyDescriptor prop, ListSortDirection direction) {
        if (prop.ComponentType != typeof(T)) {
          throw new MissingMemberException(typeof(T).Name, prop.Name);
        }
        this.prop = prop;
        this.direction = direction;
        if (OkWithIComparable(prop.PropertyType)) {
          var type = typeof(Comparer<>).MakeGenericType(prop.PropertyType);
          var property = type.GetProperty("Default");
          comparer = (IComparer)property.GetValue(null, null);
          useToString = false;
        } else if (OkWithToString(prop.PropertyType)) {
          comparer = StringComparer.CurrentCultureIgnoreCase;
          useToString = true;
        }
      }

      public override int Compare(T x, T y) {
        var obj = prop.GetValue(x);
        var obj2 = prop.GetValue(y);
        if (useToString) {
          obj = obj?.ToString();
          obj2 = obj2?.ToString();
        }
        if (direction == ListSortDirection.Ascending) {
          return comparer.Compare(obj, obj2);
        }
        return comparer.Compare(obj2, obj);
      }

      protected static bool OkWithToString(Type t) {
        if (!t.Equals(typeof(XNode))) {
          return t.IsSubclassOf(typeof(XNode));
        }
        return true;
      }

      protected static bool OkWithIComparable(Type t) {
        if (!(t.GetInterface("IComparable") != null)) {
          if (t.IsGenericType) {
            return t.GetGenericTypeDefinition() == typeof(Nullable<>);
          }
          return false;
        }
        return true;
      }

      public static bool IsAllowable(Type t) {
        if (!OkWithToString(t)) {
          return OkWithIComparable(t);
        }
        return true;
      }
    }

    private bool isSorted;

    private PropertyDescriptor sortProperty;

    private ListSortDirection sortDirection;

    protected override ListSortDirection SortDirectionCore => sortDirection;

    protected override PropertyDescriptor SortPropertyCore => sortProperty;

    protected override bool IsSortedCore => isSorted;

    protected override bool SupportsSortingCore => true;

    internal SortableBindingList(IList<T> list)
      : base(list) {
    }

    protected override void RemoveSortCore() {
      isSorted = false;
      sortProperty = null;
    }

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction) {
      var propertyType = prop.PropertyType;
      if (PropertyComparer.IsAllowable(propertyType)) {
        ((List<T>)base.Items).Sort(new PropertyComparer(prop, direction));
        sortDirection = direction;
        sortProperty = prop;
        isSorted = true;
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
    }
  }
}
using System.Data.Linq.Mapping;

namespace System.Data.Linq {
  internal struct RelatedItem {
    internal MetaType Type;

    internal object Item;

    internal RelatedItem(MetaType type, object item) {
      Type = type;
      Item = item;
    }
  }

}
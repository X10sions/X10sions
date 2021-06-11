using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace System.Data.Linq.Provider {
  internal interface IDataServices {
    DataContext Context { get; }
    MetaModel Model { get; }
    IDeferredSourceFactory GetDeferredSourceFactory(MetaDataMember member);
    object GetCachedObject(Expression query);
    bool IsCachedObject(MetaType type, object instance);
    object InsertLookupCachedObject(MetaType type, object instance);
    void OnEntityMaterialized(MetaType type, object instance);
  }
}
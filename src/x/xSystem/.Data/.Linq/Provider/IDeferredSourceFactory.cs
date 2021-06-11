using System.Collections;

namespace System.Data.Linq.Provider {
  internal interface IDeferredSourceFactory {
    IEnumerable CreateDeferredSource(object instance);
    IEnumerable CreateDeferredSource(object[] keyValues);
  }
}
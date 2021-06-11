using System.Collections.Generic;
using System.Threading;

namespace System.Data.Linq.Mapping {
  public abstract class MappingSource {
    private MetaModel primaryModel;

    private ReaderWriterLock rwlock;

    private Dictionary<Type, MetaModel> secondaryModels;

    public MetaModel GetModel(Type dataContextType) {
      if (dataContextType == null) {
        throw Error.ArgumentNull("dataContextType");
      }
      MetaModel metaModel = null;
      if (primaryModel == null) {
        metaModel = CreateModel(dataContextType);
        Interlocked.CompareExchange(ref primaryModel, metaModel, null);
      }
      if (!(primaryModel.ContextType == dataContextType)) {
        if (secondaryModels == null) {
          Interlocked.CompareExchange(ref secondaryModels, new Dictionary<Type, MetaModel>(), null);
        }
        if (rwlock == null) {
          Interlocked.CompareExchange(ref rwlock, new ReaderWriterLock(), null);
        }
        rwlock.AcquireReaderLock(-1);
        MetaModel value;
        try {
          if (secondaryModels.TryGetValue(dataContextType, out value)) {
            return value;
          }
        } finally {
          rwlock.ReleaseReaderLock();
        }
        rwlock.AcquireWriterLock(-1);
        try {
          if (!secondaryModels.TryGetValue(dataContextType, out value)) {
            if (metaModel == null) {
              metaModel = CreateModel(dataContextType);
            }
            secondaryModels.Add(dataContextType, metaModel);
            return metaModel;
          }
          return value;
        } finally {
          rwlock.ReleaseWriterLock();
        }
      }
      return primaryModel;
    }

    protected abstract MetaModel CreateModel(Type dataContextType);
  }

}
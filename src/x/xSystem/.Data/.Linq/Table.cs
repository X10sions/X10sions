using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace System.Data.Linq {

  public sealed class Table<TEntity> : IQueryProvider, ITable, IListSource, ITable<TEntity> where TEntity : class {
    private readonly MetaTable metaTable;
    private IBindingList cachedList;
    public DataContext Context { get; }

    public bool IsReadOnly => !metaTable.RowType.IsEntity;

    Expression IQueryable.Expression => Expression.Constant(this);

    Type IQueryable.ElementType => typeof(TEntity);

    IQueryProvider IQueryable.Provider => this;

    bool IListSource.ContainsListCollection => false;

    internal Table(DataContext context, MetaTable metaTable) {
      Context = context;
      this.metaTable = metaTable;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    IQueryable IQueryProvider.CreateQuery(Expression expression) {
      if (expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      var elementType = TypeSystem.GetElementType(expression.Type);
      var type = typeof(IQueryable<>).MakeGenericType(elementType);
      if (!type.IsAssignableFrom(expression.Type)) {
        throw System.Data.Linq.Error.ExpectedQueryableArgument("expression", type);
      }
      var type2 = typeof(DataQuery<>).MakeGenericType(elementType);
      return (IQueryable)Activator.CreateInstance(type2, Context, expression);
    }

    IQueryable<TResult> IQueryProvider.CreateQuery<TResult>(Expression expression) {
      if (expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      if (!typeof(IQueryable<TResult>).IsAssignableFrom(expression.Type)) {
        throw System.Data.Linq.Error.ExpectedQueryableArgument("expression", typeof(IEnumerable<TResult>));
      }
      return new DataQuery<TResult>(Context, expression);
    }

    object IQueryProvider.Execute(Expression expression) => Context.Provider.Execute(expression).ReturnValue;

    TResult IQueryProvider.Execute<TResult>(Expression expression) => (TResult)Context.Provider.Execute(expression).ReturnValue;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() => GetEnumerator();

    public IEnumerator<TEntity> GetEnumerator() => ((IEnumerable<TEntity>)Context.Provider.Execute(Expression.Constant(this)).ReturnValue).GetEnumerator();

    IList IListSource.GetList() {
      if (cachedList == null) {
        cachedList = GetNewBindingList();
      }
      return cachedList;
    }

    public IBindingList GetNewBindingList() => BindingList.Create(Context, this);

    public void InsertOnSubmit(TEntity entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!IsTrackableType(inheritanceType)) {
        throw System.Data.Linq.Error.TypeCouldNotBeAdded(inheritanceType.Type);
      }
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null) {
        trackedObject = Context.Services.ChangeTracker.Track(entity);
        trackedObject.ConvertToNew();
      } else if (trackedObject.IsWeaklyTracked) {
        trackedObject.ConvertToNew();
      } else if (trackedObject.IsDeleted) {
        trackedObject.ConvertToPossiblyModified();
      } else if (trackedObject.IsRemoved) {
        trackedObject.ConvertToNew();
      } else if (!trackedObject.IsNew) {
        throw System.Data.Linq.Error.CantAddAlreadyExistingItem();
      }
    }

    void ITable.InsertOnSubmit(object entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      InsertOnSubmit(val);
    }

    public void InsertAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      foreach (var item in Enumerable.ToList(entities)) {
        var entity = (TEntity)(object)item;
        InsertOnSubmit(entity);
      }
    }

    void ITable.InsertAllOnSubmit(IEnumerable entities) {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      foreach (var item in Enumerable.ToList(Enumerable.Cast<object>(entities))) {
        ((ITable)this).InsertOnSubmit(item);
      }
    }

    private static bool IsTrackableType(MetaType type) {
      if (type == null) {
        return false;
      }
      if (!type.CanInstantiate) {
        return false;
      }
      if (type.HasInheritance && !type.HasInheritanceCode) {
        return false;
      }
      return true;
    }

    public void DeleteOnSubmit(TEntity entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null) {
        throw System.Data.Linq.Error.CannotRemoveUnattachedEntity();
      }
      if (trackedObject.IsNew) {
        trackedObject.ConvertToRemoved();
      } else if (trackedObject.IsPossiblyModified || trackedObject.IsModified) {
        trackedObject.ConvertToDeleted();
      }
    }

    void ITable.DeleteOnSubmit(object entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      DeleteOnSubmit(val);
    }

    public void DeleteAllOnSubmit<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var list = Enumerable.ToList<TSubEntity>(entities);
      foreach (var item in list) {
        var entity = (TEntity)(object)item;
        DeleteOnSubmit(entity);
      }
    }

    void ITable.DeleteAllOnSubmit(IEnumerable entities) {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var list = Enumerable.ToList<object>(Enumerable.Cast<object>(entities));
      foreach (var item in list) {
        ((ITable)this).DeleteOnSubmit(item);
      }
    }

    public void Attach(TEntity entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      Attach(entity, false);
    }

    void ITable.Attach(object entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      Attach(val, false);
    }

    public void Attach(TEntity entity, bool asModified) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!IsTrackableType(inheritanceType)) {
        throw System.Data.Linq.Error.TypeCouldNotBeTracked(inheritanceType.Type);
      }
      if (asModified && inheritanceType.VersionMember == null && inheritanceType.HasUpdateCheck) {
        throw System.Data.Linq.Error.CannotAttachAsModifiedWithoutOriginalState();
      }
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null || trackedObject.IsWeaklyTracked) {
        if (trackedObject == null) {
          trackedObject = Context.Services.ChangeTracker.Track(entity, true);
        }
        if (asModified) {
          trackedObject.ConvertToModified();
        } else {
          trackedObject.ConvertToUnmodified();
        }
        if (Context.Services.InsertLookupCachedObject(inheritanceType, entity) != entity) {
          throw new DuplicateKeyException(entity, System.Data.Linq.Strings.CantAddAlreadyExistingKey);
        }
        trackedObject.InitializeDeferredLoaders();
        return;
      }
      throw System.Data.Linq.Error.CannotAttachAlreadyExistingEntity();
    }

    void ITable.Attach(object entity, bool asModified) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      Attach(val, asModified);
    }

    public void Attach(TEntity entity, TEntity original) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      if (original == null) {
        throw System.Data.Linq.Error.ArgumentNull("original");
      }
      if (entity.GetType() != original.GetType()) {
        throw System.Data.Linq.Error.OriginalEntityIsWrongType();
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var inheritanceType = metaTable.RowType.GetInheritanceType(entity.GetType());
      if (!IsTrackableType(inheritanceType)) {
        throw System.Data.Linq.Error.TypeCouldNotBeTracked(inheritanceType.Type);
      }
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject == null || trackedObject.IsWeaklyTracked) {
        if (trackedObject == null) {
          trackedObject = Context.Services.ChangeTracker.Track(entity, true);
        }
        trackedObject.ConvertToPossiblyModified(original);
        if (Context.Services.InsertLookupCachedObject(inheritanceType, entity) != entity) {
          throw new DuplicateKeyException(entity, System.Data.Linq.Strings.CantAddAlreadyExistingKey);
        }
        trackedObject.InitializeDeferredLoaders();
        return;
      }
      throw System.Data.Linq.Error.CannotAttachAlreadyExistingEntity();
    }

    void ITable.Attach(object entity, object original) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      if (original == null) {
        throw System.Data.Linq.Error.ArgumentNull("original");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      if (entity.GetType() != original.GetType()) {
        throw System.Data.Linq.Error.OriginalEntityIsWrongType();
      }
      Attach(val, (TEntity)original);
    }

    public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities) where TSubEntity : TEntity {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      AttachAll(entities, false);
    }

    void ITable.AttachAll(IEnumerable entities) {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      ((ITable)this).AttachAll(entities, false);
    }

    public void AttachAll<TSubEntity>(IEnumerable<TSubEntity> entities, bool asModified) where TSubEntity : TEntity {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var list = Enumerable.ToList<TSubEntity>(entities);
      foreach (var item in list) {
        var entity = (TEntity)(object)item;
        Attach(entity, asModified);
      }
    }

    void ITable.AttachAll(IEnumerable entities, bool asModified) {
      if (entities == null) {
        throw System.Data.Linq.Error.ArgumentNull("entities");
      }
      CheckReadOnly();
      Context.CheckNotInSubmitChanges();
      Context.VerifyTrackingEnabled();
      var list = Enumerable.ToList<object>(Enumerable.Cast<object>(entities));
      foreach (var item in list) {
        ((ITable)this).Attach(item, asModified);
      }
    }

    public TEntity GetOriginalEntityState(TEntity entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var metaType = Context.Mapping.GetMetaType(entity.GetType());
      if (metaType == null || !metaType.IsEntity) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject != null) {
        if (trackedObject.Original != null) {
          return (TEntity)trackedObject.CreateDataCopy(trackedObject.Original);
        }
        return (TEntity)trackedObject.CreateDataCopy(trackedObject.Current);
      }
      return null;
    }

    object ITable.GetOriginalEntityState(object entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      return GetOriginalEntityState(val);
    }

    public ModifiedMemberInfo[] GetModifiedMembers(TEntity entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var metaType = Context.Mapping.GetMetaType(entity.GetType());
      if (metaType == null || !metaType.IsEntity) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      TrackedObject trackedObject = Context.Services.ChangeTracker.GetTrackedObject(entity);
      if (trackedObject != null) {
        return Enumerable.ToArray<ModifiedMemberInfo>(trackedObject.GetModifiedMembers());
      }
      return new ModifiedMemberInfo[0];
    }

    ModifiedMemberInfo[] ITable.GetModifiedMembers(object entity) {
      if (entity == null) {
        throw System.Data.Linq.Error.ArgumentNull("entity");
      }
      var val = entity as TEntity;
      if (val == null) {
        throw System.Data.Linq.Error.EntityIsTheWrongType();
      }
      return GetModifiedMembers(val);
    }

    private void CheckReadOnly() {
      if (IsReadOnly) {
        throw System.Data.Linq.Error.CannotPerformCUDOnReadOnlyTable(ToString());
      }
    }

    public override string ToString() => "Table(" + typeof(TEntity).Name + ")";
  }
}
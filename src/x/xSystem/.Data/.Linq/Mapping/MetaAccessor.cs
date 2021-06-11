namespace System.Data.Linq.Mapping {
  public abstract class MetaAccessor {
    public abstract Type Type { get; }
    public abstract object GetBoxedValue(object instance);
    public abstract void SetBoxedValue(ref object instance, object value);
    public virtual bool HasValue(object instance) => true;
    public virtual bool HasAssignedValue(object instance) => true;
    public virtual bool HasLoadedValue(object instance) => false;
  }

  public abstract class MetaAccessor<TEntity, TMember> : MetaAccessor {
    public override Type Type => typeof(TMember);
    public override void SetBoxedValue(ref object instance, object value) {
      var instance2 = (TEntity)instance;
      SetValue(ref instance2, (TMember)value);
      instance = instance2;
    }
    public override object GetBoxedValue(object instance) => GetValue((TEntity)instance);
    public abstract TMember GetValue(TEntity instance);
    public abstract void SetValue(ref TEntity instance, TMember value);
  }

}
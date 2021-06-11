using System.Collections;
using System.Linq;

namespace System.Data.Linq {
  public interface ITable : IQueryable {
    DataContext Context { get; }
    bool IsReadOnly { get; }
    void InsertOnSubmit(object entity);
    void InsertAllOnSubmit(IEnumerable entities);
    void Attach(object entity);
    void Attach(object entity, bool asModified);
    void Attach(object entity, object original);
    void AttachAll(IEnumerable entities);
    void AttachAll(IEnumerable entities, bool asModified);
    void DeleteOnSubmit(object entity);
    void DeleteAllOnSubmit(IEnumerable entities);
    object GetOriginalEntityState(object entity);
    ModifiedMemberInfo[] GetModifiedMembers(object entity);
  }

  public interface ITable<TEntity> : IQueryable<TEntity> where TEntity : class {
    void InsertOnSubmit(TEntity entity);
    void Attach(TEntity entity);
    void DeleteOnSubmit(TEntity entity);
  }

}

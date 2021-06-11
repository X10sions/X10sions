using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BBaithwaite {
  public interface IRepository<T, TId> where T : EntityBase, IAggregateRoot<TId> {
    TId Add(T item);
    void Remove(T item);
    void Update(T item);
    T FindById(TId id);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    IEnumerable<T> FindAll();
  }
}

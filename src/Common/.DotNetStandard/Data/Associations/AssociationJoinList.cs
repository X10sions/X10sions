using System.Linq.Expressions;

namespace Common.Data.Assocations;

public class AssociationJoinList : List<IAssociationJoin> {
  /// <summary>OneToOne</summary>
  public void Add<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? {
    Add(new AssociationJoinOneToOne<T1, T2>(selector1, predicate));
    Add(new AssociationJoinOneToOne<T2, T1>(selector2, Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0])));
  }

  /// <summary>ManyToOne</summary>
  public void Add<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, IEnumerable<T1>>> selector2, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? {
    Add(new AssociationJoinOneToOne<T1, T2>(selector1, predicate));
    Add(new AssociationJoinManyToOne<T2, T1>(selector2, Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0])));
  }

  /// <summary>OneToOne</summary>
  public void Add<T1, T2>(Expression<Func<T1, T2?>> selector, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? => Add(new AssociationJoinOneToOne<T1, T2>(selector, predicate));

  /// <summary>ManyToOne</summary>
  public void Add<T1, T2>(Expression<Func<T1, IEnumerable<T2>?>> selector, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? => Add(new AssociationJoinOneToOne<T1, T2>(selector, predicate));

}

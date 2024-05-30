using System.Linq.Expressions;

namespace Common.Data.Assocations;

public class Association<T1, T2> : IAssociation<T1, T2> where T1 : class? where T2 : class? {
  Association(IAssociationJoin<T1, T2> join1, IAssociationJoin<T2, T1> join2) {
    Join1 = join1;
    Join2 = join2;
    //    Selector2 = selector2;
    //    Predicate1 = predicate;
    //    Predicate2 = Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0]);
  }

  /// <summary>OneToOne</summary>
  public Association(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2, Expression<Func<T1, T2, bool>> predicate)
    : this(
        new AssociationJoinOneToOne<T1, T2>(selector1, predicate),
        new AssociationJoinOneToOne<T2, T1>(selector2, Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0]))
        ) { }

  /// <summary>ManyToOne</summary>
  public Association(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, IEnumerable<T1>>> selector2, Expression<Func<T1, T2, bool>> predicate)
    : this(
        new AssociationJoinOneToOne<T1, T2>(selector1, predicate),
        new AssociationJoinManyToOne<T2, T1>(selector2, Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0]))
        ) { }
  public IAssociationJoin<T1, T2> Join1 { get; }
  IAssociationJoin IAssociation.Join1 => Join1;
  public IAssociationJoin<T2, T1> Join2 { get; }
  IAssociationJoin IAssociation.Join2 => Join2;
}

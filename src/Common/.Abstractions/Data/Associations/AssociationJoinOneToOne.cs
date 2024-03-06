using System.Linq.Expressions;

namespace Common.Data.Assocations;

public class AssociationJoinOneToOne<T1, T2> : IAssociationJoinOneToOne<T1, T2> where T1 : class? where T2 : class? {
  public AssociationJoinOneToOne(Expression<Func<T1, T2?>> selector, Expression<Func<T1, T2, bool>> predicate) {
    Selector = selector;
    Predicate = predicate;
  }
  public AssociationJoinOneToOne(LambdaExpression selector, Expression<Func<T1, T2, bool>> predicate)
    : this((Expression<Func<T1, T2?>>)selector, predicate) { }

  public Expression<Func<T1, T2?>> Selector { get; }
  LambdaExpression IAssociationJoin.Selector => Selector;
  public Expression<Func<T1, T2, bool>> Predicate { get; }
  LambdaExpression IAssociationJoin.Predicate => Predicate;
}

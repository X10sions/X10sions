using System.Linq.Expressions;

namespace Common.Data.Assocations;

public class AssociationJoinManyToOne<T1, T2> : IAssociationJoinManyToOne<T1, T2> where T1 : class? where T2 : class? {
  public AssociationJoinManyToOne(Expression<Func<T1, IEnumerable<T2>>> selector, Expression<Func<T1, T2, bool>> predicate) {
    Selector = selector;
    Predicate = predicate;
  }
  public AssociationJoinManyToOne(LambdaExpression selector, Expression<Func<T1, T2, bool>> predicate)
    : this((Expression<Func<T1, IEnumerable<T2>>>?)selector, predicate) { }

  public Expression<Func<T1, IEnumerable<T2>>> Selector { get; }
  LambdaExpression IAssociationJoin.Selector => Selector;
  public Expression<Func<T1, T2, bool>> Predicate { get; }
  LambdaExpression IAssociationJoin.Predicate => Predicate;
}
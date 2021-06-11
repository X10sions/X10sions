using Common.Linq;

namespace System.Linq.Expressions {
  public static class Expression_IvanStoev_Extensions {
    // 2016-03-27: IvanStoev PredicateUtils
    // https://stackoverflow.com/questions/36246162/establish-a-link-between-two-lists-in-linq-to-entities-where-clause/36247259#36247259

    public static Expression<Func<T, bool>> And_ISPO<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) {
      if (Equals(left, right)) return left;
      if (left == null || Equals(left, true.ToPredicate<T>())) return right;
      if (right == null || Equals(right, true.ToPredicate<T>())) return left;
      if (Equals(left, false.ToPredicate<T>()) || Equals(right, false.ToPredicate<T>())) return false.ToPredicate<T>();
      var body = Expression.AndAlso(left.Body, right.Body.Replace_ISPU(right.Parameters[0], left.Parameters[0]));
      return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }

    public static Expression<Func<T, bool>> Or_ISPU<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) {
      if (Equals(left, right)) return left;
      if (left == null || Equals(left, false.ToPredicate<T>())) return right;
      if (right == null || Equals(right, false.ToPredicate<T>())) return left;
      if (Equals(left, true.ToPredicate<T>()) || Equals(right, true.ToPredicate<T>())) return true.ToPredicate<T>();
      var body = Expression.OrElse(left.Body, right.Body.Replace_ISPU(right.Parameters[0], left.Parameters[0]));
      return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }

    static Expression Replace_ISPU(this Expression expression, Expression source, Expression target) => new ReplaceExpressionVisitor(source, target).Visit(expression);

  }
}

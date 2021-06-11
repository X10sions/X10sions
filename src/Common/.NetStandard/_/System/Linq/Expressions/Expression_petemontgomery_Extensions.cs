using Common.Linq;

namespace System.Linq.Expressions {       
  public static class Expression_PeteMontgomery_Extensions {

    // 2011-02-10: UniversalPredicateBuilder
    // https://petemontgomery.wordpress.com/2011/02/10/a-universal-predicatebuilder/

    public static Expression<Func<T, bool>> And_UPB<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose_UPB(second, Expression.AndAlso);
    public static Expression<Func<T, bool>> Or_UPB<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose_UPB(second, Expression.OrElse);

    static Expression<T> Compose_UPB<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge) {
      var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
      var secondBody = ParametersRebinderExpressionVisitor.ReplaceParameters(map, second.Body);
      return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

  }
}

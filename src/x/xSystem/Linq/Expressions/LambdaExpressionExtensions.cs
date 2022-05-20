namespace System.Linq.Expressions {
  public static class LambdaExpressionExtensions {

    public static Expression<Func<T, TResult>> AsTypedExpression<T, TResult>(this LambdaExpression le)
      => Expression.Lambda<Func<T, TResult>>(le.Body, le.Parameters);

  }
}

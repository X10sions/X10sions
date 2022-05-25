namespace System.Linq.Expressions {
  public static class LambdaExpressionExtensions {

    public static Expression<Func<T, TResult>> AsTypedExpression<T, TResult>(this LambdaExpression le)
      => Expression.Lambda<Func<T, TResult>>(le.Body, le.Parameters);

    //public static Expression<Func<T, TResult>> AsTypedExpression<T, TResult>(this Expression expr, TResult value)
    //  => Expression.Lambda<Func<T, TResult>>(expr, expr.Parameters);

    public static Expression<Func<T, TResult>> AsTypedExpression<T, TResult>(this LambdaExpression le, TResult value)
      => Expression.Lambda<Func<T, TResult>>(le.Body, le.Parameters);

    //public static Expression<Func<T, TResult?>> AsTypedExpressionNullable<T, TResult>(this Expression expr, TResult? value)
    //  => Expression.Lambda<Func<T, TResult?>>(expr, expr.Parameters);

    public static Expression<Func<T, TResult?>> AsTypedExpressionNullable<T, TResult>(this LambdaExpression le, TResult? value)
      => Expression.Lambda<Func<T, TResult?>>(le.Body, le.Parameters);

  }
}

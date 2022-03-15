using Common.Linq;
using System.Text.RegularExpressions;

namespace System.Linq.Expressions {
  public static class ExpressionExtensions {

    public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate) => ifTrue.HasValue && ifTrue.Value ? expr1.And(truePredicate) : expr1;
    public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate) => expr1.And(ifTrue.HasValue && ifTrue.Value ? truePredicate : falsePredicate);

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
      var parameter = Expression.Parameter(typeof(T));
      var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
      var left = leftVisitor.Visit(expr1.Body);
      var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
      var right = rightVisitor.Visit(expr2.Body);
      return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    }

    public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate) => ifTrue.HasValue && ifTrue.Value ? expr1.Or(truePredicate) : expr1;
    public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> expr1, bool? ifTrue, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate) => expr1.Or(ifTrue.HasValue && ifTrue.Value ? truePredicate : falsePredicate);

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
      var parameter = Expression.Parameter(typeof(T));
      var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
      var left = leftVisitor.Visit(expr1.Body);
      var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
      var right = rightVisitor.Visit(expr2.Body);
      return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);
    }

    public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(
      params Expression<Func<TSource, TDestination>>[] selectors) {
      var param = Expression.Parameter(typeof(TSource), "x");
      return Expression.Lambda<Func<TSource, TDestination>>(
        Expression.MemberInit(
          Expression.New(typeof(TDestination).GetConstructor(Type.EmptyTypes)),
          from selector in selectors
          let replace = new ReplaceParameterExpressionVisitor(selector.Parameters[0], param)
          from binding in ((MemberInitExpression)selector.Body).Bindings.OfType<MemberAssignment>()
          select Expression.Bind(binding.Member, replace.VisitAndConvert(binding.Expression, nameof(Combine))))
          , param);
    }

    public static Expression<Func<T, bool>> CreateBetweenExpression<T>(Expression<Func<T, IComparable>> fieldExtractor, IComparable min, IComparable max) {
      var minPar = Expression.Parameter(typeof(T), "xMin");
      var maxPar = Expression.Parameter(typeof(T), "xMax");
      var minExpression = CreateGreaterThanExpression(fieldExtractor, min);
      var maxExpression = CreateLessThanOrEqualExpression(fieldExtractor, max);
      var andExpression = Expression.And(minExpression, maxExpression);
      return Expression.Lambda<Func<T, bool>>(andExpression, minPar, maxPar);
    }

    public static Expression<Func<T, bool>> CreateFilterFromString<T>(Expression<Func<T, IComparable>> fieldExtractor, string text) {
      var greaterOrLessRegex = new Regex(@"^\s*(?<sign>\>|\<)\s*(?<number>\d+(\.\d+){0,1})\s*$");
      var match = greaterOrLessRegex.Match(text);
      if (match.Success) {
        IComparable value = match.Result("${number}");
        var sign = match.Result("${sign}");
        switch (sign) {
          case ">": return CreateGreaterThanExpression(fieldExtractor, value);
          case ">=": return CreateGreaterThanOrEqualExpression(fieldExtractor, value);
          case "<": return CreateLessThanExpression(fieldExtractor, value);
          case "<=": return CreateLessThanOrEqualExpression(fieldExtractor, value);
          default: throw new Exception("Bad Sign!");
        }
      }
      var betweenRegex = new Regex(@"^\s*(?<number1>\d+(\.\d+){0,1})\s*-\s*(?<number2>\d+(\.\d+){0,1})\s*$");
      match = betweenRegex.Match(text);
      if (match.Success) {
        var number1 = decimal.Parse(match.Result("${number1}"));
        var number2 = decimal.Parse(match.Result("${number2}"));
        return CreateBetweenExpression(fieldExtractor, number1, number2);
      }
      throw new Exception("Bad filter Format!");
    }

    public static Expression<Func<T, bool>> CreateGreaterThanExpression<T>(Expression<Func<T, IComparable>> fieldExtractor, IComparable value) {
      // https://stackoverflow.com/questions/10599831/extension-method-returning-lambda-expression-through-compare
      //    var filter = CreateGreaterThanExpression<TestEnitity>(x => x.SortProperty, 3);
      //    var items = ents.TestEnitities.Where(filter).ToArray();

      var xPar = Expression.Parameter(typeof(T), "x");
      var getter = fieldExtractor.GetMemberExpression(xPar);
      var resultBody = Expression.GreaterThan(getter, Expression.Constant(value, typeof(IComparable)));
      return Expression.Lambda<Func<T, bool>>(resultBody, xPar);
    }

    public static Expression<Func<T, bool>> CreateGreaterThanOrEqualExpression<T>(Expression<Func<T, IComparable>> fieldExtractor, IComparable value) {
      var xPar = Expression.Parameter(typeof(T), "x");
      var getter = fieldExtractor.GetMemberExpression(xPar);
      var resultBody = Expression.GreaterThanOrEqual(getter, Expression.Constant(value, typeof(IComparable)));
      return Expression.Lambda<Func<T, bool>>(resultBody, xPar);
    }

    public static Expression<Func<T, bool>> CreateLessThanExpression<T>(Expression<Func<T, IComparable>> fieldExtractor, IComparable value) {
      var xPar = Expression.Parameter(typeof(T), "x");
      var getter = fieldExtractor.GetMemberExpression(xPar);
      var resultBody = Expression.LessThan(getter, Expression.Constant(value, typeof(IComparable)));
      return Expression.Lambda<Func<T, bool>>(resultBody, xPar);
    }

    public static Expression<Func<T, bool>> CreateLessThanOrEqualExpression<T>(Expression<Func<T, IComparable>> fieldExtractor, IComparable value) {
      var xPar = Expression.Parameter(typeof(T), "x");
      var getter = fieldExtractor.GetMemberExpression(xPar);
      var resultBody = Expression.LessThanOrEqual(getter, Expression.Constant(value, typeof(IComparable)));
      return Expression.Lambda<Func<T, bool>>(resultBody, xPar);
    }


    public static MemberExpression GetMemberExpression<T>(this Expression<Func<T, IComparable>> fieldExtractor, ParameterExpression xPar)
      => (MemberExpression)new ParameterRebinderExpressionVisitor(xPar).Visit(fieldExtractor.Body);

  }
}

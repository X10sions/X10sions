using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CommonOrm {
  public class OrmJoinClause<T> {
    public OrmJoinType JoinType { get; set; }
    public Expression PropertyExpression { get; set; }
    public Expression PredicateExpression { get; set; }
    //public Expression<Func<T, object>> PropertyExpressionFunc { get; set; }
    //public Expression<Func<T, object, bool>> PredicateExpressionFunc { get; set; }
    public bool CanBeNull => JoinType != OrmJoinType.Inner;
    //public OrmRelationship Relationship { get; set; }

    public void Init<TOther>(Expression<Func<T, IEnumerable<TOther>>> prop, Expression<Func<T, TOther, bool>> predicate, OrmJoinType joinType = OrmJoinType.Left) {
      JoinType = joinType;
      PropertyExpression = prop;
      PredicateExpression = predicate;
      Type2 = typeof(TOther);
      //Relationship = OrmRelationship.OneToMany;
      //PropertyExpressionFunc = GetObjectPropertyExpression(prop);
      //PredicateExpressionFunc = GetObjectPredicateExpressionFunc(predicate);
    }

    //public void Init<TOther, TKey, TOtherKey>(Expression<Func<T, IEnumerable<TOther>>> prop, Expression<Func<T, TKey>> thisKeyExpression, Expression<Func<TOther, TOtherKey>> otherKeyExpression, OrmJoinType joinType = OrmJoinType.Left) {
    //  var predicateExpression = Expression.Lambda<Func<T, TOther, bool>>(Expression.Equal(thisKeyExpression.Body, otherKeyExpression.Body));
    //  Init(prop, predicateExpression, joinType);
    //}

    public void Init<TOther>(Expression<Func<T, TOther>> prop, Expression<Func<T, TOther, bool>> predicate, OrmJoinType joinType = OrmJoinType.Inner) {
      JoinType = joinType;
      PropertyExpression = prop;
      PredicateExpression = predicate;
      Type2 = typeof(TOther);
      //Relationship = OrmRelationship.ManyToOne;
      //PropertyExpressionFunc = GetObjectPropertyExpression(prop);
      //PredicateExpressionFunc = GetObjectPredicateExpressionFunc(predicate);
    }

    //public void Init<TOther, TKey, TOtherKey>(Expression<Func<T, TOther>> prop, Expression<Func<T, TKey>> thisKeyExpression, Expression<Func<TOther, TOtherKey>> otherKeyExpression, OrmJoinType joinType = OrmJoinType.Inner) {
    //  var predicateExpression = Expression.Lambda<Func<T, TOther, bool>>(Expression.Equal(thisKeyExpression.Body, otherKeyExpression.Body));
    //  Init(prop, predicateExpression, joinType);
    //}

    public Expression<Func<T, object>> GetObjectPropertyExpression<TOther>(Expression<Func<T, TOther>> expression)
      => Expression.Lambda<Func<T, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters);

    //public Expression<Func<T, TOther>> GetTypedPropertyExpression<TOther>(Expression<Func<T, TOther>> expression)
    //  => Expression.Lambda<Func<T, TOther>>(Expression.Convert(expression.Body, typeof(TOther)), expression.Parameters);

    public Expression<Func<T, object, bool>> GetObjectPredicateExpressionFunc<TOther>(Expression<Func<T, TOther, bool>> expression) {


      var resultBody = Expression.Convert(expression.Body, typeof(object));
      return Expression.Lambda<Func<T, object, bool>>(resultBody, expression.Parameters);

      //Expression<Func<T, object>>
      //var objPred = Expression.Lambda<Func<T, object, bool>>(Expression.Convert(predicate.Body, typeof(bool)), predicate.Parameters);
      //return (Expression<Func<T, object, bool>>)Expression.Lambda(typeof(TOther), predicate.Body, predicate.Parameters);
      var xParam = Expression.Parameter(typeof(T), expression.Parameters[0].Name);
      var otherParam = Expression.Parameter(typeof(object), expression.Parameters[1].Name);
      var call = Expression.Invoke(expression, xParam, otherParam);
      return Expression.Lambda<Func<T, object, bool>>(call, xParam, otherParam);
    }

    #region AsTyped
    public Type Type1 => typeof(T);
    public Type Type2 { get; private set; }

    public Expression AsTypedPropertyExpression() {
      var parameters = new[] { Type1 }.Select(Expression.Parameter).ToArray();
      var castedParameters = parameters.Select(x => Expression.Convert(x, typeof(object))).ToArray();
      var invocation = Expression.Invoke(PropertyExpression, castedParameters);
      return Expression.Lambda(invocation, parameters);
    }

    public Expression AsTypedPredicateExpression() {
      var parameters = new[] { Type1, Type2 }.Select(Expression.Parameter).ToArray();
      var castedParameters = parameters.Select(x => Expression.Convert(x, typeof(object))).ToArray();
      var invocation = Expression.Invoke(PredicateExpression, castedParameters);
      return Expression.Lambda(invocation, parameters);
    }

    public Expression<Func<T1, object>> TypedPropertyExpression<T1>() => (Expression<Func<T1, object>>)AsTypedPropertyExpression();
    public Expression<Func<T1, T2, bool>> TypedPredicateExpression<T1, T2>() => (Expression<Func<T1, T2, bool>>)AsTypedPredicateExpression();


    //public Expression AsTypedPropertyExpressionFunc() {
    //  var parameters = new[] { Type1 }.Select(Expression.Parameter).ToArray();
    //  var castedParameters = parameters.Select(x => Expression.Convert(x, typeof(object))).ToArray();
    //  var invocation = Expression.Invoke(PropertyExpressionFunc, castedParameters);
    //  return Expression.Lambda(invocation, parameters);
    //}

    //public Expression AsTypedPredicateExpressionFunc() {
    //  var parameters = new[] { Type1, Type2 }.Select(Expression.Parameter).ToArray();
    //  var castedParameters = parameters.Select(x => Expression.Convert(x, typeof(object))).ToArray();
    //  var invocation = Expression.Invoke(PredicateExpressionFunc, castedParameters);
    //  return Expression.Lambda(invocation, parameters);
    //}

    //public Expression<Func<T1, object>> TypedPropertyExpressionFunc<T1>() => (Expression<Func<T1, object>>)AsTypedPropertyExpressionFunc();
    //public Expression<Func<T1, T2, bool>> TypedPredicateExpressionFunc<T1, T2>() => (Expression<Func<T1, T2, bool>>)AsTypedPredicateExpressionFunc();

    #endregion


    //public Expression<Func<T, TOther, bool>> GetTypedPredicateExpressionFunc<TOther>(Expression<Func<T, object, bool>> expression) {
    //  var xParam = Expression.Parameter(typeof(T), expression.Parameters[0].Name);
    //  var otherParam = Expression.Parameter(typeof(TOther), expression.Parameters[1].Name);
    //  var call = Expression.Invoke(expression, xParam, otherParam);
    //  return Expression.Lambda<Func<T, TOther, bool>>(call, xParam, otherParam);
    //}

  }

  public class ExpressionRebinder : ExpressionVisitor {
    private readonly Dictionary<ParameterExpression, ParameterExpression> map;

    public ExpressionRebinder(Dictionary<ParameterExpression, ParameterExpression> map) {
      this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression ReplacementExpression(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
      => new ExpressionRebinder(map).Visit(exp);

    protected override Expression VisitParameter(ParameterExpression node) {
      ParameterExpression replacement;
      if (map.TryGetValue(node, out replacement)) {
        node = replacement;
      }
      return base.VisitParameter(node);
    }
  }


}
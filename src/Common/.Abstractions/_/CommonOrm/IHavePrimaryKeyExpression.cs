using System;
using System.Linq.Expressions;

namespace CommonOrm {
  public interface IHavePrimaryKeyExpression<T> {
    Expression<Func<T, object>> PrimaryKeyExpression { get; }
  }
}
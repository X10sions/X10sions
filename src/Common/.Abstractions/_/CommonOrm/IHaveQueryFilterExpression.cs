using System;
using System.Linq.Expressions;

namespace CommonOrm {
  public interface IHaveQueryFilterExpression<T> {
    Expression<Func<T, bool>>? QueryFilter { get; set; }
  }
}
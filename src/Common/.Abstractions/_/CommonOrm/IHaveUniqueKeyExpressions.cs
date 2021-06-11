using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonOrm {
  public interface IHaveUniqueKeyExpressions<T> {
    List<Expression<Func<T, object>>> UniqueKeyExpressions { get; }
  }

}
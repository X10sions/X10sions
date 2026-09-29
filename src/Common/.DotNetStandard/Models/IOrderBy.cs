using System;
using System.Linq.Expressions;

namespace Common.Models {
  public interface IOrderBy {
    dynamic DynamicExpression { get; }
    //public Expression<Func<TToOrder, TBy>> TypedExpression { get; }
  }

  public interface IOrderByWithDirection : IOrderBy {
    bool IsDescending { get; }
  }

  public class OrderBy<TToOrder, TBy> : IOrderBy {
    public OrderBy(Expression<Func<TToOrder, TBy>> expression) {
      TypedExpression = expression;
    }
    public dynamic DynamicExpression => TypedExpression;
    public Expression<Func<TToOrder, TBy>> TypedExpression { get; }
  }

  public class OrderByWithDirection<TToOrder, TBy> : OrderBy<TToOrder, TBy>, IOrderByWithDirection {
    public OrderByWithDirection(Expression<Func<TToOrder, TBy>> expression, bool isDescending) : base(expression) {
      IsDescending = isDescending;
    }
    public bool IsDescending { get; set; }
  }

  //public class xOrderBy<T> : IOrderBy {
  //  public xOrderBy(Expression<Func<SearchResultItem, T>> expression) {
  //    this.expression = expression;
  //  }
  //  private readonly Expression<Func<SearchResultItem, T>> expression;
  //  public dynamic Expression => this.expression;
  //}
}

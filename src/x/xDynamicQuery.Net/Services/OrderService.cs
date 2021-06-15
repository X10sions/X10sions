using DynamicQueryNet.Enums;
using DynamicQueryNet.Inputs;
using DynamicQueryNet.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicQueryNet.Services {
  public static class OrderService {

    public static IOrderedQueryable<T> Ordering<T>(IQueryable<T> input, List<OrderInput> orderInputs) {
      var parameter = Expression.Parameter(typeof(T), "p");
      var firstOrderInput = orderInputs[0];
      var result = firstOrderInput.Order == OrderTypeEnum.Asc ? OrderingHelper.OrderBy(input, firstOrderInput.PropertyName, parameter) : OrderingHelper.OrderByDescending(input, firstOrderInput.PropertyName, parameter);
      var loopTo = orderInputs.Count - 1;
      for (int i = 1; i <= loopTo; i++) {
        var orderInput = orderInputs[i];
        result = orderInput.Order == OrderTypeEnum.Asc ? OrderingHelper.ThenBy(result, orderInput.PropertyName, parameter) : OrderingHelper.ThenByDescending(result, orderInput.PropertyName, parameter);
      }
      return result;
    }

    public static IOrderedQueryable<T> Ordering<T>(IQueryable<T> input, OrderInput orderInput) {
      var parameter = Expression.Parameter(typeof(T), "p");
      return orderInput.Order == OrderTypeEnum.Asc ? OrderingHelper.OrderBy(input, orderInput.PropertyName, parameter) : OrderingHelper.OrderByDescending(input, orderInput.PropertyName, parameter);
    }

  }
}

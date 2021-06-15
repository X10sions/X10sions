using DynamicQueryNet.Inputs;
using DynamicQueryNet.Services;
using System.Collections.Generic;

namespace System.Linq {

  public static class IQueryable_NasirJafarzadeh_Extensions {

    public static IQueryable<T> DynamicQueryNetFilter<T>(this IQueryable<T> input, List<FilterInput> filterInputs) => filterInputs != null ? FilterService.Filter(input, filterInputs) : input;
    public static IQueryable<T> DynamicQueryNetFilter<T>(this IQueryable<T> input, FilterInput filterinput) => filterinput != null ? FilterService.Filter(input, filterinput) : input;
    public static IOrderedQueryable<T> DynamicQueryNetFilter<T>(this IQueryable<T> input, OrderFilterInput orderFilterInput) => (IOrderedQueryable<T>)((IOrderedQueryable<T>)orderFilterInput != null ? input.DynamicQueryNetFilter(orderFilterInput.Filter).DynamicQueryNetOrder(orderFilterInput.Order) : input);
    public static IOrderedQueryable<T> DynamicQueryNetFilter<T>(this IQueryable<T> input, OrderFilterNonFilterInput orderFilterInput) => (IOrderedQueryable<T>)((IOrderedQueryable<T>)orderFilterInput != null ? input.DynamicQueryNetFilter(orderFilterInput.Filter).DynamicQueryNetOrder(orderFilterInput.Order) : input);
    public static IOrderedQueryable<T> DynamicQueryNetFilter<T>(this IQueryable<T> input, DynamicQueryNetInput dynamicInput) => dynamicInput != null ? input.DynamicQueryNetFilter(dynamicInput.Filter).DynamicQueryNetOrder(dynamicInput.Order).DynamicQueryNetPaging(dynamicInput.Paging) : (IOrderedQueryable<T>)input;
    public static IOrderedQueryable<T> DynamicQueryNetOrder<T>(this IQueryable<T> input, OrderInput orderInput) => orderInput != null ? OrderService.Ordering(input, orderInput) : (IOrderedQueryable<T>)input;
    public static IOrderedQueryable<T> DynamicQueryNetOrder<T>(this IQueryable<T> input, List<OrderInput> orderInput) => orderInput != null ? OrderService.Ordering(input, orderInput) : (IOrderedQueryable<T>)input;
    public static IQueryable<T> DynamicQueryNetPaging<T>(this IQueryable<T> input, PagingInput paging__1) => paging__1 != null ? input.Skip(paging__1.Page * paging__1.Size).Take(paging__1.Size) : input;
    public static IOrderedQueryable<T> DynamicQueryNetPaging<T>(this IOrderedQueryable<T> input, PagingInput paging__1) => (IOrderedQueryable<T>)((IOrderedQueryable<T>)paging__1 != null ? input.Skip(paging__1.Page * paging__1.Size).Take(paging__1.Size) : input);

  }
}
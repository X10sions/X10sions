﻿using DynamicQueryNet.Enums;
using DynamicQueryNet.Inputs;
using DynamicQueryNet.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace DynamicQueryNet.Services {
  public static class FilterService {
    public static IQueryable<T> Filter<T>(IQueryable<T> dataInput, List<FilterInput> filterInputs) {
      var parameter = Expression.Parameter(typeof(T), "p");
      Expression resultExpr = Expression.Constant(true);
      foreach (var filterInput in filterInputs) {
        if (filterInput.Value is IEnumerable<object>) {
          var valueArrayExpression = MultipleValueHandleExpression<T>(filterInput, parameter);
          if (valueArrayExpression != null)
            resultExpr = Expression.AndAlso(resultExpr, valueArrayExpression);
        } else
          resultExpr = Expression.AndAlso(resultExpr, FilterExpression<T>(filterInput, parameter));
      }
      return dataInput.Where(Expression.Lambda<Func<T, bool>>(resultExpr, parameter));
    }

    public static IQueryable<T> Filter<T>(IQueryable<T> dataInput, FilterInput filterInput) {
      var parameter = Expression.Parameter(typeof(T), "p");
      if (filterInput.Value is IEnumerable<object>) {
        var valueArrayExpression = MultipleValueHandleExpression<T>(filterInput, parameter);
        if (valueArrayExpression != null)
          return dataInput.Where(Expression.Lambda<Func<T, bool>>(valueArrayExpression, parameter));
      }
      return dataInput.Where(Expression.Lambda<Func<T, bool>>(FilterExpression<T>(filterInput, parameter), parameter));
    }

    public static Expression FilterExpression<T>(FilterInput filterInput, ParameterExpression parameter) {
      var propertyExpr = Expression.Property(parameter, filterInput.PropertyName);
      var value = Expression.Constant(filterInput.Value);
      ICompare compare;
      switch (filterInput.Type) {
        case InputTypeEnum.Number:
        case InputTypeEnum.Boolean: {
            compare = new NormalCompare();
            break;
          }

        case InputTypeEnum.String: {
            compare = new StringCompare();
            break;
          }

        default: {
            compare = new NormalCompare();
            break;
          }
      }
      var compareInput = new CompareInput() {
        PropertyExpr = propertyExpr,
        Value = value
      };
      Expression resultExpr;
      switch (filterInput.Operation) {
        case OperationTypeEnum.Equal: {
            resultExpr = compare.Equal<T>(compareInput);
            break;
          }

        case OperationTypeEnum.NotEqual: {
            resultExpr = compare.NotEqual<T>(compareInput);
            break;
          }

        case OperationTypeEnum.GreaterThan: {
            resultExpr = compare.GreaterThan<T>(compareInput);
            break;
          }

        case OperationTypeEnum.GreaterThanOrEqual: {
            resultExpr = compare.GreaterThanOrEqual<T>(compareInput);
            break;
          }

        case OperationTypeEnum.LessThan: {
            resultExpr = compare.LessThan<T>(compareInput);
            break;
          }

        case OperationTypeEnum.LessThanOrEqual: {
            resultExpr = compare.LessThanOrEqual<T>(compareInput);
            break;
          }

        case OperationTypeEnum.Contain: {
            resultExpr = compare.Contains<T>(compareInput);
            break;
          }

        default: {
            resultExpr = compare.Equal<T>(compareInput);
            break;
          }
      }
      return resultExpr;
    }

    private static Expression MultipleValueHandleExpression<T>(FilterInput filterInput, ParameterExpression parameter) {
      var timer = Stopwatch.StartNew();
      Expression valueArrayExpression = null;
      var isJarray = JArrayUtil.IsJArray(filterInput.Value);
      var values = filterInput.Value as IEnumerable<object>;
      if (values == null)
        return Expression.Empty();
      foreach (var value in values) {
        filterInput.Value = isJarray ? ((JValue)value).Value : value;
        if (valueArrayExpression == null) {
          valueArrayExpression = FilterExpression<T>(filterInput, parameter);
          continue;
        }
        if (filterInput.Operation == OperationTypeEnum.NotEqual)
          valueArrayExpression = Expression.AndAlso(valueArrayExpression, FilterExpression<T>(filterInput, parameter));
        else
          valueArrayExpression = Expression.OrElse(valueArrayExpression, FilterExpression<T>(filterInput, parameter));
      }
      timer.Stop();
      Trace.Write(timer.ElapsedTicks);
      return valueArrayExpression;
    }
  }
}

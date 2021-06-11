using Microsoft.AspNetCore.Identity;
using System;
using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Mvc.ModelBinding {
  public static class ModelStateDictionaryExtensions {

    //public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, string errorMessage) => modelState.AddModelError(ExpressionHelper.GetExpressionText(expression), errorMessage);

    //public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, Exception exception) => modelState.AddModelError(ExpressionHelper.GetExpressionText(expression), exception);

    public static void AddError(this ModelStateDictionary modelState, string errorMessage) => modelState.AddModelError(string.Empty, errorMessage);

    public static void AddErrors(this ModelStateDictionary modelState, IdentityResult result) {
      foreach (var error in result.Errors) {
        modelState.AddModelError(string.Empty, error.Description);
      }
    }

    public static ModelStateEntry HandlerModelStateEntry(this ModelStateDictionary modelState) => modelState["handler"];
    public static object HandlerRawValue(this ModelStateDictionary modelState) => modelState.HandlerModelStateEntry().RawValue;
    public static string HandlerAttemptedValue(this ModelStateDictionary modelState) => modelState.HandlerModelStateEntry().AttemptedValue;

  }
}
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;
public static class ModelStateDictionaryExtensions {

  public static void AddError(this ModelStateDictionary modelState, string errorMessage) => modelState.AddModelError(string.Empty, errorMessage);

  public static void AddErrors(this ModelStateDictionary modelState, IdentityResult result) {
    foreach (var error in result.Errors) {
      modelState.AddModelError(string.Empty, error.Description);
    }
  }
  public static void AddModelError<TViewModel, TProperty>(this ModelStateDictionary modelState, Expression<Func<TViewModel, TProperty>> lambdaExpression, string error) => modelState.AddModelError(lambdaExpression.GetMemberName(), error);
  //public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, string errorMessage) => modelState.AddModelError(ExpressionHelper.GetExpressionText(expression), errorMessage);
  //public static void AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, Exception exception) => modelState.AddModelError(ExpressionHelper.GetExpressionText(expression), exception);

  public static ModelStateDictionary AddModelErrorIf(this ModelStateDictionary modelState, bool errorIfTrue, string key, string errorMessage) {
    if (errorIfTrue) {
      modelState.AddModelError(key, errorMessage);
    }
    return modelState;
  }

  public static List<string> ErrorKeyList(this ModelStateDictionary modelState) {
    var list = new List<string>();
    if (!modelState.IsValid) {
      foreach (var result in modelState.Keys.SelectMany(
          key => modelState[key].Errors.Select(x => key + ": " + x.ErrorMessage))) {
        list.Add(result);
      }
    }
    return list;
  }

  public static string ErrorKeyString(this ModelStateDictionary modelState) {
    var sb = new StringBuilder();
    foreach (var error in modelState.ErrorKeyList()) {
      sb.AppendLine(error);
    }
    return sb.ToString();
  }


  public static ModelStateEntry HandlerModelStateEntry(this ModelStateDictionary modelState) => modelState["handler"];
  public static object HandlerRawValue(this ModelStateDictionary modelState) => modelState.HandlerModelStateEntry().RawValue;
  public static string HandlerAttemptedValue(this ModelStateDictionary modelState) => modelState.HandlerModelStateEntry().AttemptedValue;

}
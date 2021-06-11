using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Identity {
  public static class IdentityResultExtensions {

    public static void AddErrors(this IdentityResult result, ModelStateDictionary modelState) => modelState.AddErrors(result);

  }
}
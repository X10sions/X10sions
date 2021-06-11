using System.Text;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures {
  public static class FormContextExtensions {

    public static string DebugString(this FormContext formContext, bool includeChildren = false) {
      var sb = new StringBuilder();
      sb.AppendLine($"  FormContext:");
      sb.AppendLine($"    .CanRenderAtEndOfForm: {formContext.CanRenderAtEndOfForm}");
      sb.AppendLine($"    .FormData: {formContext.FormData}");
      sb.AppendLine($"    .HasAntiforgeryToken: {formContext.HasAntiforgeryToken}");
      sb.AppendLine($"    .HasEndOfFormContent: {formContext.HasEndOfFormContent}");
      sb.AppendLine($"    .HasFormData: {formContext.HasFormData}");
      return sb.ToString();
    }

  }
}

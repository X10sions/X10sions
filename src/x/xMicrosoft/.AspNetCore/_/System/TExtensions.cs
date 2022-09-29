using System.ComponentModel.DataAnnotations;

namespace System {
  public static class TExtensions {

    public static IEnumerable<string> GetValidationErrorMessages<T>(this T instance) where T : class 
      => from x in instance.GetValidationRsults() where !string.IsNullOrWhiteSpace(x.ErrorMessage) select x.ErrorMessage;

    public static IEnumerable<ValidationResult> GetValidationRsults<T>(this T instance) where T : class {
      var context = new ValidationContext(instance, null, null);
      var results = new List<ValidationResult>();
      Validator.TryValidateObject(instance, context, results, true);
      return results;
    }

  }
}

using System.ComponentModel.DataAnnotations;

namespace System {
  public static class TExtensions {

    public static IEnumerable<string> GetValidationAttributeErrors<T>(this T instance) {
      var context = new ValidationContext(instance, serviceProvider: null, items: null);
      var results = new List<ValidationResult>();
      Validator.TryValidateObject(instance, context, results, true);
      return from x in results where !string.IsNullOrWhiteSpace(x.ErrorMessage) select x.ErrorMessage;
    }

  }
}
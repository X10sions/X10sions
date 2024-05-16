namespace System.ComponentModel.DataAnnotations;
public static class DataAnnotationsExtensions {

  public static ICollection<ValidationResult> GetValidationResults<T>(this T instance) {
    var results = new List<ValidationResult>();
    Validator.TryValidateObject(instance, new ValidationContext(instance), results, true);
    return results;
  }

  public static (bool, List<ValidationResult>) TryValidate<T>(this T instance) {
    var validationContext = new ValidationContext(instance);
    var validationResults = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(instance, validationContext,  validationResults, true);
    return (isValid, validationResults);
  }

  public static bool TryValidate<T>(this T instance, out ICollection<ValidationResult> results) {
    results = new List<ValidationResult>();
    return Validator.TryValidateObject(instance, new ValidationContext(instance), results, true);
  }

}
namespace FluentValidation.Results;

public static class ValidationFailureExtensions {

  public static IDictionary<string, string[]> ToDictionary1(this IEnumerable<ValidationFailure> failures) {
    return failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
      .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
  }

  public static IDictionary<string, string[]> ToDictionary2(this IEnumerable<ValidationFailure> failures) {
    var dic = new Dictionary<string, string[]>();
    var propertyNames = failures.Select(e => e.PropertyName).Distinct();
    foreach (var propertyName in propertyNames) {
      var propertyFailures = failures.Where(e => e.PropertyName == propertyName).Select(e => e.ErrorMessage).ToArray();
      dic.Add(propertyName, propertyFailures);
    }
    return dic;
  }

}
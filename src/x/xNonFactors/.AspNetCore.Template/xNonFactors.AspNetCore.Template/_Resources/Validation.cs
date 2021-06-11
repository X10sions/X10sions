namespace xNonFactors.MvcTemplate.Resources {
  public static class Validation {

    public static string For(string key, params object?[] args) {
      var validation = Resource.Localized("Form", "Validations", key);
      return string.IsNullOrEmpty(validation) || args.Length == 0 ? validation : string.Format(validation, args);
    }

    public static string For<TView>(string key, params object?[] args) {
      var validation = Resource.Localized(typeof(TView).Name, "Validations", key);
      return string.IsNullOrEmpty(validation) || args.Length == 0 ? validation : string.Format(validation, args);
    }

  }
}
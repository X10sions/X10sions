namespace Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public interface IClientModelValidatorWithJavascript : IClientModelValidator {
  string ClientSideJavascript { get; }
  string JavascriptMethodName { get; }

}

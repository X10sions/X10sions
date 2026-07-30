namespace Common.Results;

public readonly record struct ResultError(string Code, string? Description = null, ResultErrorType Type = ResultErrorType.Unknown, string? StackTrace = null) {
  public static readonly ResultError None = new(string.Empty);
  public static readonly ResultError Unspecified = new("Unspecified Error");
  public static readonly ResultError Validation = new("Validation Error", null, ResultErrorType.Validation);
  public static readonly ResultError NullValue = new("Null Value Error");
  public static ResultError Unexpected(string code, string description) => new(code, description, StackTrace: Environment.StackTrace);

  //public static implicit operator Result(Error error) => Result.Fail( null, [error]);
}

public enum ResultErrorType { Unknown, Conflict, NotFound, Problem, Validation }


public static class CustomErrors {
  public static readonly ResultError DataNotUpdatedException = new("Data Not Updated.");
  public static readonly ResultError BusinessException = new("Data Not Updated.");
  public static ResultError UserNotFound<T>(T userId) => new($"User id '{userId}' not found.", null, ResultErrorType.NotFound);
}

public static class ResultErrorExtensions {
  public static List<T> Validate<T>(this List<T> errors, Func<bool> invalidCondition, Func<T> getError) {
    if (invalidCondition())
      errors.Add(getError());
    return errors;
  }

  public static List<string> Validate(this List<string> errors, Func<bool> invalidCondition, string errorMessage) => errors.Validate(invalidCondition, () => errorMessage);
  public static List<ResultError> Validate(this List<ResultError> errors, Func<bool> invalidCondition, Func<string> getErrorMessage) => errors.Validate(invalidCondition, getErrorMessage());

  public static List<ResultError> Validate(this List<ResultError> errors, Func<bool> invalidCondition, string errorMessage) => errors.Validate(invalidCondition, () => new ResultError(errorMessage));



}

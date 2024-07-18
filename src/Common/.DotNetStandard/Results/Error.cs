namespace Common.Results;

public sealed record Error(string Code, string? Description = null, ErrorType Type = ErrorType.Unknown) {
  public static readonly Error None = new(string.Empty);
  public static readonly Error Unspecified = new("Unspecified Error");
  public static readonly Error Validation = new("Validation Error",null, ErrorType.Validation);
  public static readonly Error NullValue = new("Null Value Error");

  //public static implicit operator Result(Error error) => Result.Fail( null, [error]);
}

public enum ErrorType {Unknown, Conflict, NotFound, Problem, Validation }


public static class CustomErrors {
  public static readonly Error DataNotUpdatedException = new("Data Not Updated.");
  public static readonly Error BusinessException = new("Data Not Updated.");
  public static Error UserNotFound<T>(T userId) => new($"User id '{userId}' not found.",null, ErrorType.NotFound);

}

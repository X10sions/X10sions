using System.Net;

namespace Common.Results;

/// <summary>Microsoft.AspNetCore.Http.Results.Problem</summary>
public record HttpProblem(string Title, string Detail, string Type, HttpStatusCode StatusCode, IEnumerable<Error> Extensions) ;

public static class ApiResults {

  static string GetTitle(Error error) => error.Type switch {
    ErrorType.Conflict => error.Code,
    ErrorType.NotFound => error.Code,
    ErrorType.Problem => error.Code,
    ErrorType.Validation => error.Code,
    ErrorType.Unknown => error.Code,
    _ => "Server Failure",
  };

  static string GetDetail(Error error) => error.Type switch {
    ErrorType.Conflict => error.Description,
    ErrorType.NotFound => error.Description,
    ErrorType.Problem => error.Description,
    ErrorType.Validation => error.Description,
    ErrorType.Unknown => error.Description,
    _ => "An unexpected error occurred",
  };

  static string GetType(Error error) => error.Type switch {
    ErrorType.Conflict => "https://tools.org/html/rcf7231#section-?.?.?",
    ErrorType.NotFound => "https://tools.org/html/rcf7231#section-?.?.?",
    ErrorType.Problem => "https://tools.org/html/rcf7231#section-?.?.?",
    ErrorType.Validation => "https://tools.org/html/rcf7231#section-?.?.?",
    ErrorType.Unknown => "https://tools.org/html/rcf7231#section-?.?.?",
    _ => "https://tools.org/html/rcf7231#section-?.?.?",
  };

  static HttpStatusCode GetStatusCode(Error error) => error.Type switch {
    ErrorType.Conflict => HttpStatusCode.Conflict,
    ErrorType.NotFound => HttpStatusCode.NotFound,
    ErrorType.Problem => HttpStatusCode.BadRequest,
    ErrorType.Validation => HttpStatusCode.BadRequest,
    ErrorType.Unknown => HttpStatusCode.InternalServerError,
    _ => HttpStatusCode.InternalServerError,
  };

  static Dictionary<string, object?> GetErrors(IResult result) {
    var validationErrors = result.Errors.Where(x => x.Type == ErrorType.Validation);
    if (!validationErrors.Any()) {
      return [];
    }
    return new Dictionary<string, object?>{
      { "errors", validationErrors }
    };
  }

  public static HttpProblem Problem(Result result) {
    if (result.IsSuccess()) throw new InvalidOperationException();
    var error = result.Errors.FirstOrDefault();
    return new HttpProblem(
      GetTitle(error),
      GetDetail(error),
      GetType(error),
      GetStatusCode(error),
      result.Errors
      );

  }

}
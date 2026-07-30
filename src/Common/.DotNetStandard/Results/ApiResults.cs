using System.Net;

namespace Common.Results;

/// <summary>Microsoft.AspNetCore.Http.Results.Problem</summary>
public record HttpProblem(string Title, string Detail, string Type, HttpStatusCode StatusCode, IEnumerable<ResultError> Extensions);

public static class ApiResults {

  static string GetTitle(ResultError error) => error.Type switch {
    ResultErrorType.Conflict => error.Code,
    ResultErrorType.NotFound => error.Code,
    ResultErrorType.Problem => error.Code,
    ResultErrorType.Validation => error.Code,
    ResultErrorType.Unknown => error.Code,
    _ => "Server Failure",
  };

  static string GetDetail(ResultError error) => error.Type switch {
    ResultErrorType.Conflict => error.Description,
    ResultErrorType.NotFound => error.Description,
    ResultErrorType.Problem => error.Description,
    ResultErrorType.Validation => error.Description,
    ResultErrorType.Unknown => error.Description,
    _ => "An unexpected error occurred",
  };

  static string GetType(ResultError error) => error.Type switch {
    ResultErrorType.Conflict => "https://tools.org/html/rcf7231#section-6.5.8",
    ResultErrorType.NotFound => "https://tools.org/html/rcf7231#section-6.5.4",
    ResultErrorType.Problem => "https://tools.org/html/rcf7231#section-6.6.1",
    ResultErrorType.Validation => "https://tools.org/html/rcf7231#section-6.5.1",
    ResultErrorType.Unknown => "https://tools.org/html/rcf7231#section-?.?.?",
    _ => "https://tools.org/html/rcf7231#section-?.?.?",
  };

  static HttpStatusCode GetStatusCode(ResultError error) => error.Type switch {
    ResultErrorType.Conflict => HttpStatusCode.Conflict,
    ResultErrorType.NotFound => HttpStatusCode.NotFound,
    ResultErrorType.Problem => HttpStatusCode.BadRequest,
    ResultErrorType.Validation => HttpStatusCode.BadRequest,
    ResultErrorType.Unknown => HttpStatusCode.InternalServerError,
    _ => HttpStatusCode.InternalServerError,
  };

  static Dictionary<string, object?> GetErrors(IResult result) {
    var validationErrors = result.Errors.Where(x => x.Type == ResultErrorType.Validation);
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
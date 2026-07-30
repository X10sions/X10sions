using Common.ValueObjects;

namespace Common.Results;

/// <summary>
/// LanguageExt
/// FluentResults
/// Ardalis.Result
/// ErrorOr, OneOf
/// CSharpFunctionalExtensions
/// </summary>
public readonly record struct Result : IResult {

  internal Result(string? message, params ResultError[] errors) {
    Message = message;
    Errors.AddRange(errors.Where(x => x != ResultError.None));
  }

  public List<ResultError> Errors { get; } = new();
  public string? Message { get; }

  public static Result Fail(ResultError error) => new Result(null, [error]);
  public static Result Fail(string? message = null, params ResultError[] errors) => new Result(message, errors ?? [ResultError.Unspecified]);
  public static Task<Result> FailAsync(string? message = null, params ResultError[] errors) => Task.FromResult(Fail(message, errors));
  public static Result Success(string? message = null) => new Result(message, []);
  public static Result<TValue> Success<TValue>(TValue value, string? message = null) => new Result<TValue>(value, message, []);
  public static Result<TValue> Fail<TValue>(ResultError error) => new Result<TValue>(default, null, [error]);
  public static Task<Result> SuccessAsync(string? message = null) => Task.FromResult(Success(message));
}

public readonly record struct Result<T> : IResult<T> {
  internal Result(T value, string? message, params ResultError[] errors) {
    Value = value;
    Message = message;
    Errors.AddRange(errors.Where(x => x != ResultError.None));
  }
  public List<ResultError> Errors { get; } = new();
  public string? Message { get; }
  public T Value { get; }

  public static Result<T> Fail(T data, string? message = null, params ResultError[] errors) => new Result<T>(data, message, errors);
  public static Task<Result<T>> FailAsync(T data, string? message = null, params ResultError[] errors) => Task.FromResult(Fail(data, message, errors));
  public static Result<T> Success(T data, string? message = null) => new Result<T>(data, message, []);
  public static Task<Result<T>> SuccessAsync(T data, string? message = null) => Task.FromResult(Success(data, message));

  public static Result<T> ValidationFailure<T>(ResultError error) => new(default, null, [error]);

  //public static implicit operator Result<T>(T? value) => value is not null ? Success(value) : Fail<T>(ResultError.NullValue);

}

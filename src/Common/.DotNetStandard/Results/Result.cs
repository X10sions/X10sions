using Common.ValueObjects;

namespace Common.Results;

/// <summary>
/// LanguageExt
/// FluentResults
/// Ardalis.Result
/// ErrorOr, OneOf
/// CSharpFunctionalExtensions
/// </summary>
public class Result : IResult {

  internal Result(string? message, IEnumerable<Error> errors) {
    Message = message;
    Errors.AddRange(errors.Where(x => x != Error.None));
  }

  public List<Error> Errors { get; } = new();
  public string? Message { get; }

  public static Result Fail(Error error) => new Result(null, [error]);
  public static Result Fail(string? message = null, IEnumerable<Error>? errors = null) => new Result(message, errors ?? [Error.Unspecified]);
  public static Task<Result> FailAsync(string? message = null, IEnumerable<Error>? errors = null) => Task.FromResult(Fail(message, errors));
  public static Result Success(string? message = null) => new Result(message, []);
  public static Result<TValue> Success<TValue>(TValue value, string? message = null) => new Result<TValue>(value, message, []);
  public static Result<TValue> Fail<TValue>(Error error) => new Result<TValue>(default, null, [error]);
  public static Task<Result> SuccessAsync(string? message = null) => Task.FromResult(Success(message));
}

public class Result<T> : Result, IResult<T>, IValueObject<T> {
  internal Result(T value, string? message, IEnumerable<Error> errors) : base(message, errors) {
    Value = value;
  }

  public T Value { get; }

  //public new static IResult<T> Fail(string? message = null, IEnumerable<Error>? errors = null) => new Result<T>(default, message, errors ?? [Error.Unspecified]);
  public static Result<T> Fail(T data, string? message = null, IEnumerable<Error>? errors = null) => new Result<T>(data, message, errors ?? [Error.Unspecified]);
  //public new static Task<IResult<T>> FailAsync(string? message = null, IEnumerable<Error>? errors = null) => Task.FromResult(Fail(message, errors));
  public static Task<Result<T>> FailAsync(T data, string? message = null, IEnumerable<Error>? errors = null) => Task.FromResult(Fail(data, message, errors));
  //public new static IResult<T> Success(string? message = null) => new Result<T>(message, []);
  public static Result<T> Success(T data, string? message = null) => new Result<T>(data, message, []);
  //public new static Task<IResult<T>> SuccessAsync(string? message = null) => Task.FromResult(Success(message));
  public static Task<Result<T>> SuccessAsync(T data, string? message = null) => Task.FromResult(Success(data, message));

  public static Result<T> ValidationFailure<T>(Error error) => new(default, null, [error]);

  public static implicit operator Result<T>(T? value) => value is not null ? Success(value) : Fail<T>(Error.NullValue);

}

namespace Common.Results;

//public record BaseResponseDTO(bool IsSuccess, string[] Errors);

public class Result : IResult {
  public Result() { }

  public Result(string? message = null, IEnumerable<string>? errors = null) {
    Message = message;
    if (errors != null) Errors.AddRange(errors);
  }

  public List<string> Errors { get; set; } = new List<string>();

  public bool Failed => !Succeeded;
  public string? Message { get; set; }
  public bool Succeeded { get; set; }

  public static IResult Fail(string? message = null) => new Result { Succeeded = false, Message = message };
  //public static IResult Fail(string? message = null) => new Result(message, new[] { nameof(Fail) });
  public static Task<IResult> FailAsync(string? message = null) => Task.FromResult(Fail(message));
  public static IResult Success(string? message = null) => new Result { Succeeded = true, Message = message };
  public static Task<IResult> SuccessAsync(string? message = null) => Task.FromResult(Success(message));
}

public class Result<T> : Result, IResult<T> {
  public Result() { }
  public Result(string? message = null, IEnumerable<string>? errors = null) {
    Message = message;
    if (errors != null) Errors.AddRange(errors);
  }
  public Result(T data, string? message = null, IEnumerable<string>? errors = null) : this(message, errors) {
    Data = data;
  }

  public T Data { get; set; }

  public static new IResult<T> Fail(string? message = null) => new Result<T> { Succeeded = false, Message = message };
  //public new static IResult<T> Fail(string? message = null) => new Result<T>(message, new[] { nameof(Fail) });
  public static IResult<T> Fail(T data, string? message = null) => new Result<T>(data, message, new[] { nameof(Fail) });
  public static new Task<IResult<T>> FailAsync(string? message = null) => Task.FromResult(Fail(message));
  public static Task<IResult<T>> FailAsync(T data, string? message = null) => Task.FromResult(Fail(data, message));
  public static new IResult<T> Success(string? message = null) => new Result<T> { Succeeded = true, Message = message };
  public static IResult<T> Success(T data, string? message = null) => new Result<T> { Succeeded = true, Data = data, Message = message };
  public static new Task<IResult<T>> SuccessAsync(string? message = null) => Task.FromResult(Success(message));
  public static Task<IResult<T>> SuccessAsync(T data, string? message = null) => Task.FromResult(Success(data, message));
}

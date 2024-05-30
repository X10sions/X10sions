namespace Common.Data;

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


public class Response<T> {
  public Response() { }
  public Response(T data, string? message = null) {
    Succeeded = true;
    Message = message;
    Data = data;
  }
  public Response(string message) {
    Succeeded = false;
    Message = message;
  }
  public bool Succeeded { get; set; }
  public string? Message { get; set; }
  public List<string> Errors { get; set; } = new List<string>();
  public T? Data { get; set; }
}

public class PagedResponse<T> : Response<T> {
  public PagedResponse(T data, int pageNumber, int pageSize) : base(data, null) {
    PageNumber = pageNumber;
    PageSize = pageSize;
    Succeeded = true;
  }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  //public Uri FirstPage { get; set; }
  //public Uri LastPage { get; set; }
  //public int TotalPages { get; set; }
  //public int TotalRecords { get; set; }
  //public Uri NextPage { get; set; }
  //public Uri PreviousPage { get; set; }

}

public class PaginatedResult<T> : Result {
  public PaginatedResult(List<T> data) {
    Data = data;
  }

  public List<T> Data { get; set; }

  internal PaginatedResult(bool succeeded, List<T> data = default, List<string>? messages = null, long count = 0, int page = 1, int pageSize = 10) {
    Data = data;
    Page = page;
    Succeeded = succeeded;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    TotalCount = count;
  }

  public static PaginatedResult<T> Failure(List<string> messages) => new PaginatedResult<T>(false, default, messages);
  public static PaginatedResult<T> Success(List<T> data, long count, int page, int pageSize) => new PaginatedResult<T>(true, data, null, count, page, pageSize);

  public int Page { get; set; }
  public int TotalPages { get; set; }
  public long TotalCount { get; set; }
  public bool HasPreviousPage => Page > 1;
  public bool HasNextPage => Page < TotalPages;
}


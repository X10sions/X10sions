namespace Common.Results;
public interface IResult {
  string? Message { get; }
  List<Error> Errors { get; }
}

public interface IResult<out T> : IResult {
  T Value { get; }
}

public static class IResultExtensions {

  public static bool IsSuccess(this IResult result) => result.Errors.Count < 1;
  public static bool IsFail(this IResult result) => !result.IsSuccess();

  public static TOut Match<TOut>(this IResult result, Func<TOut> onSuccess, Func<IResult, TOut> onFail)
    => result.IsSuccess() ? onSuccess() : onFail(result);

  public static TOut Match<TIn, TOut>(this IResult<TIn> result, Func<TIn, TOut> onSuccess, Func<IResult<TIn>, TOut> onFail)
    => result.IsSuccess() ? onSuccess(result.Value) : onFail(result);

}
namespace Common.Results;
public interface IResult {
  string? Message { get; set; }
  List<string> Errors { get; set; }
}

public interface IResult<out T> : IResult {
  T Data { get; }
}

public static class ResultExtensions {

  public static bool Succeeded(this IResult result) => result.Errors.Count < 1;
  public static bool Failed(this IResult result) => !result.Succeeded();

}
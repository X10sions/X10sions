using System.Net;

namespace Common.Exceptions;
public class HandledException : Exception {
  public HandledException() { }
  public HandledException(string message) : base(message) { }
  public HandledException(string? message = null, string? title = null, HttpStatusCode? httpStatusCode = null) : base(message) {
    Title = title;
    if (httpStatusCode is not null) {
      HttpStatusCode = httpStatusCode.Value;
    }
  }
  public HandledException(string message, Exception inner) : base(message, inner) { }
  public HandledException(string format, params object[] args) : base(string.Format(format, args)) { }

  public string? Title { get; }
  public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.OK;
  public int StatusCode => (int)HttpStatusCode;
}

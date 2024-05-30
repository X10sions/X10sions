namespace Common.Exceptions;
public class NotAuthorisedException : UnauthorizedAccessException {
  public NotAuthorisedException() { }
  public NotAuthorisedException(string message) : base(message) { }
  public NotAuthorisedException(string message, Exception inner) : base(message, inner) { }
  public NotAuthorisedException(string format, params object[] args) : base(string.Format(format, args)) { }
}

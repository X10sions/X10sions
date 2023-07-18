namespace Common.Exceptions;
public class UnknownUserException : Exception {
  public UnknownUserException() { }
  public UnknownUserException(string message) : base(message) { }
  public UnknownUserException(string message, Exception inner) : base(message, inner) { }
  public UnknownUserException(string format, params object[] args) : base(string.Format(format, args)) { }
}
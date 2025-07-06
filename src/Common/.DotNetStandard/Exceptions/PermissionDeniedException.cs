namespace Common.Exceptions;
[Obsolete($"Use UnauthorizedAccessException")]
public class PermissionDeniedException : UnauthorizedAccessException {
  public PermissionDeniedException() { }
  public PermissionDeniedException(string message) : base(message) { }
  public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
  public PermissionDeniedException(string format, params object[] args) : base(string.Format(format, args)) { }
}
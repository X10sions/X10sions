using System;

namespace Common.Exceptions {
  public class PermissionDeniedException : Exception {
    public PermissionDeniedException() { }
    public PermissionDeniedException(string message) : base(message) { }
    public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
    public PermissionDeniedException(string format, params object[] args) : base(string.Format(format, args)) { }
  }
}
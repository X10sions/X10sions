using System;
using System.Runtime.Serialization;

namespace Common.Exceptions {
  public class PermissionDeniedException : Exception {
    public PermissionDeniedException() { }
    public PermissionDeniedException(string message) : base(message) { }
    public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
    public PermissionDeniedException(string format, params object[] args) : base(string.Format(format, args)) { }

    [Obsolete("cannot access HttpContext.Current")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);
      if (info != null) {
        info.AddValue("My.User.Name",  "HttpContext.Current.User.Name");
      }
    }
  }
}
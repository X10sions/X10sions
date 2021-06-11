using System;

namespace Common.Exceptions {
  public class HandledException : Exception {
    public HandledException() { }
    public HandledException(string message) : base(message) { }
    public HandledException(string message, Exception inner) : base(message, inner) { }
    public HandledException(string format, params object[] args) : base(string.Format(format, args)) { }
  }      
}
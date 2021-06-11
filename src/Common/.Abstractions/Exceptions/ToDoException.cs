using System;

namespace Common.Exceptions {
  [Obsolete("Work still to be done")]
  public class ToDoException : Exception {
    public ToDoException() { }
    public ToDoException(string message) : base(message) { }
    public ToDoException(string message, Exception inner) : base(message, inner) { }
    public ToDoException(string format, params object[] args) : base(string.Format(format, args)) { }
  }
}
